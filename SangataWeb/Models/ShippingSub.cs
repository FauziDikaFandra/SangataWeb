using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblShippingSub", Schema = "dbo")]
    public class ShippingSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? sisID { get; set; }
        public string? sisNo { get; set; }
        public decimal? sisQty { get; set; }
        public string? sisPOSubNo { get; set; }
        public int sisItem { get; set; }
        public decimal? sisFreightAlloc { get; set; }
        [NotMapped]
        public string? sDescription { get; set; }
        [NotMapped]
        public string? uDescription { get; set; }
        [NotMapped]
        public decimal? UnitCost { get; set; }
        [NotMapped]
        public string? reqsStockCode { get; set; }
        [NotMapped]
        public string? poCurrency { get; set; }
        [NotMapped]
        public decimal? siFreightCost { get; set; }
        [NotMapped]
        public decimal? FreightCost { get; set; }
        [NotMapped]
        public decimal? UnitCostFreight { get; set; }
        [NotMapped]
        public decimal? TotalCost { get; set; }
        [NotMapped]
        public decimal? TotalFreight { get; set; }
    }
}
