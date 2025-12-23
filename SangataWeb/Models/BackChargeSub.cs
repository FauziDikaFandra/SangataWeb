using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblBackChargeSub", Schema = "dbo")]
    public class BackChargeSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? bsID { get; set; }
        public int? bsKPCReqNo { get; set; }
        public int? bsKPCStoreNo { get; set; }
        public string? bsODGStockCode { get; set; }
        public decimal? bsQty { get; set; }
        public decimal? bsUnitCost { get; set; }
        public int? bsQtyUsed { get; set; }
        [NotMapped]
        public string? uDescription { get; set; }
        [NotMapped]
        public string? sDescription { get; set; }
        [NotMapped]
        public decimal? bsQtyBack { get; set; }
    }
}
