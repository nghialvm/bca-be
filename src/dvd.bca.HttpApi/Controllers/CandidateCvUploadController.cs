using dvd.bca.CandidateDocuments;
using dvd.bca.CandidateDocuments.Dtos;
using dvd.bca.Candidates;
using dvd.bca.Models.Candidate;
using dvd.bca.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;

namespace dvd.bca.Controllers
{
    [Route("api/app/candidate-document")]
    [Authorize(bcaPermissions.Recruitment.CandidateDocuments.Create)]
    public class CandidateCvUploadController : bcaController
    {
        private static readonly string[] AllowedExtensions = [".pdf"];
        private readonly ICurrentCandidateResolver _currentCandidateResolver;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICandidateDocumentAppService _candidateDocumentAppService;

        public CandidateCvUploadController(
            IWebHostEnvironment webHostEnvironment,
            ICandidateDocumentAppService candidateDocumentAppService,
            ICurrentCandidateResolver currentCandidateResolver)
        {
            _webHostEnvironment = webHostEnvironment;
            _candidateDocumentAppService = candidateDocumentAppService;
            _currentCandidateResolver = currentCandidateResolver;
        }

        [HttpPost("upload-cv")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<CandidateCvUploadResultDto> UploadCvAsync([FromForm] CandidateCvUploadInput input)
        {
            if (input.File == null || input.File.Length == 0)
            {
                throw new UserFriendlyException("CV file is required.");
            }

            var candidateId = await _currentCandidateResolver.NormalizeCandidateIdAsync(input.CandidateId);

            var extension = Path.GetExtension(input.File.FileName);
            if (string.IsNullOrWhiteSpace(extension) ||
                Array.IndexOf(AllowedExtensions, extension.ToLowerInvariant()) < 0)
            {
                throw new UserFriendlyException("Only PDF files are allowed.");
            }

            var uploadsRoot = Path.Combine(
                _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot"),
                "uploads",
                "cv",
                candidateId.ToString()
            );

            Directory.CreateDirectory(uploadsRoot);

            var storedFileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
            var filePath = Path.Combine(uploadsRoot, storedFileName);

            await using (var stream = input.File.OpenReadStream())
            await using (var target = System.IO.File.Create(filePath))
            {
                await stream.CopyToAsync(target);
            }

            var relativeUrl = $"/uploads/cv/{candidateId:D}/{storedFileName}";
            var absoluteUrl = $"{Request.Scheme}://{Request.Host}{relativeUrl}";

            var document = await _candidateDocumentAppService.CreateAsync(new CreateCandidateDocumentDto
            {
                CandidateId = candidateId,
                DocumentType = "CV",
                FileName = input.File.FileName,
                FilePath = absoluteUrl,
                FileSize = input.File.Length,
                ContentType = input.File.ContentType ?? "application/pdf",
                Description = input.Description
            });

            return new CandidateCvUploadResultDto
            {
                DocumentId = document.Id,
                FileName = document.FileName,
                FileUrl = absoluteUrl,
                FileSize = document.FileSize,
                ContentType = document.ContentType ?? "application/pdf"
            };
        }
    }
}
