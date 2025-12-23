using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblPO", Schema = "dbo")]
    public class PO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int poID { get; set; }
        public DateTime? poDate { get; set; }
        public int poSupplier { get; set; }
        public string? poJobNo { get; set; }
        public string? podecimal { get; set; }
        public decimal? poDuties { get; set; }
        public decimal? poFreight { get; set; }
        public string? poReqNo { get; set; }
        public decimal? poPersen { get; set; }

    }
}
