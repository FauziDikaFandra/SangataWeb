using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblAdjustment", Schema = "dbo")]
    public class Adjustment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? adNo { get; set; }
        public DateTime? adDate { get; set; }
        public string? adStoreMan { get; set; }
        public string? adApproved { get; set; }
    }
}
