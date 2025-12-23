using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblShipping", Schema = "dbo")]
    public class Shipping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? siID { get; set; }
        public DateTime? siDate { get; set; }
        public string? siFreightType { get; set; }
        public string? siSiteReceived { get; set; }
        public string? siJobNo { get; set; }
        public decimal? siFreightCost { get; set; }
    }
}
