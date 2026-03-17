using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum OfferStatus
    {
        Draft = 1,      // Nháp
        Sent = 2,       // Đã gửi
        Accepted = 3,   // Ứng viên chấp nhận
        Declined = 4,   // Ứng viên từ chối
        Expired = 5     // Hết hạn
    }
}
