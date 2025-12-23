using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    public class ActionModelData
    {
        public int Id { get; set; }
        public string? Typ { get; set; }
        public string? Idstr { get; set; }
        public string? Idstr2 { get; set; }
        public string? Idstr3 { get; set; }
    }
}
