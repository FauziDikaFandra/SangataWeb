using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("qryWINo", Schema = "dbo")]
    [Keyless]
    public class vWINo
    {
        public int? rJobOrder { get; set; }
        public DateTime? rDateFinish { get; set; }
        public string? rDescription { get; set; }
        public string? rLocation { get; set; }
    }
}
