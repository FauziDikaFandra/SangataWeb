using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("vShippingPONo", Schema = "dbo")]
    [Keyless]
    public class vShippingPONo
    {
        public string? posID { get; set; }
        public string? reqsStockCode { get; set; }
        public int? uID { get; set; }
        public string? sDescription { get; set; }
        public string? uDescription { get; set; }
        public string? poCurrency { get; set; }
        public decimal? posUnitCost { get; set; }
        public decimal? poFreight { get; set; }
        public decimal? posFreightAlloc { get; set; }
        public decimal? poDuties { get; set; }
        public decimal? posQty { get; set; }

    }
}
