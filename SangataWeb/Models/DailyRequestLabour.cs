using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblDailyRequestLabour", Schema = "dbo")]
    public class DailyRequestLabour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? drtID { get; set; }
        public string? drtTechnician { get; set; }
        public decimal? drtHours { get; set; }
        public int? drtItem { get; set; }
        public int? drtRefNo { get; set; }
        public decimal? drtPrice { get; set; }
        public string? drtRefNoID { get; set; }
        public string? drtTrTrade { get; set; }
        public int? drTrSplit { get; set; }

        [NotMapped]
        public string? tName { get; set; }

        [NotMapped]
        public string? tTrade { get; set; }



    }
}
