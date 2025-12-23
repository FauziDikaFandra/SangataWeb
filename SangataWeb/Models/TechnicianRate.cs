using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblTechnicianRate", Schema = "dbo")]
    public class TechnicianRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? trID { get; set; }
        public string? trTechnician { get; set; }
        public int? trRateGroup { get; set; }
        public decimal? trPriceRate { get; set; }
        public string? trTrade { get; set; }
        public decimal? trPriceRateIDR { get; set; }

    }
}
