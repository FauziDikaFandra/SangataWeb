using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblInternalReqSub", Schema = "dbo")]
    public class InternalReqSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? irsID { get; set; }
        public string? irsNo { get; set; }
        public int irsItem { get; set; }
        public decimal? irsQtyIN { get; set; }
        public decimal? irsQtyBack { get; set; }
        public string? irsStockCode { get; set; }
        public decimal? irsUnitCost { get; set; }

        [NotMapped]
        public string? uDescription { get; set; }
        [NotMapped]
        public string? sDescription { get; set; }
        [NotMapped]
        public decimal? QtyUsed { get; set; }
    }
}
