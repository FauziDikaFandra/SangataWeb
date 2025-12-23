using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SangataWeb.Models;
using System.Xml.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;
using Azure;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SangataWeb.Class
{

    public class DelData : IDelData
    {
        private readonly LightingPlantContext _lightingPlantContext;
        private readonly StoreIDRContext _storeIDRContext;
        public DelData(LightingPlantContext lightingPlantContext, StoreIDRContext storeIDRContext)
        {
            _lightingPlantContext = lightingPlantContext;
            _storeIDRContext = storeIDRContext;
        }
        public async Task<ActionResult> ApiDeleteService(int Id)
        {
            var service = await _lightingPlantContext.Service.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _lightingPlantContext.Service.Remove(service!);
            await _lightingPlantContext.SaveChangesAsync();
            int id = service!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteBreakdowns(int Id)
        {
            var repair = await _lightingPlantContext.Repair.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _lightingPlantContext.Repair.Remove(repair!);
            await _lightingPlantContext.SaveChangesAsync();
            int id = repair!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteLabour(int Id)
        {
            var labour = await _lightingPlantContext.DailyRequestLabour.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _lightingPlantContext.DailyRequestLabour.Remove(labour!);
            await _lightingPlantContext.SaveChangesAsync();
            int id = labour!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteMaterial(int Id)
        {
            var material = await _storeIDRContext.DailyRequestMaterial.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.DailyRequestMaterial.Remove(material!);
            await _storeIDRContext.SaveChangesAsync();
            int id = material!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteIRequestSubOut(int Id)
        {
            var material = await _storeIDRContext.InternalReqSubOUT.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.InternalReqSubOUT.Remove(material!);
            await _storeIDRContext.SaveChangesAsync();
            int id = material!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteIRequestSubIn(int Id)
        {
            var material = await _storeIDRContext.InternalReqSub.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.InternalReqSub.Remove(material!);
            await _storeIDRContext.SaveChangesAsync();
            int id = material!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteAdjustmentSub(int Id)
        {
            var material = await _storeIDRContext.AdjustmentSub.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.AdjustmentSub.Remove(material!);
            await _storeIDRContext.SaveChangesAsync();
            int id = material!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteShippingSub(int Id)
        {
            var material = await _storeIDRContext.ShippingSub.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.ShippingSub.Remove(material!);
            await _storeIDRContext.SaveChangesAsync();
            int id = material!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteStoreIssueSub(int Id)
        {
            var labour = await _storeIDRContext.StoreIssueSub.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.StoreIssueSub.Remove(labour!);
            await _storeIDRContext.SaveChangesAsync();
            int id = labour!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteDocketSub(int Id)
        {
            var material = await _storeIDRContext.DocketSub.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.DocketSub.Remove(material!);
            await _storeIDRContext.SaveChangesAsync();
            int id = material!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteBackChargeSub(int Id)
        {
            var material = await _storeIDRContext.BackChargeSub.Where(x => x.Id == Id).SingleOrDefaultAsync();
            _storeIDRContext.BackChargeSub.Remove(material!);
            await _storeIDRContext.SaveChangesAsync();
            int id = material!.Id;
            return new JsonResult(new { success = true, result = id });
        }
        public async Task<ActionResult> ApiDeleteMaster(ActionModelData actionModel)
        {
            int id;
            switch (actionModel.Typ)
            {
                case "Foreman":
                    var _foreman = await _storeIDRContext.Foreman.Where(x => x.Id == actionModel.Id).SingleOrDefaultAsync();
                    _storeIDRContext.Foreman.Remove(_foreman!);
                    await _storeIDRContext.SaveChangesAsync();
                    id = _foreman!.Id;
                    break;
                case "Storeman":
                    var _storeman = await _storeIDRContext.StoreMan.Where(x => x.Id == actionModel.Id).SingleOrDefaultAsync();
                    _storeIDRContext.StoreMan.Remove(_storeman!);
                    await _storeIDRContext.SaveChangesAsync();
                    id = _storeman!.Id;
                    break;
                case "Unit":
                    var _unit = await _storeIDRContext.Unit.Where(x => x.Id == actionModel.Id).SingleOrDefaultAsync();
                    _storeIDRContext.Unit.Remove(_unit!);
                    await _storeIDRContext.SaveChangesAsync();
                    id = _unit!.Id;
                    break;
                case "Supplier":
                    var _supplier = await _storeIDRContext.SupplierList.Where(x => x.Id == actionModel.Id).SingleOrDefaultAsync();
                    _storeIDRContext.SupplierList.Remove(_supplier!);
                    await _storeIDRContext.SaveChangesAsync();
                    id = _supplier!.Id;
                    break;
                default:
                    id = 0;
                    break;
            }
            
            return new JsonResult(new { success = true, result = id });
        }
    }   
}
