using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("vDocketSub", Schema = "dbo")]
    [Keyless]
    public class vDocketSub
    {
        public int? Id { get; set; }
        public string? ddsID { get; set; }
        public string? ddsNo { get; set; }
        public decimal? ddsQty { get; set; }
        public string? ddsPOSubNo { get; set; }
        public string? sDescription { get; set; }
        public string? uDescription { get; set; }
        public decimal? UnitCost { get; set; }
        public string? reqsStockCode { get; set; }
        public string? poCurrency { get; set; }
        public int? ddsItem { get; set; }
        public decimal? posFreightAlloc { get; set; }
        public decimal? poDuties { get; set; }
        public decimal? poFreight { get; set; }
        public decimal? posqty { get; set; }
        public decimal? TotalCost { get; set; }

    }
}
