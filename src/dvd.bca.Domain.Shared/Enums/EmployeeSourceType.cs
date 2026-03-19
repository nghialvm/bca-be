using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Enums
{
    public enum EmployeeSourceType
    {
        Manual = 1,         // Tạo thủ công
        FromCandidate = 2,  // Tạo từ Candidate
        FromApplication = 3,// Tạo từ Application
        FromOffer = 4       // Tạo từ Offer (chuẩn nhất)
    }
}
