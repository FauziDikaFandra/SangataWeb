using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblPOSub", Schema = "dbo")]
    public class POSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? posID { get; set; }
        public int posPONo { get; set; }
        public decimal? posQty { get; set; }
        public string? posReqsNo { get; set; }
        public decimal? posDiscount { get; set; }
        public decimal? posUnitCost { get; set; }
        public decimal? posFreightAlloc { get; set; }
        public int posItem { get; set; }

    }
}
