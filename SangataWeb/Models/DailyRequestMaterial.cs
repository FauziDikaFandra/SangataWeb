using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblDailyRequestMaterial", Schema = "dbo")]
    public class DailyRequestMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? drsID { get; set; }
        public string? drsStockCode { get; set; }
        public int? drsNo { get; set; }
        public decimal? drsQty { get; set; }
        public decimal? drsQtyBack { get; set; }
        public string? drsRemark { get; set; }
        public int? drsItem { get; set; }
        public decimal? drsPrice { get; set; }
        public int? drsContractType { get; set; }
        public string? drsNoID { get; set; }
        public int? drsFlag { get; set; }
        public int? drsSplit { get; set; }

        [NotMapped]
        public string? sDescription { get; set; }

        [NotMapped]
        public string? uDescription { get; set; }

        [NotMapped]
        public decimal? QtyUsed { get; set; }
        


    }
}
