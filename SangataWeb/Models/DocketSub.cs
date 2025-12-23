using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblDocketSub", Schema = "dbo")]
    public class DocketSub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ddsID { get; set; }
        public string? ddsNo { get; set; }
        public int ddsPONo { get; set; }
        public decimal ddsQty { get; set; }
        public string? ddsPOSubNo { get; set; }
        public int ddsItem { get; set; }


    }
}
