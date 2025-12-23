using SangataWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
namespace SangataWeb
{
    public class LightingPlantContext : DbContext
    {
        public LightingPlantContext(DbContextOptions<LightingPlantContext> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<AssetMain> AssetMain { get; set; }
        public DbSet<ServicePrice> ServicePrice { get; set; }
        public DbSet<Technician> Technician { get; set; }
        public DbSet<Crew> Crew { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<RepairGroup> RepairGroup { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceX> ServiceX { get; set; }
        public DbSet<Repair> Repair { get; set; }
        public DbSet<DailyRequest> DailyRequest { get; set; }
        public DbSet<DailyRequestLabour> DailyRequestLabour { get; set; }
        
        public DbSet<TechnicianRate> TechnicianRate { get; set; }
        public DbSet<RateGroup> RateGroup { get; set; }
    }
}
