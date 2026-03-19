using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum CandidateResponseChannel
    {
        System = 1,   // Phản hồi trên hệ thống
        Email = 2,    // Phản hồi qua email
        Phone = 3,    // Phản hồi qua điện thoại
        Interview = 4,// Phản hồi trong buổi trao đổi/phỏng vấn
        Other = 5     // Nguồn khác
    }
}