namespace SchoolMedical.Core.DTOs.MedicalInventory
{
    public class UseItemsDto
    {
        public List<EventItemDto> Items { get; set; }
    }

    public class EventItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
} 