using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblInternalReqSubOUT", Schema = "dbo")]
    public class InternalReqSubOUT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? irosID { get; set; }
        public string? irosNo { get; set; }
        public int? irosOrReqNo { get; set; }
        public decimal? irosQtyIN { get; set; }
        public decimal? irosQtyBack { get; set; }
        public string? irosStockCode { get; set; }
        public string? irosRemark { get; set; }
        public int? irItem { get; set; }


        [NotMapped]
        public string? uDescription { get; set; }
        [NotMapped]
        public string? sDescription { get; set; }
        [NotMapped]
        public decimal? QtyUsed { get; set; }
    }
}
