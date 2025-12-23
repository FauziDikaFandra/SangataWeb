using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblRequisition", Schema = "dbo")]
    public class Requisition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int reqNo { get; set; }
        public string? reqJobNo { get; set; }
        public DateTime? reqDate { get; set; }
        public string? reqPreparedBy { get; set; }
        public string? reqApprovedBy { get; set; }
        public string? reqRemark { get; set; }

    }
}
