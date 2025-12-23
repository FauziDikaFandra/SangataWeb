using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblAdjustmentSub", Schema = "dbo")]
    public class AdjustmentSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? adsNo { get; set; }
        public int? adsMainNo { get; set; }
        public string? adsStockCode { get; set; }
        public decimal? adsQty { get; set; }
        public decimal? adsQtyBack { get; set; }
        public string? adsRemark { get; set; }
        public int? adsItem { get; set; }

        [NotMapped]
        public string? uDescription { get; set; }
        [NotMapped]
        public string? sDescription { get; set; }
        [NotMapped]
        public decimal? QtyUsed { get; set; }
    }
}
