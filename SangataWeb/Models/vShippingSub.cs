using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("vShippingSub", Schema = "dbo")]
    [Keyless]
    public class vShippingSub
    {
        public int? Id { get; set; }
        public string? sisID { get; set; }
        public string? sisNo { get; set; }
        public decimal? sisQty { get; set; }
        public string? sisPOSubNo { get; set; }
        public string? sDescription { get; set; }
        public string? uDescription { get; set; }
        public decimal? UnitCost { get; set; }
        public string? reqsStockCode { get; set; }
        public string? poCurrency { get; set; }
        public int? sisItem { get; set; }
        public decimal? sisFreightAlloc { get; set; }
        public decimal? siFreightCost { get; set; }
        public decimal? FreightCost { get; set; }
        public decimal? UnitCostFreight { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalFreight { get; set; }

    }
}
