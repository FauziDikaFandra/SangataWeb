using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblDocket", Schema = "dbo")]
    public class Docket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ddJobNo { get; set; }
        public string? ddID { get; set; }
        public DateTime? ddDate { get; set; }
        public string? ddFreightType { get; set; }
        public string? ddSiteReceived { get; set; }

    }
}
