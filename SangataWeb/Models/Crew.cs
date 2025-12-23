using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblCrew", Schema = "dbo")]
    public class Crew
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? crID { get; set; }
        public string? crCrewName { get; set; }
        public string? crCrewHead { get; set; }

    }
}
