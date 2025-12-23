using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("qryListMaterialOutSummary", Schema = "dbo")]
    [Keyless]
    public class ListMaterialOutSummary
    {
        public string? JobNo { get; set; }
        public string? StockCode { get; set; }
        public string? sDescription { get; set; }
        public decimal? QtyUsed { get; set; }
        public string? Foreman { get; set; }
        public string? RefNo { get; set; }
        public DateTime? Date { get; set; }

    }
}
