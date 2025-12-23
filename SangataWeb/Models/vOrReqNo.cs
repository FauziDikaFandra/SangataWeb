using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("vOrReqNo", Schema = "dbo")]
    [Keyless]
    public class vOrReqNo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? reqsID { get; set; }
        public string? reqsStockCode { get; set; }
        public string? sDescription { get; set; }
        public int uID { get; set; }
        public string? uDescription { get; set; }

    }
}
