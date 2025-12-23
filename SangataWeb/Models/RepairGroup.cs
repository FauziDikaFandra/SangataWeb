using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblRepairGroup", Schema = "dbo")]
    public class RepairGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? gbID { get; set; }
        public string? gbDecription { get; set; }

    }
}
