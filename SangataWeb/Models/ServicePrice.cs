using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblServicePrice", Schema = "dbo")]
    public class ServicePrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? scId { get; set; }
        public int? scName { get; set; }
        public int? scClient { get; set; }
        public decimal? scPrice { get; set; }
        public string? scDescription { get; set; }

    }
}
