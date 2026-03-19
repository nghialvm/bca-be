using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum EmployeeStatus
    {
        PendingActivation = 1, // Mới tạo, chưa chính thức đi làm
        Active = 2,            // Đang làm việc
        Inactive = 3,          // Tạm ngưng (có thể quay lại)
        Resigned = 4           // Đã nghỉ việc
    }
}