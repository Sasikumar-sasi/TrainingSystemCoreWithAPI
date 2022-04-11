using Microsoft.EntityFrameworkCore;

namespace TrainingSystem.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> dbConOptions) : base(dbConOptions)
        {

        }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Trainee> trainees { get; set; }

    }

}
