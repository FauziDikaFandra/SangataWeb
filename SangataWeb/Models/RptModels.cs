using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SangataWeb.Models
{
    public class RptModels
    {
        public class RptMaterialRequisition
        {
            public string drForeman { get; set; }
            public DateTime? drDate { get; set; }
            public int? drRefNo { get; set; }
            public int? drJobNo { get; set; }
            public string Description { get; set; }
            public int? Project { get; set; }
            public string sStockCode { get; set; }
            public string sDescription { get; set; }
            public string sManufacture { get; set; }
            public string sModel { get; set; }
            public string uDescription { get; set; }
            public decimal drsQty { get; set; }
            public decimal drsQtyBack { get; set; }
            public decimal QtyUsed { get; set; }
        }
        public List<RptMaterialRequisition> rptMaterialRequisition { get; set; }
    }
    
}
