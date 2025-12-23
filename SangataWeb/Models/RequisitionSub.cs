using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblRequisitionSub", Schema = "dbo")]
    public class RequisitionSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int reqsItem { get; set; }
        public int reqsReqNo { get; set; }
        public string? reqsStockCode { get; set; }
        public string? reqsID { get; set; }
        public decimal? reqsQty { get; set; }
        public string? reqsCurr { get; set; }
        public int reqsSupplier { get; set; }
        public decimal? reqsUnitPrice { get; set; }
        public DateTime? reqsDateDue { get; set; }
        public DateTime? reqsFreightConfirm { get; set; }
        public DateTime? reqsDatereq { get; set; }
        public int reqsQuotation { get; set; }

    }
}
