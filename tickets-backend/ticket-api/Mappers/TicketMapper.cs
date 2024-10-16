// API/Mappers/TicketMapper.cs
using API.DTOs;
using API.Models;

namespace API.Mappers
{
    public static class TicketMapper
    {
        public static TicketDto MapToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                Description = ticket.Description,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }

        public static Ticket MapToEntity(CreateTicketDto createTicketDto)
        {
            return new Ticket
            {
                Description = createTicketDto.Description,
                Status = createTicketDto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
