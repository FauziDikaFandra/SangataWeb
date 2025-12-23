using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblRecBy", Schema = "dbo")]
    public class RecBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? iroRecBy { get; set; }

    }
}
