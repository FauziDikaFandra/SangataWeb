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
using System;

namespace SangataWeb.Class
{

    public class SetData : ISetData
    {
        private readonly LightingPlantContext _lightingPlantContext;
        private readonly StoreIDRContext _storeIDRContext;
        public SetData(LightingPlantContext lightingPlantContext,StoreIDRContext storeIDRContext)
        {
            _lightingPlantContext = lightingPlantContext;
            _storeIDRContext = storeIDRContext;
        }
        public async Task<ResultJson> ApiCreateAssetMain([FromBody] AssetMain main, string typ)
        {
            if (typ == "Create")
            {
                await _lightingPlantContext.AssetMain.AddAsync(main);
                await _lightingPlantContext.SaveChangesAsync();
            }
            else
            {
                _lightingPlantContext.AssetMain.Update(main);
                try
                {
                    await _lightingPlantContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = main.Id, typ_ = main.aID };
        }
        public async Task<ActionResult> ApiCreateService([FromBody] Service service, string typ)
        {
            bool _success = true;
            string _typ_ = "";
            if (typ == "Create")
            {
                var getService = await _lightingPlantContext.Service.Where(p => p.sID == service.sID).FirstOrDefaultAsync();
                var getBreakdowns = await _lightingPlantContext.Repair.Where(p => p.rJobOrder == service.sID).FirstOrDefaultAsync();
                if (getService != null || getBreakdowns != null)
                {

                    _typ_ = Convert.ToString(service.sID);
                    _success = false;
                }
                else
                {

                    _typ_ = "Create";
                    await _lightingPlantContext.Service.AddAsync(service);
                    await _lightingPlantContext.SaveChangesAsync();

                }

            }
            else
            {

                _typ_ = Convert.ToString(service.sID);
                _lightingPlantContext.Service.Update(service);
                try
                {
                    await _lightingPlantContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    
                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = service.Id;
            return new JsonResult(new { success = _success, result = id, typ_ = _typ_ });
        }
        public async Task<ActionResult> ApiCreateBreakdowns([FromBody] Repair repair, string typ)
        {
            bool _success = true;
            string _typ_ = "";
            if (typ == "Create")
            {
                var getService = await _lightingPlantContext.Service.Where(p => p.sID == repair.rJobOrder).FirstOrDefaultAsync();
                var getBreakdowns = await _lightingPlantContext.Repair.Where(p => p.rJobOrder == repair.rJobOrder).FirstOrDefaultAsync();
                if (getService != null || getBreakdowns != null)
                {
                    _typ_ = Convert.ToString(repair.rJobOrder);
                    _success = false;
                }
                else
                {
                    await _lightingPlantContext.Repair.AddAsync(repair);
                    await _lightingPlantContext.SaveChangesAsync();

                }


            }
            else
            {
                _typ_ = Convert.ToString(repair.rJobOrder);
                _lightingPlantContext.Repair.Update(repair);
                try
                {
                    await _lightingPlantContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = repair.Id;
            return new JsonResult(new { success = _success, result = id, typ_ = _typ_ });
        }
        public async Task<ResultJson> ApiCreateRequest([FromBody] DailyRequest daily, string typ)
        {
            if (typ == "Create")
            {
                await _lightingPlantContext.DailyRequest.AddAsync(daily);
                await _lightingPlantContext.SaveChangesAsync();
            }
            else
            {
                _lightingPlantContext.DailyRequest.Update(daily);
                try
                {
                    await _lightingPlantContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = daily.Id, typ_ = daily.drRefNoID };
        }
        public async Task<ActionResult> ApiCreateLabour([FromBody] DailyRequestLabour daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getLabour = _lightingPlantContext.DailyRequestLabour.Where(p => p.drtTechnician == daily.drtTechnician && p.drtRefNoID == daily.drtRefNoID).FirstOrDefault();
                
                if (getLabour != null)
                {
                    _typ_ = Convert.ToString(daily.drtTechnician);
                    _success = false;
                }
                else
                {
                    var getLabourDesc = _lightingPlantContext.DailyRequestLabour.Where(p => p.drtRefNoID == daily.drtRefNoID).OrderByDescending(x => x.drtItem).FirstOrDefault();
                    int? itm = 1;
                    if (getLabourDesc != null)
                    {
                        itm = getLabourDesc.drtItem + 1;
                    }
                    _typ_ = "Create";
                    daily.drtItem = itm;
                    daily.drtRefNo = Convert.ToInt32(daily.drtRefNoID);
                    daily.drtID = daily.drtRefNoID + "-" + Convert.ToString(itm);
                    await _lightingPlantContext.DailyRequestLabour.AddAsync(daily);
                    await _lightingPlantContext.SaveChangesAsync();

                }

            }
            else
            {
                daily.drtRefNo = Convert.ToInt32(daily.drtRefNoID);
                daily.drtID = daily.drtRefNoID + "-" + Convert.ToString(daily.drtItem);
                _lightingPlantContext.DailyRequestLabour.Update(daily);
                try
                {
                    await _lightingPlantContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.drtItem });
        }
        public async Task<ActionResult> ApiCreateLabourMinor([FromBody] DailyRequestLabour daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getLabour = _lightingPlantContext.DailyRequestLabour.Where(p => p.drtTechnician == daily.drtTechnician && p.drtRefNoID == daily.drtRefNoID).FirstOrDefault();

                if (getLabour != null)
                {
                    _typ_ = Convert.ToString(daily.drtTechnician);
                    _success = false;
                }
                else
                {
                    var getLabourDesc = _lightingPlantContext.DailyRequestLabour.Where(p => p.drtRefNoID == daily.drtRefNoID).OrderByDescending(x => x.drtItem).FirstOrDefault();
                    int? itm = 1;
                    if (getLabourDesc != null)
                    {
                        itm = getLabourDesc.drtItem + 1;
                    }
                    _typ_ = "Create";
                    daily.drtItem = itm;
                    daily.drtID = daily.drtRefNoID + "-" + Convert.ToString(itm);
                    await _lightingPlantContext.DailyRequestLabour.AddAsync(daily);
                    await _lightingPlantContext.SaveChangesAsync();

                }

            }
            else
            {
                daily.drtID = daily.drtRefNoID + "-" + Convert.ToString(daily.drtItem);
                _lightingPlantContext.DailyRequestLabour.Update(daily);
                try
                {
                    await _lightingPlantContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.drtItem });
        }
        public async Task<ActionResult> ApiCreateMaterial([FromBody] DailyRequestMaterial daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.DailyRequestMaterial.Where(p => p.drsStockCode == daily.drsStockCode).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.drsStockCode);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.DailyRequestMaterial.Where(p => p.drsNoID == daily.drsNoID).OrderByDescending(x => x.drsItem).FirstOrDefault();
                    int? itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.drsItem + 1;
                    }
                    _typ_ = "Create";
                    daily.drsItem = itm;
                    daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.drsID = daily.drsNoID + "-" + Convert.ToString(itm);
                    await _storeIDRContext.DailyRequestMaterial.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                daily.drsNo = Convert.ToInt32(daily.drsNoID);
                daily.drsID = daily.drsNoID + "-" + Convert.ToString(daily.drsItem);
                _storeIDRContext.DailyRequestMaterial.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.drsItem });
        }
        public async Task<ActionResult> ApiCreateMaterialStore([FromBody] DailyRequestMaterial daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.DailyRequestMaterial.Where(p => p.drsStockCode == daily.drsStockCode).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.drsStockCode);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.DailyRequestMaterial.Where(p => p.drsNoID == daily.drsNoID).OrderByDescending(x => x.drsItem).FirstOrDefault();
                    int? itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.drsItem + 1;
                    }
                    _typ_ = "Create";
                    daily.drsItem = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.drsID = daily.drsNoID + "-" + Convert.ToString(itm);
                    await _storeIDRContext.DailyRequestMaterial.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                daily.drsID = daily.drsNoID + "-" + Convert.ToString(daily.drsItem);
                _storeIDRContext.DailyRequestMaterial.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.drsItem });
        }
        public async Task<ActionResult> ApiCreateMaterialMinor([FromBody] DailyRequestMaterial daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.DailyRequestMaterial.Where(p => p.drsStockCode == daily.drsStockCode).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.drsStockCode);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.DailyRequestMaterial.Where(p => p.drsNoID == daily.drsNoID).OrderByDescending(x => x.drsItem).FirstOrDefault();
                    int? itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.drsItem + 1;
                    }
                    _typ_ = "Create";
                    daily.drsItem = itm;
                    daily.drsID = daily.drsNoID + "-" + Convert.ToString(itm);
                    await _storeIDRContext.DailyRequestMaterial.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                daily.drsID = daily.drsNoID + "-" + Convert.ToString(daily.drsItem);
                _storeIDRContext.DailyRequestMaterial.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.drsItem });
        }
        public async Task<ResultJson> ApiCreateMain([FromBody] Repair main, string typ)
        {
            if (typ == "Create")
            {
                await _lightingPlantContext.Repair.AddAsync(main);
                await _lightingPlantContext.SaveChangesAsync();
            }
            else
            {
                _lightingPlantContext.Repair.Update(main);
                try
                {
                    await _lightingPlantContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = main.Id, typ_ = Convert.ToString(main.rJobOrder) };
        }
        public async Task<ResultJson> ApiCreateStore([FromBody] Store main, string typ)
        {
            if (typ == "Create")
            {
                await _storeIDRContext.Store.AddAsync(main);
                await _storeIDRContext.SaveChangesAsync();
                var getEnd = _storeIDRContext.CCSCodeNew.Where(p => p.sCCSNew == main.sCCSNew).FirstOrDefault();
                if (getEnd != null)
                {
                    string nextNew = Convert.ToString(Convert.ToInt32(getEnd.sStockCodeNew.Split("-")[1]) + 1);
                    getEnd.sStockCodeNew = getEnd.sCCSNew + "-" + nextNew.PadLeft(3, '0');
                    _storeIDRContext.CCSCodeNew.Update(getEnd);
                }


                var getFirst = _storeIDRContext.CCSCode.Where(p => p.ccsID == main.sCCSCode).FirstOrDefault();
                if (getFirst != null)
                {
                    string nextNew = Convert.ToString(Convert.ToInt32(getFirst.ccsStockCode.Split("-")[1]) + 1);
                    getFirst.ccsStockCode = getFirst.ccsID + "-" + nextNew.PadLeft(3, '0');
                    _storeIDRContext.CCSCode.Update(getFirst);
                }

                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            else
            {
                _storeIDRContext.Store.Update(main);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }

            }
            
            return new ResultJson { success = false, result = main.Id, typ_ = Convert.ToString(main.sCCSCode) };
        }
        public async Task<ResultJson> ApiCreateIRequestOut([FromBody] InternalReqOUT daily, string typ)
        {
            if (typ == "Create")
            {
                await _storeIDRContext.InternalReqOUT.AddAsync(daily);
                await _storeIDRContext.SaveChangesAsync();
            }
            else
            {
                _storeIDRContext.InternalReqOUT.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = daily.Id, typ_ = daily.iroID };
        }
        public async Task<ResultJson> ApiCreateIRequestIn([FromBody] InternalReq daily, string typ)
        {
            if (typ == "Create")
            {

                await _storeIDRContext.InternalReq.AddAsync(daily);
                await _storeIDRContext.SaveChangesAsync();
            }
            else
            {
                _storeIDRContext.InternalReq.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = daily.Id, typ_ = daily.irID };
        }
        public async Task<ActionResult> ApiCreateIRequestSubOut([FromBody] InternalReqSubOUT daily, string typ)
        {
            string _typ_ = "";
            string _no = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.InternalReqSubOUT.Where(p => p.irosID == daily.irosID).FirstOrDefault();

                if (getMaterial != null)
                {
                    _no = "";
                    _typ_ = Convert.ToString(daily.irosID);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.InternalReqSubOUT.Where(p => p.irosNo == daily.irosNo).OrderByDescending(x => x.irItem).FirstOrDefault();
                    int? itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.irItem + 1;
                    }
                    _typ_ = "Create";
                    
                    daily.irItem = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.irosID = daily.irosNo + "-" + Convert.ToString(itm);
                    _no = Convert.ToString(daily.irosID);
                    await _storeIDRContext.InternalReqSubOUT.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                //daily.irosID = daily.irosNo + "-" + Convert.ToString(daily.irItem);
                _storeIDRContext.InternalReqSubOUT.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.irItem, no = _no });
        }
        public async Task<ActionResult> ApiCreateIRequestSubIn([FromBody] InternalReqSub daily, string typ)
        {
            string _typ_ = "";
            string _no = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.InternalReqSub.Where(p => p.irsID == daily.irsID).FirstOrDefault();

                if (getMaterial != null)
                {
                    _no = "";
                    _typ_ = Convert.ToString(daily.irsID);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.InternalReqSub.Where(p => p.irsID == daily.irsID).OrderByDescending(x => x.irsItem).FirstOrDefault();
                    int itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.irsItem + 1;
                    }
                    _typ_ = "Create";

                    daily.irsItem = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.irsID = daily.irsNo + "-" + Convert.ToString(itm);
                    _no = Convert.ToString(daily.irsID);
                    await _storeIDRContext.InternalReqSub.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                //daily.irosID = daily.irosNo + "-" + Convert.ToString(daily.irItem);
                _storeIDRContext.InternalReqSub.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.irsItem, no = _no });
        }
        public async Task<ResultJson> ApiCreateAdjustment([FromBody] Adjustment daily, string typ)
        {
            if (typ == "Create")
            {

                await _storeIDRContext.Adjustment.AddAsync(daily);
                await _storeIDRContext.SaveChangesAsync();
            }
            else
            {
                _storeIDRContext.Adjustment.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = daily.Id, typ_ = daily.adNo };
        }
        public async Task<ActionResult> ApiCreateAdjustmentSub([FromBody] AdjustmentSub daily, string typ)
        {
            string _typ_ = "";
            string _no = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.AdjustmentSub.Where(p => p.adsStockCode == daily.adsStockCode && p.adsMainNo == Convert.ToInt32(daily.adsMainNo)).FirstOrDefault();

                if (getMaterial != null)
                {
                    _no = "";
                    _typ_ = Convert.ToString(daily.adsStockCode);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.AdjustmentSub.Where(p => p.adsMainNo == Convert.ToInt32(daily.adsMainNo)).OrderByDescending(x => x.adsItem).FirstOrDefault();
                    int? itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.adsItem + 1;
                    }
                    _typ_ = "Create";

                    daily.adsItem = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.adsNo = daily.adsMainNo + "-" + Convert.ToString(itm);
                    _no = Convert.ToString(daily.adsNo);
                    await _storeIDRContext.AdjustmentSub.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                //daily.irosID = daily.irosNo + "-" + Convert.ToString(daily.irItem);
                _storeIDRContext.AdjustmentSub.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.adsItem, no = _no });
        }
        public async Task<ResultJson> ApiCreateShipping([FromBody] Shipping daily, string typ)
        {
            if (typ == "Create")
            {

                await _storeIDRContext.Shipping.AddAsync(daily);
                await _storeIDRContext.SaveChangesAsync();
            }
            else
            {
                _storeIDRContext.Shipping.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = daily.Id, typ_ = daily.siID };
        }
        public async Task<ActionResult> ApiCreateShippingSub([FromBody] ShippingSub daily, string typ)
        {
            string _typ_ = "";
            string _no = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.ShippingSub.Where(p => p.sisID == daily.sisID).FirstOrDefault();

                if (getMaterial != null)
                {
                    _no = "";
                    _typ_ = Convert.ToString(daily.sisID);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.ShippingSub.Where(p => p.sisNo == daily.sisNo).OrderByDescending(x => x.sisItem).FirstOrDefault();
                    int itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.sisItem + 1;
                    }
                    _typ_ = "Create";

                    daily.sisItem = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.sisID = daily.sisNo + "-" + Convert.ToString(itm);
                    _no = Convert.ToString(daily.sisID);
                    await _storeIDRContext.ShippingSub.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                //daily.irosID = daily.irosNo + "-" + Convert.ToString(daily.irItem);
                _storeIDRContext.ShippingSub.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.sisItem, no = _no });
        }
        public async Task<ActionResult> ApiStoreIssueSub([FromBody] StoreIssueSub daily, string typ)
        {
            string _typ_ = "";
            string _no = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getLabour = _storeIDRContext.StoreIssueSub.Where(p => p.siID == daily.siID).FirstOrDefault();

                if (getLabour != null)
                {
                    _no = "";
                    _typ_ = Convert.ToString(daily.siID);
                    _success = false;
                }
                else
                {
                    var getLabourDesc = _storeIDRContext.StoreIssueSub.Where(p => p.siNo == daily.siNo).OrderByDescending(x => x.siItem).FirstOrDefault();
                    int itm = 1;
                    if (getLabourDesc != null)
                    {
                        itm = getLabourDesc.siItem + 1;
                    }
                    _typ_ = "Create";

                    daily.siItem = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.siID = daily.siNo + "-" + Convert.ToString(itm);
                    _no = Convert.ToString(daily.siID);
                    await _storeIDRContext.StoreIssueSub.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                //daily.irosID = daily.irosNo + "-" + Convert.ToString(daily.irItem);
                _storeIDRContext.StoreIssueSub.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.siItem, no = _no });
        }
        public async Task<ResultJson> ApiCreateDocket([FromBody] Docket daily, string typ)
        {
            if (typ == "Create")
            {

                await _storeIDRContext.Docket.AddAsync(daily);
                await _storeIDRContext.SaveChangesAsync();
            }
            else
            {
                _storeIDRContext.Docket.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = daily.Id, typ_ = daily.ddID };
        }
        public async Task<ActionResult> ApiCreateDocketSub([FromBody] DocketSub daily, string typ)
        {
            string _typ_ = "";
            string _no = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.DocketSub.Where(p => p.ddsID == daily.ddsID).FirstOrDefault();

                if (getMaterial != null)
                {
                    _no = "";
                    _typ_ = Convert.ToString(daily.ddsID);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.DocketSub.Where(p => p.ddsNo == daily.ddsNo).OrderByDescending(x => x.ddsItem).FirstOrDefault();
                    int itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.ddsItem + 1;
                    }
                    _typ_ = "Create";

                    daily.ddsItem = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                    daily.ddsID = daily.ddsNo + "-" + Convert.ToString(itm);
                    _no = Convert.ToString(daily.ddsID);
                    await _storeIDRContext.DocketSub.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                //daily.irosID = daily.irosNo + "-" + Convert.ToString(daily.irItem);
                _storeIDRContext.DocketSub.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.ddsItem, no = _no });
        }
        public async Task<ResultJson> ApiCreateBackCharge([FromBody] BackCharge daily, string typ)
        {
            if (typ == "Create")
            {

                await _storeIDRContext.BackCharge.AddAsync(daily);
                await _storeIDRContext.SaveChangesAsync();
            }
            else
            {
                _storeIDRContext.BackCharge.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }
            return new ResultJson { success = false, result = daily.Id, typ_ = Convert.ToString(daily.bKPCReqNo) };
        }
        public async Task<ActionResult> ApiCreateBackChargeSub([FromBody] BackChargeSub daily, string typ)
        {
            string _typ_ = "";
            string _no = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.BackChargeSub.Where(p => p.bsKPCReqNo == daily.bsKPCReqNo && p.bsODGStockCode == daily.bsODGStockCode).FirstOrDefault();

                if (getMaterial != null)
                {
                    _no = "";
                    _typ_ = Convert.ToString(daily.bsODGStockCode);
                    _success = false;
                }
                else
                {
                    var getMaterialDesc = _storeIDRContext.BackChargeSub.Where(p => p.bsKPCReqNo == daily.bsKPCReqNo).OrderByDescending(x => x.bsKPCStoreNo).FirstOrDefault();
                    var getMaterialBsID = _storeIDRContext.BackChargeSub.OrderByDescending(x => x.bsID).FirstOrDefault();
                    int? itm = 1;
                    if (getMaterialDesc != null)
                    {
                        itm = getMaterialDesc.bsKPCStoreNo + 1;
                    }
                    _typ_ = "Create";

                    daily.bsKPCStoreNo = itm;
                    //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                   
                    if (getMaterialBsID != null)
                    {
                        daily.bsID = daily.bsID + 1;
                    }
                    else
                    {
                        daily.bsID = 1;
                    }
                    _no = Convert.ToString(daily.bsID);
                    await _storeIDRContext.BackChargeSub.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                //daily.drsNo = Convert.ToInt32(daily.drsNoID);
                //daily.irosID = daily.irosNo + "-" + Convert.ToString(daily.irItem);
                _storeIDRContext.BackChargeSub.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.bsKPCStoreNo, no = _no });
        }
        public async Task<ActionResult> ApiCreateForeman([FromBody] Foreman daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.Foreman.Where(p => p.ftEmplNo == daily.ftEmplNo || p.ftEmplName == daily.ftEmplName).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.ftEmplNo + " - " + daily.ftEmplName);
                    _success = false;
                }
                else
                {
                    await _storeIDRContext.Foreman.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                _storeIDRContext.Foreman.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.ftEmplNo });
        }
        public async Task<ActionResult> ApiCreateStoreman([FromBody] StoreMan daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.StoreMan.Where(p => p.stEmplNo == daily.stEmplNo || p.stEmplName == daily.stEmplName).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.stEmplNo + " - " + daily.stEmplName);
                    _success = false;
                }
                else
                {
                    await _storeIDRContext.StoreMan.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                _storeIDRContext.StoreMan.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.stEmplNo });
        }
        public async Task<ActionResult> ApiCreateUnit([FromBody] Unit daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.Unit.Where(p =>p.uDescription == daily.uDescription).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.uDescription);
                    _success = false;
                }
                else
                {
                    await _storeIDRContext.Unit.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                _storeIDRContext.Unit.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.uID });
        }
        public async Task<ActionResult> ApiCreateSupplier([FromBody] SupplierList daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.SupplierList.Where(p => p.sName == daily.sName).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.sName);
                    _success = false;
                }
                else
                {
                    await _storeIDRContext.SupplierList.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                _storeIDRContext.SupplierList.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.sID });
        }
        public async Task<ActionResult> ApiCreateCCSCode([FromBody] CCSCode daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.CCSCode.Where(p => p.ccsID == daily.ccsID).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.ccsID);
                    _success = false;
                }
                else
                {
                    await _storeIDRContext.CCSCode.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                _storeIDRContext.CCSCode.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.ccsID });
        }
        public async Task<ActionResult> ApiCreateCustomer([FromBody] Customer daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.Customer.Where(p => p.cName == daily.cName).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.cName);
                    _success = false;
                }
                else
                {
                    await _storeIDRContext.Customer.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                _storeIDRContext.Customer.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.cID });
        }
        public async Task<ActionResult> ApiCreateLocation([FromBody] Location daily, string typ)
        {
            string _typ_ = "";
            bool _success = true;
            if (typ == "Create")
            {
                var getMaterial = _storeIDRContext.Location.Where(p => p.lLocationCode == daily.lLocationCode).FirstOrDefault();

                if (getMaterial != null)
                {
                    _typ_ = Convert.ToString(daily.lLocationCode);
                    _success = false;
                }
                else
                {
                    await _storeIDRContext.Location.AddAsync(daily);
                    await _storeIDRContext.SaveChangesAsync();

                }

            }
            else
            {
                _storeIDRContext.Location.Update(daily);
                try
                {
                    await _storeIDRContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {

                }
            }


            //return new ContentResult
            //{
            //    Content = service.Id.ToString(),
            //    ContentType = "text/plain; charset=utf-8"
            //};
            int id = daily.Id;

            return new JsonResult(new { success = _success, result = id, typ_ = _typ_, item = daily.lLocationCode });
        }
    }   
}
