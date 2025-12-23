using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblAssetMain", Schema = "dbo")]
    public class AssetMain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? aID { get; set; }
        public string? aModel { get; set; }
        public string? aAssetType { get; set; }
        public string? aNotes { get; set; }
        public int? dtYear { get; set; }
        public int? dtMonth { get; set; }

        [NotMapped]
        public int? Total1 { get; set; }

        [NotMapped]
        public int? Total2 { get; set; }

    }
}
