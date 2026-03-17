using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public static class RecruitmentRequestStatusExtensions
    {
        public static string ToDisplayName(this RecruitmentRequestStatus status)
        {
            return status switch
            {
                RecruitmentRequestStatus.Draft => "Nháp",
                RecruitmentRequestStatus.PendingApproval => "Chờ duyệt",
                RecruitmentRequestStatus.Approved => "Đã duyệt",
                RecruitmentRequestStatus.Rejected => "Từ chối",
                RecruitmentRequestStatus.Published => "Đã đăng tuyển",
                RecruitmentRequestStatus.Closed => "Đóng tuyển",
                RecruitmentRequestStatus.Cancelled => "Hủy",
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}
