using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblStoreMan", Schema = "dbo")]
    public class StoreMan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? stEmplNo { get; set; }
        public string? stEmplName { get; set; }
    }
}
