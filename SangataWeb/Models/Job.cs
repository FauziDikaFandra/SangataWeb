using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    public class Job
    {
        public int? rJobOrder { get; set; }
        public DateTime? rDateFinish { get; set; }
        public string? rDescription { get; set; }
        public string? rLocation { get; set; }
    }
}
