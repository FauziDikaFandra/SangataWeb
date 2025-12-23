using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblTechnician", Schema = "dbo")]
    public class Technician
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? tID { get; set; }
        public string? tName { get; set; }
        public string? tTrade { get; set; }
        public int? tCrew { get; set; }

        [NotMapped]
        public string? crCrewName { get; set; }

        [NotMapped]
        public string? rgDescription { get; set; }

        [NotMapped]
        public string? trTrade { get; set; }

        [NotMapped]
        public decimal? trPriceRate { get; set; }

    }
}
