using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblStore", Schema = "dbo")]
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? sStockCode { get; set; }
        public int? sCCSCode { get; set; }
        public string? sSC { get; set; }
        public string? sSCME { get; set; }
        public string? sDescription { get; set; }
        public string? sManufacture { get; set; }
        public string? sModel { get; set; }
        public decimal? sWeight { get; set; }
        public int? sUnit { get; set; }
        public decimal? sMinStock { get; set; }
        public decimal? sMaxtock { get; set; }
        public string? sBinNo { get; set; }
        public string? sRackNo { get; set; }
        public string? sNote { get; set; }
        public int? sClientOwn { get; set; }
        public decimal? sFixPrice { get; set; }
        public string? sStockLama { get; set; }
        public string? sStockCodeNew { get; set; }
        public string? sCCSNew { get; set; }

        [NotMapped]
        public string? uDescription { get; set; }

        [NotMapped]
        public string? sCCSName { get; set; }

    }
}
