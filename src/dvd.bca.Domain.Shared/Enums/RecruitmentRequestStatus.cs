using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum RecruitmentRequestStatus
    {
        Draft = 0,               // Nháp
        PendingApproval = 1,     // Chờ duyệt
        Approved = 2,            // Đã duyệt
        Rejected = 3,            // Bị từ chối
        Published = 4,           // Đã đăng tuyển
        Closed = 5,              // Đã đóng tuyển
        Cancelled = 6            // Đã hủy
    }
}
