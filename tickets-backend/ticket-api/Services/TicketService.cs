using API.DTOs;

namespace API.Services
{
    public interface TicketService
    {
        Task<List<TicketDto>> GetTicketsAsync(int pageNumber, int pageSize, string sortDirection, string keyword);
        Task<TicketDto> GetTicketAsync(int id);
        Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto);
        Task<TicketDto> UpdateTicketAsync(int id, CreateTicketDto ticketDto);
        Task<bool> DeleteTicketAsync(int id);
    }
}
