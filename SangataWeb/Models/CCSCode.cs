using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblCCSCode", Schema = "dbo")]
    public class CCSCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ccsID { get; set; }
        public string? ccsStockCode { get; set; }
        public string? ccsDescription { get; set; }
        public string? ccsNotes { get; set; }
    }
}
