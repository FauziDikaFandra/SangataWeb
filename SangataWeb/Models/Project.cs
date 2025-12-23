using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblProject", Schema = "dbo")]
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? pNo { get; set; }
        public string? pName { get; set; }
        public string? pLocation { get; set; }
        public string? pDivison { get; set; }
        public string? pJobClosed { get; set; }
        public DateTime? pDateClosed { get; set; }
        public string? pJobType { get; set; }
        public string? pContract { get; set; }
        public string? pSpecCase { get; set; }
        public string? pdecimal { get; set; }

    }
}
