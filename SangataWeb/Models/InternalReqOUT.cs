using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblInternalReqOUT", Schema = "dbo")]
    public class InternalReqOUT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? iroID { get; set; }
        public DateTime? iroDate { get; set; }
        public string? iroPreparedBy { get; set; }
        public string? iroJobNo { get; set; }
        public DateTime? iroDateRec { get; set; }
        public string? iroRecBy { get; set; }
        public string? iroCurrency { get; set; }
        public string? iroRemark { get; set; }

        [NotMapped]
        public string? iroJobName { get; set; }
    }
}
