using API.DTOs;
using API.Exceptions;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this namespace

namespace API.Controllers
{
    [Route("api/v1/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly ILogger<TicketController> _logger; // Add logger

        public TicketController(TicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger; // Initialize logger
        }

        [HttpGet]
        public async Task<ActionResult<List<TicketDto>>> GetTickets(int page = 1, int size = 10, string sortDirection = "asc", string keyword = "")
        {
            _logger.LogInformation("GetTickets: Page={Page}, Size={Size}, SortDirection={SortDirection}, Keyword={Keyword}", page, size, sortDirection, keyword);
            var tickets = await _ticketService.GetTicketsAsync(page, size, sortDirection, keyword);
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketAsync(id);
                return Ok(ticket);
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto createTicketDto)
        {
            var ticketDto = await _ticketService.CreateTicketAsync(createTicketDto);
            return CreatedAtAction(nameof(GetTicket), new { id = ticketDto.Id }, ticketDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketDto>> UpdateTicket(int id, CreateTicketDto ticketDto)
        {
            try
            {
                var updatedTicketDto = await _ticketService.UpdateTicketAsync(id, ticketDto);
                return Ok(updatedTicketDto);
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            try
            {
                var result = await _ticketService.DeleteTicketAsync(id);
                return NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
