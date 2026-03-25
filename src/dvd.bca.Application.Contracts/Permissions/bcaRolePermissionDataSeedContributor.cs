using dvd.bca.Permissions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace dvd.bca.Data
{
    public class bcaRolePermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IdentityRoleManager _roleManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public bcaRolePermissionDataSeedContributor(
            IIdentityRoleRepository roleRepository,
            IdentityRoleManager roleManager,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await CreateRoleIfNotExistsAsync("Admin", false);
            await CreateRoleIfNotExistsAsync("Employer", false);
            await CreateRoleIfNotExistsAsync("Candidate", true);

            await GrantAdminPermissionsAsync();
            await GrantEmployerPermissionsAsync();
            await GrantCandidatePermissionsAsync();
        }

        private async Task CreateRoleIfNotExistsAsync(string roleName, bool isDefault = false)
        {
            var role = await _roleRepository.FindByNormalizedNameAsync(roleName.ToUpperInvariant());

            if (role != null)
            {
                // Nếu role đã tồn tại thì vẫn cập nhật lại IsDefault cho đúng
                role.IsDefault = isDefault;
                (await _roleManager.UpdateAsync(role)).CheckErrors();
                return;
            }

            var newRole = new IdentityRole(_guidGenerator.Create(), roleName)
            {
                IsDefault = isDefault
            };

            (await _roleManager.CreateAsync(newRole)).CheckErrors();
        }

        private async Task GrantAdminPermissionsAsync()
        {
            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "Admin",
                new[]
                {
                    bcaPermissions.Recruitment.Departments.Default,
                    bcaPermissions.Recruitment.Departments.Create,
                    bcaPermissions.Recruitment.Departments.Update,
                    bcaPermissions.Recruitment.Departments.Delete,

                    bcaPermissions.Recruitment.JobPositions.Default,
                    bcaPermissions.Recruitment.JobPositions.Create,
                    bcaPermissions.Recruitment.JobPositions.Update,
                    bcaPermissions.Recruitment.JobPositions.Delete,

                    bcaPermissions.Recruitment.RecruitmentRequests.Default,
                    bcaPermissions.Recruitment.RecruitmentRequests.Create,
                    bcaPermissions.Recruitment.RecruitmentRequests.Update,
                    bcaPermissions.Recruitment.RecruitmentRequests.Delete,
                    bcaPermissions.Recruitment.RecruitmentRequests.Approve,
                    bcaPermissions.Recruitment.RecruitmentRequests.Reject,
                    bcaPermissions.Recruitment.RecruitmentRequests.Publish,
                    bcaPermissions.Recruitment.RecruitmentRequests.Close,

                    bcaPermissions.Recruitment.Candidates.Default,
                    bcaPermissions.Recruitment.Candidates.Create,
                    bcaPermissions.Recruitment.Candidates.Update,
                    bcaPermissions.Recruitment.Candidates.Delete,

                    bcaPermissions.Recruitment.CandidateDocuments.Default,
                    bcaPermissions.Recruitment.CandidateDocuments.Create,
                    bcaPermissions.Recruitment.CandidateDocuments.Update,
                    bcaPermissions.Recruitment.CandidateDocuments.Delete,

                    bcaPermissions.Recruitment.Applications.Default,
                    bcaPermissions.Recruitment.Applications.Create,
                    bcaPermissions.Recruitment.Applications.Update,
                    bcaPermissions.Recruitment.Applications.Delete,

                    bcaPermissions.Recruitment.ApplicationScreenings.Default,
                    bcaPermissions.Recruitment.ApplicationScreenings.Create,
                    bcaPermissions.Recruitment.ApplicationScreenings.Update,
                    bcaPermissions.Recruitment.ApplicationScreenings.Delete,

                    bcaPermissions.Recruitment.InterviewSchedules.Default,
                    bcaPermissions.Recruitment.InterviewSchedules.Create,
                    bcaPermissions.Recruitment.InterviewSchedules.Update,
                    bcaPermissions.Recruitment.InterviewSchedules.Delete,

                    bcaPermissions.Recruitment.InterviewEvaluations.Default,
                    bcaPermissions.Recruitment.InterviewEvaluations.Create,
                    bcaPermissions.Recruitment.InterviewEvaluations.Update,
                    bcaPermissions.Recruitment.InterviewEvaluations.Delete,

                    bcaPermissions.Recruitment.Offers.Default,
                    bcaPermissions.Recruitment.Offers.Create,
                    bcaPermissions.Recruitment.Offers.Update,
                    bcaPermissions.Recruitment.Offers.Delete,

                    bcaPermissions.Recruitment.Employees.Default,
                    bcaPermissions.Recruitment.Employees.Create,
                    bcaPermissions.Recruitment.Employees.Update,
                    bcaPermissions.Recruitment.Employees.Delete,

                    bcaPermissions.Recruitment.CandidateResponses.Default,
                    bcaPermissions.Recruitment.CandidateResponses.Create,
                    bcaPermissions.Recruitment.CandidateResponses.Update,
                    bcaPermissions.Recruitment.CandidateResponses.Delete,

                    bcaPermissions.Recruitment.Reports.Default
                }
            );
        }

        private async Task GrantEmployerPermissionsAsync()
        {
            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "Employer",
                new[]
                {
                    bcaPermissions.Recruitment.Departments.Default,
                    bcaPermissions.Recruitment.JobPositions.Default,

                    bcaPermissions.Recruitment.RecruitmentRequests.Default,
                    bcaPermissions.Recruitment.RecruitmentRequests.Create,
                    bcaPermissions.Recruitment.RecruitmentRequests.Update,
                    bcaPermissions.Recruitment.RecruitmentRequests.Approve,
                    bcaPermissions.Recruitment.RecruitmentRequests.Reject,
                    bcaPermissions.Recruitment.RecruitmentRequests.Publish,
                    bcaPermissions.Recruitment.RecruitmentRequests.Close,

                    bcaPermissions.Recruitment.Candidates.Default,
                    bcaPermissions.Recruitment.Candidates.Create,
                    bcaPermissions.Recruitment.Candidates.Update,

                    bcaPermissions.Recruitment.CandidateDocuments.Default,
                    bcaPermissions.Recruitment.CandidateDocuments.Create,
                    bcaPermissions.Recruitment.CandidateDocuments.Update,
                    bcaPermissions.Recruitment.CandidateDocuments.Delete,

                    bcaPermissions.Recruitment.Applications.Default,
                    bcaPermissions.Recruitment.Applications.Create,
                    bcaPermissions.Recruitment.Applications.Update,

                    bcaPermissions.Recruitment.ApplicationScreenings.Default,
                    bcaPermissions.Recruitment.ApplicationScreenings.Create,
                    bcaPermissions.Recruitment.ApplicationScreenings.Update,

                    bcaPermissions.Recruitment.InterviewSchedules.Default,
                    bcaPermissions.Recruitment.InterviewSchedules.Create,
                    bcaPermissions.Recruitment.InterviewSchedules.Update,

                    bcaPermissions.Recruitment.InterviewEvaluations.Default,
                    bcaPermissions.Recruitment.InterviewEvaluations.Create,
                    bcaPermissions.Recruitment.InterviewEvaluations.Update,

                    bcaPermissions.Recruitment.Offers.Default,
                    bcaPermissions.Recruitment.Offers.Create,
                    bcaPermissions.Recruitment.Offers.Update,

                    bcaPermissions.Recruitment.Employees.Default,
                    bcaPermissions.Recruitment.Employees.Create,
                    bcaPermissions.Recruitment.Employees.Update,

                    bcaPermissions.Recruitment.CandidateResponses.Default,
                    bcaPermissions.Recruitment.Reports.Default
                }
            );
        }

        private async Task GrantCandidatePermissionsAsync()
        {
            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "Candidate",
                new[]
                {
                    bcaPermissions.Recruitment.Candidates.Default,
                    bcaPermissions.Recruitment.Candidates.Update,
                    bcaPermissions.Recruitment.Applications.Default,
                    bcaPermissions.Recruitment.Offers.Default,
                    bcaPermissions.Recruitment.CandidateResponses.Default,
                    bcaPermissions.Recruitment.CandidateResponses.Create
                }
            );
        }
    }
}