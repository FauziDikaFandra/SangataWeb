using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("vStoreIssueSub", Schema = "dbo")]
    [Keyless]
    public class vStoreIssueSub
    {
        public int Id { get; set; }
        public string? siID { get; set; }
        public string? siNo { get; set; }
        public string? siOrReqNo { get; set; }
        public int? siQty { get; set; }
        public string? reqsStockCode { get; set; }
        public string? uDescription { get; set; }
        public string? sDescription { get; set; }
        public decimal? siUnitCost { get; set; }
        public decimal? siFreightCost { get; set; }
        public decimal? siFreightAlloc { get; set; }
        public decimal? FreightCost { get; set; }
        public decimal? UnitCostFreight { get; set; }
        public decimal? TotalCost { get; set; }
        public string? siCurrency { get; set; }
        public int? siItem { get; set; }
        public decimal? TotalFreight { get; set; }


    }
}
