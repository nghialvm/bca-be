using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum InterviewStatus
    {
        Pending = 1,      // Chờ xác nhận
        Confirmed = 2,    // Đã xác nhận
        Rescheduled = 3,  // Đổi lịch
        Cancelled = 4,    // Hủy lịch
        Completed = 5     // Hoàn thành
    }
}
