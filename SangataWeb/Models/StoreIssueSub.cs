using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblStoreIssueSub", Schema = "dbo")]
    public class StoreIssueSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? siID { get; set; }
        public string? siNo { get; set; }
        public string? siOrReqNo { get; set; }
        public int siQty { get; set; }
        public decimal? siUnitCost { get; set; }
        public string? siCurrency { get; set; }
        public int siItem { get; set; }
        public decimal? siFreightAlloc { get; set; }

    }
}
