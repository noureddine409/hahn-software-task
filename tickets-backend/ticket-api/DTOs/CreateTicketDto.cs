using API.Models;
using System.ComponentModel.DataAnnotations;


namespace API.DTOs
{
    public class CreateTicketDto
    {
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(40, ErrorMessage = "Description can't be longer than 40 characters.")]
        public required string Description { get; set; }
        
        [Required(ErrorMessage = "Status is required.")]
        public TicketStatus Status { get; set; } = TicketStatus.Open; // Default status
    }
}
