using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblReportMaster", Schema = "dbo")]
    public class ReportMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? rCategories { get; set; }
        public string? rDescription { get; set; }
        public int rDropdown1 { get; set; }
        public int rDropdown2 { get; set; }
        public string? rDDName { get; set; }
        public string? rDD2Name { get; set; }
        public string? rDDTable { get; set; }
        public string? rDD2Table { get; set; }
        public string? rDDColumns { get; set; }
        public string? rDD2Columns { get; set; }
        public int rDateCriteria { get; set; }
        public string? rQuery { get; set; }
        public int rStatus { get; set; }
    }
}
