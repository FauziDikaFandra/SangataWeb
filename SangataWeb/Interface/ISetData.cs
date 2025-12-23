using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SangataWeb.Models;

namespace SangataWeb.Class
{
    public interface ISetData
    {
        Task<ResultJson> ApiCreateAssetMain([FromBody] AssetMain main, string typ);
        Task<ActionResult> ApiCreateService([FromBody] Service service, string typ);
        Task<ActionResult> ApiCreateBreakdowns([FromBody] Repair repair, string typ);
        Task<ResultJson> ApiCreateRequest([FromBody] DailyRequest daily, string typ);
        Task<ActionResult> ApiCreateLabour([FromBody] DailyRequestLabour daily, string typ);
        Task<ActionResult> ApiCreateLabourMinor([FromBody] DailyRequestLabour daily, string typ);
        Task<ActionResult> ApiCreateMaterial([FromBody] DailyRequestMaterial daily, string typ);
        Task<ActionResult> ApiCreateMaterialMinor([FromBody] DailyRequestMaterial daily, string typ);
        Task<ResultJson> ApiCreateMain([FromBody] Repair main, string typ);
        Task<ResultJson> ApiCreateStore([FromBody] Store main, string typ);
        Task<ActionResult> ApiCreateMaterialStore([FromBody] DailyRequestMaterial daily, string typ);
        Task<ResultJson> ApiCreateIRequestOut([FromBody] InternalReqOUT daily, string typ);
        Task<ActionResult> ApiCreateIRequestSubOut([FromBody] InternalReqSubOUT daily, string typ);
        Task<ResultJson> ApiCreateIRequestIn([FromBody] InternalReq daily, string typ);
        Task<ActionResult> ApiCreateIRequestSubIn([FromBody] InternalReqSub daily, string typ);
        Task<ResultJson> ApiCreateAdjustment([FromBody] Adjustment daily, string typ);
        Task<ActionResult> ApiCreateAdjustmentSub([FromBody] AdjustmentSub daily, string typ);
        Task<ResultJson> ApiCreateShipping([FromBody] Shipping daily, string typ);
        Task<ActionResult> ApiCreateShippingSub([FromBody] ShippingSub daily, string typ);
        Task<ActionResult> ApiStoreIssueSub([FromBody] StoreIssueSub daily, string typ);
        Task<ResultJson> ApiCreateDocket([FromBody] Docket daily, string typ);
        Task<ActionResult> ApiCreateDocketSub([FromBody] DocketSub daily, string typ);
        Task<ResultJson> ApiCreateBackCharge([FromBody] BackCharge daily, string typ);
        Task<ActionResult> ApiCreateBackChargeSub([FromBody] BackChargeSub daily, string typ);
        Task<ActionResult> ApiCreateForeman([FromBody] Foreman daily, string typ);
        Task<ActionResult> ApiCreateStoreman([FromBody] StoreMan daily, string typ);
        Task<ActionResult> ApiCreateUnit([FromBody] Unit daily, string typ);
        Task<ActionResult> ApiCreateSupplier([FromBody] SupplierList daily, string typ);
        Task<ActionResult> ApiCreateCCSCode([FromBody] CCSCode daily, string typ);
        Task<ActionResult> ApiCreateCustomer([FromBody] Customer daily, string typ);
        Task<ActionResult> ApiCreateLocation([FromBody] Location daily, string typ);
    }
}
