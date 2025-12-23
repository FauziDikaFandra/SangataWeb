using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("qryStockIN", Schema = "dbo")]
    [Keyless]
    public class StockIn
    {
        public string? Type { get; set; }
        public string? NoRef { get; set; }
        public DateTime? Date { get; set; }
        public string? FreightType { get; set; }
        public decimal? FreightAlloc { get; set; }
        public decimal? Quantity { get; set; }
        public string? PONo { get; set; }
        public string? StockCode { get; set; }
        public decimal? UnitCost { get; set; }
        public string? Supplier { get; set; }
        public string? Currency { get; set; }
        public string? Project { get; set; }
        [NotMapped]
        public decimal? inSum { get; set; }
    }
}
