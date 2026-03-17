using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum CandidateStatus
    {
        Active = 1,        // Đang hoạt động
        Blacklisted = 2,   // Hạn chế/không tiếp nhận
        Hired = 3,         // Đã tuyển dụng
        Rejected = 4,      // Đã bị loại
        Archived = 5       // Lưu trữ
    }
}
