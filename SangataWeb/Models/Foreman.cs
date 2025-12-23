using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblForeman", Schema = "dbo")]
    public class Foreman
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ftEmplNo { get; set; }
        public string? ftEmplName { get; set; }
    }
}
