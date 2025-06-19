public class MedicalEventDto
{
    public int EventID { get; set; }
    public int StudentID { get; set; }
    public string StudentName { get; set; }
    public string EventType { get; set; }
    public DateTime EventTime { get; set; }
    public string Description { get; set; }
    public string? Status { get; set; } = "Chưa xử lý"; // Default status is "Chưa xử lý"
}
