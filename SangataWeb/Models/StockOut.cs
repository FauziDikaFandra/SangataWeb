using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("qryStockOut", Schema = "dbo")]
    [Keyless]
    public class StockOut
    {
        public string? StoreMan { get; set; }
        public string? Order { get; set; }
        public string? JobNo { get; set; }
        public string? Job { get; set; }
        public DateTime? Date { get; set; }
        public string? RefNo { get; set; }
        public string? StockCode { get; set; }
        public string? Type { get; set; }
        public decimal? QtyUsed { get; set; }
        public string? Project { get; set; }
        public string? pName { get; set; }
        [NotMapped]
        public decimal? outSum { get; set; }
    }
}
