using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblSupplierList", Schema = "dbo")]
    public class SupplierList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int sID { get; set; }
        public string? sName { get; set; }
        public string? sMaterialCategory { get; set; }
        public string? sStreet { get; set; }
        public string? sCity { get; set; }
        public string? sZIP { get; set; }
        public string? sCountry { get; set; }
        public string? sPhoneNumber1 { get; set; }
        public string? sPhoneNumber2 { get; set; }
        public string? sFaxNumber1 { get; set; }
        public string? sFaxNumber2 { get; set; }
        public string? sMobile { get; set; }
        public string? sEMail { get; set; }
        public string? sWebPage { get; set; }
        public string? sPerson1 { get; set; }
        public string? sPerson2 { get; set; }
        public string? sPersonTitle1 { get; set; }
        public string? sPersonTitle2 { get; set; }

    }
}
