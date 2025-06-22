namespace SchoolMedical.Core.DTOs.SchoolCheckup
{
    public class CheckupResponseRequest
    {
        public string ResponseStatus { get; set; } // "Đã đồng ý", "Từ chối", "Chờ phản hồi"
        public string Note { get; set; }
    }
} 