using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    [Table("tblRepair", Schema = "dbo")]
    public class Repair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? rJobOrder { get; set; }
        public int? rODGRefNo { get; set; }
        public string? rODGRefNoID { get; set; }
        public string? rClientRefType { get; set; }
        public string? rPRNo { get; set; }
        public string? rClientRefNo { get; set; }
        public string? rCostCenter { get; set; }
        public string? rAssetID { get; set; }
        public string? rDescription { get; set; }
        public string? rSolution { get; set; }
        public string? rCurrency { get; set; }
        public string? rHeadTech { get; set; }
        public decimal? rEstCost { get; set; }
        public decimal? rEstCostLabor { get; set; }
        public string? rJobOrderType { get; set; }
        public decimal? rQuoteValue { get; set; }
        public int? rFollowService { get; set; }
        public int? rServiceNo { get; set; }
        public int? rInvoiced { get; set; }
        public DateTime? rDateInvoiced { get; set; }
        public string? rInvoiceNo { get; set; }
        public string? rReason { get; set; }
        public string? rProject { get; set; }
        public int? rClientApproved { get; set; }
        public string? rLocation { get; set; }
        public int? rCustomer { get; set; }
        public int? rHourMeter { get; set; }
        public int? rRepairGroup { get; set; }
        public string? rWorkType { get; set; }
        public decimal? rVATMaterial { get; set; }
        public DateTime? rDateOrder { get; set; }
        public DateTime? rDateStart { get; set; }
        public DateTime? rDateFinish { get; set; }
        public DateTime? rDateReport { get; set; }
        public DateTime? rTimeStart { get; set; }
        public DateTime? rTimeOrder { get; set; }
        public DateTime? rTimeFinish { get; set; }
        public DateTime? rTimeReport { get; set; }
        public string? rOriginator { get; set; }
        public string? rReportTo { get; set; }
        public string? rCompleteBy { get; set; }
        public string? rRemark { get; set; }
        public int? rMultipleBilling { get; set; }
        public int? rCancelled { get; set; }
        public DateTime? rDateCancelled { get; set; }
        public string? rCancelledBy { get; set; }
        public string? rCancelledReason { get; set; }
        public int? rJobSlash { get; set; }
        public string? rCompanyName { get; set; }
        public string? rJobAllocID { get; set; }
        public string? rJobType { get; set; }
        public int? rSent { get; set; }
        public DateTime? rDateSent { get; set; }
        public string? rInvoiceNond { get; set; }
        public int? rSplit { get; set; }
        public int? rClose { get; set; }
        public string? rPONumber { get; set; }
        public int? rMatInvoiced { get; set; }
        public int? rLabInvoiced { get; set; }
        public int? rCIC { get; set; }
        public string? rIREQ { get; set; }
        public string? rTenderNo { get; set; }
        public int? rContract { get; set; }
        public int? rVariation { get; set; }
        public decimal? rRate { get; set; }
        public int? rAdjust { get; set; }
        public DateTime? rDateClosed { get; set; }
        public string? rWO { get; set; }
        public int? rTypeService { get; set; }


        [NotMapped]
        public string? tName { get; set; }

        [NotMapped]
        public string? RepairGroupName { get; set; }

        [NotMapped]
        public string? rJobOrderGroup { get; set; }

        [NotMapped]
        public string? drRefNoID { get; set; }

        [NotMapped]
        public DateTime? drDate { get; set; }

        [NotMapped]
        public string? drOriginator { get; set; }

        [NotMapped]
        public decimal? drPercent { get; set; }
    }
}
