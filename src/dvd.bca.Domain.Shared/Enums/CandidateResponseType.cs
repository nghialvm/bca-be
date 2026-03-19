using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum CandidateResponseType
    {
        InterviewAccepted = 1,           // Ứng viên xác nhận tham gia phỏng vấn
        InterviewDeclined = 2,           // Ứng viên từ chối lịch phỏng vấn
        InterviewRescheduledRequest = 3, // Ứng viên đề nghị đổi lịch phỏng vấn
        OfferAccepted = 4,               // Ứng viên chấp nhận offer
        OfferDeclined = 5,               // Ứng viên từ chối offer
        RecruitmentResultAcknowledged = 6, // Ứng viên đã xác nhận kết quả tuyển dụng
        RecruitmentResultDeclined = 7,   // Ứng viên từ chối kết quả tuyển dụng
        Other = 8                        // Phản hồi khác
    }
}