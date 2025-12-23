using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblCCSCodeNew", Schema = "dbo")]
    public class CCSCodeNew
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? sCCSNew { get; set; }
        public string? sStockCodeNew { get; set; }
    }
}
