using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    public class ResultJson
    {
        public bool success { get; set; }
        public int? result { get; set; }
        public string? typ_ { get; set; }

    }
}
