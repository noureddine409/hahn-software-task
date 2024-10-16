using API.Models;

namespace API.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}