using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblInternalReq", Schema = "dbo")]
    public class InternalReq
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? irID { get; set; }
        public DateTime? irDate { get; set; }
        public int? irPreparedBy { get; set; }
        public string? irJobNo { get; set; }
        public DateTime? irDateRec { get; set; }
        public string? irRecBy { get; set; }
        public string? irCurrency { get; set; }
        public string? irRef { get; set; }
        public string? irRemark { get; set; }

        [NotMapped]
        public string? irJobName { get; set; }
    }
}
