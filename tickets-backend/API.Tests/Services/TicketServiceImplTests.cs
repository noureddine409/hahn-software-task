using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Exceptions;
using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.Tests.Services
{
    public class TicketServiceImplTests
    {
        private readonly Mock<GenericRepository<Ticket>> _mockRepository;
        private readonly Mock<ILogger<TicketServiceImpl>> _mockLogger;
        private readonly TicketServiceImpl _ticketService;

        public TicketServiceImplTests()
        {
            _mockRepository = new Mock<GenericRepository<Ticket>>();
            _mockLogger = new Mock<ILogger<TicketServiceImpl>>();
            _ticketService = new TicketServiceImpl(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldReturnTicketDto_WhenTicketIsCreated()
        {
            // Arrange
            var createTicketDto = new CreateTicketDto
            {
                Description = "Sample Ticket",
                Status = TicketStatus.Open
            };

            var ticket = new Ticket
            {
                Id = 1,
                Description = "Sample Ticket",
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            // Act
            var result = await _ticketService.CreateTicketAsync(createTicketDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ticket.Description, result.Description);
        }

        [Fact]
        public async Task GetTicketAsync_ShouldReturnTicketDto_WhenTicketExists()
        {
            // Arrange
            var ticket = new Ticket
            {
                Id = 1,
                Description = "Sample Ticket",
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ticket);

            // Act
            var result = await _ticketService.GetTicketAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ticket.Description, result.Description);
        }

        [Fact]
        public async Task GetTicketAsync_ShouldThrowElementNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Ticket)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => _ticketService.GetTicketAsync(1));
            Assert.Equal("Ticket with ID 1 not found.", exception.Message);
        }

        [Fact]
        public async Task UpdateTicketAsync_ShouldReturnUpdatedTicketDto_WhenTicketExists()
        {
            // Arrange
            var ticket = new Ticket
            {
                Id = 1,
                Description = "Old Description",
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            var updateDto = new CreateTicketDto
            {
                Description = "Updated Description",
                Status = TicketStatus.Closed
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ticket);
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

            // Act
            var result = await _ticketService.UpdateTicketAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Description, result.Description);
            Assert.Equal(updateDto.Status, result.Status);
        }

        [Fact]
        public async Task UpdateTicketAsync_ShouldThrowElementNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            var updateDto = new CreateTicketDto
            {
                Description = "Updated Description",
                Status = TicketStatus.Closed
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Ticket)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => _ticketService.UpdateTicketAsync(1, updateDto));
            Assert.Equal("Ticket with ID 1 not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteTicketAsync_ShouldReturnTrue_WhenTicketExists()
        {
            // Arrange
            var ticket = new Ticket
            {
                Id = 1,
                Description = "Sample Ticket",
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ticket);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _ticketService.DeleteTicketAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTicketAsync_ShouldThrowElementNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Ticket)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => _ticketService.DeleteTicketAsync(1));
            Assert.Equal("Ticket with ID 1 not found.", exception.Message);
        }
    }
}
