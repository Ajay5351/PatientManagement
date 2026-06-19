using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PatientManagement.Models;

namespace PatientManagement.Data
{
    public class PatientDbContext : IdentityDbContext<ApplicationModel>
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {
        }

        public DbSet<PatientModel> Patients { get; set; }

        public class PatientDbContextFactory : IDesignTimeDbContextFactory<PatientDbContext>
        {
            public PatientDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<PatientDbContext>();

                optionsBuilder.UseSqlServer(
                    "Server=.;Database=PatientManagementDb;Integrated Security=True;TrustServerCertificate=True;");

                return new PatientDbContext(optionsBuilder.Options);
            }
        }
    }
}
