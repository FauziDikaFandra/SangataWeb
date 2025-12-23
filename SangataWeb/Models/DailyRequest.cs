using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblDailyRequest", Schema = "dbo")]
    public class DailyRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int drRefNo { get; set; }
        public DateTime? drDate { get; set; }
        public string? drStoreMan { get; set; }
        public string? drForeman { get; set; }
        public int? drJobNo { get; set; }
        public decimal? drPercent { get; set; }
        public string? drOriginator { get; set; }
        public string? drRefNoID { get; set; }
        public string? drOrderTypeGroup { get; set; }
        public string? drJobNoID { get; set; }
        public string? drRemark { get; set; }
        public int? drCompleted { get; set; }

        [NotMapped]
        public string? sAssetID { get; set; }

        [NotMapped]
        public string? sLocation { get; set; }

        [NotMapped]
        public int? sTypeService { get; set; }

        [NotMapped]
        public int? Total1 { get; set; }

        [NotMapped]
        public int? Total2 { get; set; }

        [NotMapped]
        public DateTime? rDateFinish { get; set; }

        [NotMapped]
        public string? rDescription { get; set; }

        [NotMapped]
        public string? rLocation { get; set; }

    }
}
