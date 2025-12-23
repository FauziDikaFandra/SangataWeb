using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblBackCharge", Schema = "dbo")]
    public class BackCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? bKPCReqNo { get; set; }
        public DateTime? bDate { get; set; }
        public string? bPickUP { get; set; }
        public int? bCostCenter { get; set; }
        public string? bIssueBy { get; set; }
        public string? bJobNo { get; set; }
        public string? bCurrency { get; set; }
        [NotMapped]
        public string? bJobName { get; set; }
    }
}
