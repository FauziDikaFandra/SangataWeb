using Microsoft.EntityFrameworkCore;

namespace SangataWeb.Models
{
    [Keyless]
    public class DropDownReport
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
