using Microsoft.AspNetCore.Mvc;
using SangataWeb.Models;

namespace SangataWeb.Class
{
    public interface IDelData
    {
        Task<ActionResult> ApiDeleteService(int Id);
        Task<ActionResult> ApiDeleteBreakdowns(int Id);
        Task<ActionResult> ApiDeleteLabour(int Id);
        Task<ActionResult> ApiDeleteMaterial(int Id);
        Task<ActionResult> ApiDeleteIRequestSubOut(int Id);
        Task<ActionResult> ApiDeleteIRequestSubIn(int Id);
        Task<ActionResult> ApiDeleteAdjustmentSub(int Id);
        Task<ActionResult> ApiDeleteShippingSub(int Id);
        Task<ActionResult> ApiDeleteStoreIssueSub(int Id);
        Task<ActionResult> ApiDeleteDocketSub(int Id);
        Task<ActionResult> ApiDeleteBackChargeSub(int Id);
        Task<ActionResult> ApiDeleteMaster(ActionModelData actionModel);
    }
}
