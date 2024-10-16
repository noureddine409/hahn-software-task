using API.DTOs;
using API.Exceptions;
using API.Mappers;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class TicketServiceImpl : TicketService
    {
        private readonly GenericRepository<Ticket> _ticketRepository;
        private readonly ILogger<TicketServiceImpl> _logger; // Add logger

        public TicketServiceImpl(GenericRepository<Ticket> ticketRepository, ILogger<TicketServiceImpl> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger; // Initialize logger
        }

        public async Task<List<TicketDto>> GetTicketsAsync(int pageNumber, int pageSize, string sortDirection, string keyword)
        {
            _logger.LogInformation("Fetching tickets: PageNumber={PageNumber}, PageSize={PageSize}, SortDirection={SortDirection}, Keyword={Keyword}", pageNumber, pageSize, sortDirection, keyword);

            var tickets = await _ticketRepository.GetAllAsync(pageNumber, pageSize, query =>
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(t => t.Description.ToLower().Contains(keyword.ToLower()));
                }

                return sortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(t => t.CreatedAt)
                    : query.OrderBy(t => t.CreatedAt);
            });

            return tickets.Select(TicketMapper.MapToDto).ToList();
        }

        public async Task<TicketDto> GetTicketAsync(int id)
        {
            _logger.LogInformation("Getting ticket with ID: {Id}", id);
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket with ID {Id} not found.", id);
                throw new ElementNotFoundException($"Ticket with ID {id} not found.");
            }

            return TicketMapper.MapToDto(ticket);
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto)
        {
            var ticket = TicketMapper.MapToEntity(createTicketDto);
            await _ticketRepository.AddAsync(ticket);
            _logger.LogInformation("Created ticket with ID: {Id}", ticket.Id);
            return TicketMapper.MapToDto(ticket);
        }

        public async Task<TicketDto> UpdateTicketAsync(int id, CreateTicketDto ticketDto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket with ID {Id} not found for update.", id);
                throw new ElementNotFoundException($"Ticket with ID {id} not found.");
            }

            ticket.Description = ticketDto.Description;
            ticket.Status = ticketDto.Status;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            _logger.LogInformation("Updated ticket with ID: {Id}", id);
            return TicketMapper.MapToDto(ticket);
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket with ID {Id} not found for deletion.", id);
                throw new ElementNotFoundException($"Ticket with ID {id} not found.");
            }

            await _ticketRepository.DeleteAsync(id);
            _logger.LogInformation("Deleted ticket with ID: {Id}", id);
            return true;
        }
    }
}
