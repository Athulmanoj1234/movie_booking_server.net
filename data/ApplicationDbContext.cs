using Microsoft.EntityFrameworkCore;
using movie_booking.Models;

namespace movie_booking.data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }    
    }
}
