using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum ApplicationStatus
    {
        Submitted = 1,           // Đã nộp hồ sơ
        Screening = 2,           // Đang sàng lọc
        ScreeningRejected = 3,   // Không đạt vòng lọc hồ sơ
        InterviewScheduled = 4,  // Đã lên lịch phỏng vấn
        Interviewing = 5,        // Đang phỏng vấn
        PassedInterview = 6,     // Đạt phỏng vấn
        FailedInterview = 7,     // Trượt phỏng vấn
        Offered = 8,             // Đã gửi offer
        OfferAccepted = 9,       // Đã nhận offer
        OfferDeclined = 10,      // Từ chối offer
        Hired = 11,              // Đã nhận việc
        Rejected = 12,           // Bị loại
        Cancelled = 13           // Hồ sơ bị hủy
    }
}
