using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblService", Schema = "dbo")]
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? sID { get; set; }
        public string? sAssetID { get; set; }
        public DateTime? sDateService { get; set; }
        public int? sFaultYesNo { get; set; }
        public string? sFaultDesc { get; set; }
        public string? sTechnicianID { get; set; }
        public int? sHourMeter { get; set; } = 0;
        public int? sTypeService { get; set; }
        public string? sLocation { get; set; }
        public string? sWO { get; set; }
        public string? sIDCode { get; set; }

        [NotMapped]
        public string? tName { get; set; }

        [NotMapped]
        public int? sTypeServiceName { get; set; }

    }
}
