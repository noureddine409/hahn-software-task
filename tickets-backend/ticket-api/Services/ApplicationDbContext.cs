using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services

{
    public class ApplicationDbContext: DbContext {

        public ApplicationDbContext(DbContextOptions options): base(options) {

        }

        public DbSet<Ticket> tickets {get; set;}

    }

}