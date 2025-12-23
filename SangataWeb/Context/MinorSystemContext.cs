using SangataWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
namespace SangataWeb
{
    public class MinorSystemContext : DbContext
    {
        public MinorSystemContext(DbContextOptions<MinorSystemContext> options) : base(options)
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
        public DbSet<ClientRef> ClientRef { get; set; }

        public DbSet<OrderType> OrderType { get; set; }
        public DbSet<CompleteBy> CompleteBy { get; set; }
    }
}
