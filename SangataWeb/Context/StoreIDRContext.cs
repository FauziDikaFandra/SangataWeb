using SangataWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.SqlServer.Server;
namespace SangataWeb
{
    public class StoreIDRContext : DbContext
    {
        public StoreIDRContext(DbContextOptions<StoreIDRContext> options) : base(options)
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
        public DbSet<CCSCode> CCSCode { get; set; }
        public DbSet<CCSCodeNew> CCSCodeNew { get; set; }
        public DbSet<Rack> Rack { get; set; }
        public DbSet<Bin> Bin { get; set; }
        public DbSet<InternalReqOUT> InternalReqOUT { get; set; }
        public DbSet<InternalReqSubOUT> InternalReqSubOUT { get; set; }
        public DbSet<Adjustment> Adjustment { get; set; }
        public DbSet<AdjustmentSub> AdjustmentSub { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<ShippingSub> ShippingSub { get; set; }
        public DbSet<BackCharge> BackCharge { get; set; }
        public DbSet<BackChargeSub> BackChargeSub { get; set; }
        public DbSet<SupplierList> SupplierList { get; set; }
        public DbSet<InternalReq> InternalReq { get; set; }
        public DbSet<InternalReqSub> InternalReqSub { get; set; }
        public DbSet<RequisitionSub> RequisitionSub { get; set; }
        public DbSet<StoreIssueSub> StoreIssueSub { get; set; }
        public DbSet<PO> PO { get; set; }
        public DbSet<POSub> POSub { get; set; }
        public DbSet<Docket> Docket { get; set; }
        public DbSet<DocketSub> DocketSub { get; set; }
        public DbSet<StoreSum> StoreSum { get; set; }
        public DbSet<PreparedBy> PreparedBy { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<RecBy> RecBy { get; set; }
        public DbSet<ListMaterialOutSummary> ListMaterialOutSummary { get; set; }
        public DbSet<ListMaterialOut> ListMaterialOut { get; set; }
        public DbSet<vShippingSub> vShippingSub { get; set; }
        public DbSet<vStoreIssueSub> vStoreIssueSub { get; set; }
        public DbSet<FreightType> FreightType { get; set; }
        public DbSet<vOrReqNo> vOrReqNo { get; set; }
        public DbSet<Requisition> Requisition { get; set; }
        public DbSet<vShippingPONo> vShippingPONo { get; set; }
        public DbSet<vDocketSub> vDocketSub { get; set; }
        public DbSet<StockIn> StockIn { get; set; }
        public DbSet<StockOut> StockOut { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Foreman> Foreman { get; set; }
        public DbSet<StoreMan> StoreMan { get; set; }
        public DbSet<DailyRequestMaterial> DailyRequestMaterial { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<vWINo> vWINo { get; set; }
        public DbSet<ReportMaster> ReportMaster { get; set; }
        public DbSet<DropDownReport> DropDownReport { get; set; }
    }
}
