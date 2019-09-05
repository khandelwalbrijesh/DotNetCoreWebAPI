using Microsoft.EntityFrameworkCore;

namespace SampleDotNetCoreApplication.Models
{
    public class FamilyContext : DbContext
    {
        public FamilyContext(DbContextOptions<FamilyContext> options)
            : base(options)
        {
        }


        public DbSet<Family> Families { get; set; }

    }
}
