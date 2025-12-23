using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("qryStoreSum", Schema = "dbo")]
    [Keyless]
    public class StoreSum
    {
        public string? sStockCode { get; set; }
        public string? sDescription { get; set; }
        public string? sManufacture { get; set; }
        public int? ccsID { get; set; }
        public string? sModel { get; set; }
        public string? ccsDescription { get; set; }
        public decimal? sFixPrice { get; set; }
        public DateTime? DateOUT { get; set; }
        public DateTime? DateIN { get; set; }
        public string? NoRefIN { get; set; }
        public decimal? sMinStock { get; set; }
        public decimal? sMaxtock { get; set; }
        public decimal? QtyIN { get; set; }
        public decimal? QtyOUT { get; set; }
        public int? sQtyOwn { get; set; }
        public decimal? QtyActual { get; set; }
        public decimal? QtyOrder { get; set; }
        public decimal? qtyOver { get; set; }
        public decimal? QtyReal { get; set; }
        public decimal? TotalCostIN { get; set; }
        public string? CurrencyIN { get; set; }
        public string? TypeIN { get; set; }
        public string? PONoIN { get; set; }
        public string? SupplierIN { get; set; }
        public int? ConvertedCost { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalReal { get; set; }
        public decimal? TotalOut { get; set; }
        public string? uDescription { get; set; }
        public string? sBinNo { get; set; }
        public string? sRackNo { get; set; }

    }
}
