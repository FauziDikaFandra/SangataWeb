using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using SangataWeb.Models;
using SkiaSharp;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SangataWeb.Class
{

    public class GetData : IGetData
    {
        private readonly LightingPlantContext _lightingPlantContext;
        private readonly MinorSystemContext _minorSystemContext;
        private readonly StoreIDRContext _storeIDRContext;
        public GetData(LightingPlantContext lightingPlantContext, MinorSystemContext minorSystemContext, StoreIDRContext storeIDRContext)
        {
            _lightingPlantContext = lightingPlantContext;
            _minorSystemContext = minorSystemContext;
            _storeIDRContext = storeIDRContext;
        }
        public async Task<ActionResult<Users>>? ApiLogin()
        {
            var users = await _lightingPlantContext.Users.OrderBy(x => x.UserName).FirstOrDefaultAsync();
            return users;
        }
        public async Task<ActionResult<List<Users>>>? ApiUserAll()
        {
            var users = _lightingPlantContext.Users.OrderBy(x => x.UserName).ToList();
            return users;
        }
        public async Task<ActionResult<List<AssetMain>>>? ApiAssetMain(string typ, string aID)
        {
            if (typ == "Type")
            {
                var assetMain = _lightingPlantContext.AssetMain.Where(x => x.aAssetType != "").ToList();
                var result = assetMain.GroupBy(test => test.aAssetType)
                      .Select(grp => grp.First()).ToList();
                return result;
            }
            else 
            {
                var assetMain = _lightingPlantContext.AssetMain.Where(x => x.aModel != "").ToList();
                var result = assetMain.GroupBy(test => test.aModel)
                      .Select(grp => grp.First()).ToList();
                return result;
            }
            
        }
        public async Task<ResultJson>? ApiAssetMainChecking(AssetMain main)
        {
            var getEnd = _lightingPlantContext.AssetMain.Where(p => p.aID == main.aID).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = main.aID };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = main.aID };
            }
        }
        public async Task<ActionResult<AssetMain>>? ApiAssetMainBy(ActionModelData actionModel)
        {
            AssetMain assetMain = new AssetMain();
            switch (actionModel.Typ)
            {
                case "keep":
                    assetMain = _lightingPlantContext.AssetMain.Where(x => x.Id == actionModel.Id).FirstOrDefault();
                    break;
                case "keep2":
                    assetMain = _lightingPlantContext.AssetMain.Where(x => x.aID == actionModel.Idstr).FirstOrDefault();
                    break;
                case "next":
                    var getEnd = _lightingPlantContext.AssetMain.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    //assetMain = _lightingPlantContext.AssetMain.Where(x => x.Id == (actionModel.Id + 1)).FirstOrDefault();
                    assetMain = _lightingPlantContext.AssetMain.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefault();
                    if (assetMain == null)
                    {
                        assetMain = _lightingPlantContext.AssetMain.Where(x => x.Id == actionModel.Id).FirstOrDefault();
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    //assetMain = _lightingPlantContext.AssetMain.Where(x => x.Id == (actionModel.Id - 1)).FirstOrDefault();
                    assetMain = _lightingPlantContext.AssetMain.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (assetMain == null)
                    {
                        assetMain = _lightingPlantContext.AssetMain.Where(x => x.Id == actionModel.Id).FirstOrDefault();
                    }
                    
                    break;
                case "end":
                    assetMain = _lightingPlantContext.AssetMain.OrderByDescending(p => p.Id).First();
                    break;
                default:
                    assetMain = _lightingPlantContext.AssetMain.GroupBy(x => x.Id).Select(g => g.OrderBy(x => x.Id).First()).FirstOrDefault();
                    break;
            }
            return assetMain;
        }
        public async Task<ActionResult<List<ServicePrice>>>? ApiServicePrice()
        {
            var service = _lightingPlantContext.ServicePrice.OrderBy(x => x.scId).ToList();
            return service;
        }
        public async Task<ActionResult<List<Technician>>>? ApiTechnician()
        {
            var technicianDD = from t1 in _lightingPlantContext.Technician.OrderBy(x => x.tID)
                               join t2 in _lightingPlantContext.Crew on t1.tCrew equals t2.crID
                               select new Technician() { tID = t1.tID, tName = t1.tName, crCrewName = t2.crCrewName, tTrade = t1.tTrade };
            return technicianDD.ToList();
        }
        public async Task<ActionResult<List<Technician>>>? ApiTechnicianRate()
        {
            var technicianDD = from t1 in _lightingPlantContext.Technician.OrderBy(x => x.tID)
                               join t2 in _lightingPlantContext.Crew on t1.tCrew equals t2.crID
                               join t3 in _lightingPlantContext.TechnicianRate on t1.tID equals t3.trTechnician
                               join t4 in _lightingPlantContext.RateGroup on t3.trRateGroup equals t4.rgID
                               select new Technician() { tID = t1.tID, tName = t1.tName, rgDescription = t4.rgDescription, tTrade = t1.tTrade, trTrade = t3.trTrade, trPriceRate = t3.trPriceRate };
            return technicianDD.ToList();
        }
        public async Task<ActionResult<List<Project>>>? ApiProject()
        {
            //var project = _lightingPlantContext.Project.Where(y => y.pJobClosed == "0").OrderBy(x => x.pNo).ToList();
            var project = _lightingPlantContext.Project.OrderBy(x => x.pNo).ToList();
            return project;
        }
        public async Task<ActionResult<List<RepairGroup>>>? ApiRepairGroup()
        {
            var repairGroup = _lightingPlantContext.RepairGroup.OrderBy(x => x.gbID).ToList();
            return repairGroup;
        }
        public async Task<ActionResult<List<Service>>>? ApiService(string sAssetID)
        {
            var service = await (from t1 in _lightingPlantContext.Service
                               join t2 in _lightingPlantContext.Technician on new { X1 = t1.sTechnicianID, X2 = t1.sAssetID } equals new { X1 = t2.tID, X2 = sAssetID } 
                               join t3 in _lightingPlantContext.ServicePrice on t1.sTypeService equals t3.scId
                               select new Service() { Id = t1.Id, sID = t1.sID, sAssetID = t1.sAssetID, sDateService = t1.sDateService,
                                   sFaultYesNo = t1.sFaultYesNo,
                                   sFaultDesc = t1.sFaultDesc,
                                   sTechnicianID = t1.sTechnicianID,
                                   sHourMeter = t1.sHourMeter,
                                   sTypeService = t1.sTypeService,
                                   sLocation = t1.sLocation,
                                   sWO = t1.sWO,
                                   sIDCode = t1.sIDCode,
                                   tName = t2.tName,
                                   sTypeServiceName = t3.scName
                               }).ToListAsync();

            //var service = _lightingPlantContext.Service.Where(y => y.sAssetID == sAssetID).OrderBy(x => x.sID).ToList();
            return service;
        }
        public async Task<ActionResult<List<Repair>>>? ApiRepair(string sAssetID)
        {
            var repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.rAssetID == sAssetID).OrderBy(y => y.rJobOrder)
                         join t2 in _lightingPlantContext.Technician on t1.rHeadTech equals t2.tID  into t3
                          from t4 in t3.DefaultIfEmpty()
                          join t5 in _lightingPlantContext.RepairGroup on t1.rRepairGroup equals t5.gbID
                         select new Repair()
                          {
                              Id = t1.Id,
                              rJobOrder = t1.rJobOrder,
                              rODGRefNo = t1.rODGRefNo,
                              rODGRefNoID = t1.rODGRefNoID,
                              rClientRefType = t1.rClientRefType,
                              rPRNo = t1.rPRNo,
                              rClientRefNo = t1.rClientRefNo,
                              rCostCenter = t1.rCostCenter,
                              rAssetID = t1.rAssetID,
                              rDescription = t1.rDescription,
                              rSolution = t1.rSolution,
                              rCurrency = t1.rCurrency,
                              rHeadTech = Convert.ToString(t1.rHeadTech).PadLeft(4, '0'),
                              rEstCost = t1.rEstCost,
                              rEstCostLabor = t1.rEstCostLabor,
                              rJobOrderType = t1.rJobOrderType,
                              rQuoteValue = t1.rQuoteValue,
                              rFollowService = t1.rFollowService,
                              rServiceNo = t1.rServiceNo,
                              rInvoiced = t1.rInvoiced,
                              rDateInvoiced = t1.rDateInvoiced,
                              rInvoiceNo = t1.rInvoiceNo,
                              rReason = t1.rReason,
                              rProject = t1.rProject,
                              rClientApproved = t1.rClientApproved,
                              rLocation = t1.rLocation,
                              rCustomer = t1.rCustomer,
                              rHourMeter = t1.rHourMeter,
                              rRepairGroup = t1.rRepairGroup,
                              rWorkType = t1.rWorkType,
                              rVATMaterial = t1.rVATMaterial,
                              rDateOrder = t1.rDateOrder,
                              rDateStart = t1.rDateStart,
                              rDateFinish = t1.rDateFinish,
                              rDateReport = t1.rDateReport,
                              rTimeStart = t1.rTimeStart,
                              rTimeOrder = t1.rTimeOrder,
                              rTimeFinish = t1.rTimeFinish,
                              rTimeReport = t1.rTimeReport,
                              rOriginator = t1.rOriginator,
                              rReportTo = t1.rReportTo,
                              rCompleteBy = t1.rCompleteBy,
                              rRemark = t1.rRemark,
                              rMultipleBilling = t1.rMultipleBilling,
                              rCancelled = t1.rCancelled,
                              rDateCancelled = t1.rDateCancelled,
                              rCancelledBy = t1.rCancelledBy,
                              rCancelledReason = t1.rCancelledReason,
                              rJobSlash = t1.rJobSlash,
                              rCompanyName = t1.rCompanyName,
                              rJobAllocID = t1.rJobAllocID,
                              rJobType = t1.rJobType,
                              rSent = t1.rSent,
                              rDateSent = t1.rDateSent,
                              rInvoiceNond = t1.rInvoiceNond,
                              rSplit = t1.rSplit,
                              rClose = t1.rClose,
                              rPONumber = t1.rPONumber,
                              rMatInvoiced = t1.rMatInvoiced,
                              rLabInvoiced = t1.rLabInvoiced,
                              rCIC = t1.rCIC,
                              rIREQ = t1.rIREQ,
                              rTenderNo = t1.rTenderNo,
                              rContract = t1.rContract,
                              rVariation = t1.rVariation,
                              rRate = t1.rRate,
                              rAdjust = t1.rAdjust,
                              rDateClosed = t1.rDateClosed,
                              rWO = t1.rWO,
                              rTypeService = t1.rTypeService,
                              tName = t4.tName,
                              RepairGroupName = t5.gbDecription
                          }).ToListAsync();
            //var repair = _lightingPlantContext.Repair.Where(y => y.rAssetID == sAssetID).OrderBy(x => x.rJobOrder).ToList();
            return repair;
        }
        public async Task<ActionResult<List<AssetMain>>>? ApiAssetMainFind()
        {

            //var main = _lightingPlantContext.AssetMain
            //.GroupJoin(_lightingPlantContext.Service,
            //asset => asset.aID,
            //service => service.sAssetID,
            //(asset, services) => new
            //{
            //    Asset = asset,
            //    Services = services
            //})
            //.GroupJoin(_lightingPlantContext.Repair,
            //assetWithServices => assetWithServices.Asset.aID,
            //repair => repair.rAssetID,
            //(assetWithServices, repairs) => new 
            //{
            //    Asset = assetWithServices.Asset,
            //    Total1 = assetWithServices.Services.Count(),
            //    Total2 = repairs.Count()
            //})
            //.Select(x => new 
            //{
            //    x.Asset.Id,
            //    x.Asset.aID,
            //    x.Asset.aAssetType,
            //    x.Asset.aModel,
            //    x.Asset.aNotes,
            //    Total1 = x.Total1,
            //    Total2 = x.Total2
            //})
            //.OrderBy(x => x.aID)
            //.ToList();
            var main = _lightingPlantContext.AssetMain.ToList();
            //List<AssetMain> main = new List<AssetMain>();
            //foreach (var t0 in header)
            //{
            //    AssetMain dtl = new AssetMain
            //    {
            //        Id = t0.Id,
            //        aID = t0.aID,
            //        aAssetType = t0.aAssetType,
            //        aModel = t0.aModel,
            //        aNotes = t0.aNotes,
            //        Total1 = (from tX in this._lightingPlantContext.Service.Where(c => c.sAssetID == t0.aID) select tX).Count(),
            //        Total2 = (from tX in this._lightingPlantContext.Repair.Where(c => c.rAssetID == t0.aID) select tX).Count(),
            //    };
            //    header.Add(dtl);
            //    // DO MY PROCESSING
            //}
            //var main = await (from t0 in _lightingPlantContext.AssetMain
            //            select new AssetMain()
            //            {
            //                Id = t0.Id,
            //                aID = t0.aID,
            //                aAssetType = t0.aAssetType,
            //                aModel = t0.aModel,
            //                aNotes = t0.aNotes,
            //                Total1 = (from tX in this._lightingPlantContext.Service.Where(c => c.sAssetID == t0.aID) select tX).Count(),
            //                Total2 = (from tX in this._lightingPlantContext.Repair.Where(c => c.rAssetID == t0.aID) select tX).Count(),
            //            }).ToListAsync();
            //select new AssetMain()
            //            {
            //                Id = t0.Id,
            //                aID = t0.aID,
            //                aAssetType = t0.aAssetType,
            //                aModel = t0.aModel,
            //                aNotes = t0.aNotes,
            //                Total1 = (from tX in this._lightingPlantContext.Service select t2).Count(),
            //                Total2 = (from tX in this._lightingPlantContext.Repair select t3).Count(),
            //            };
            //var main2 = from t0 in main
            //            join t1 in _lightingPlantContext.Repair on t0.aID equals t1.rAssetID into t2
            //           select new AssetMain()
            //           {
            //               Id = t0.Id,
            //               aID = t0.aID,
            //               aAssetType = t0.aAssetType,
            //               aModel = t0.aModel,
            //               aNotes = t0.aNotes,
            //               Total1 = t0.Total1,
            //               Total2 = t2.Count(),
            //           };
            return main;
        }
        public async Task<ActionResult<DailyRequest>>? ApiDailyRequest()
        {
            //var daily = _lightingPlantContext.DailyRequest.OrderByDescending(x => x.Id).First();

            var daily = await (from t1 in _lightingPlantContext.DailyRequest.OrderByDescending(x => x.Id)
                               join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                               from t4 in t3.DefaultIfEmpty()
                               join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                               from t7 in t6.DefaultIfEmpty()
                               select new DailyRequest()
                                 {
                                     Id = t1.Id,
                                     drRefNo = t1.drRefNo,
                                     drDate = t1.drDate,
                                     drStoreMan = t1.drStoreMan,
                                     drForeman = t1.drForeman,
                                     drJobNo = t1.drJobNo,
                                     drPercent = t1.drPercent,
                                     drOriginator = t1.drOriginator,
                                     drRefNoID = t1.drRefNoID,
                                     drOrderTypeGroup = t1.drOrderTypeGroup,
                                     drJobNoID = t1.drJobNoID,
                                     drRemark = t1.drRemark,
                                     drCompleted = t1.drCompleted,
                                     sAssetID = t4.sAssetID,
                                     sLocation = t4.sLocation,
                                     sTypeService = t7.scName
                                 }).FirstOrDefaultAsync();
            return daily;
        }
        public async Task<ActionResult<List<DailyRequestLabour>>>? ApiDailyRequestLabour(string drRefNo)
        {
            
            var daily = await (from t1 in _lightingPlantContext.DailyRequestLabour.Where(x => x.drtRefNoID == drRefNo).OrderByDescending(y => y.drtItem)
                               join t2 in _lightingPlantContext.Technician on t1.drtTechnician equals t2.tID into t3
                               from t4 in t3.DefaultIfEmpty()
                               select new DailyRequestLabour()
                               {
                                   Id = t1.Id,
                                   drtID = t1.drtID,
                                   drtTechnician = t1.drtTechnician,
                                   drtHours = t1.drtHours,
                                   drtItem = t1.drtItem,
                                   drtRefNo = t1.drtRefNo,
                                   drtPrice = t1.drtPrice,
                                   drtRefNoID = t1.drtRefNoID,
                                   drtTrTrade = t1.drtTrTrade,
                                   drTrSplit = t1.drTrSplit,
                                   tName = t4.tName,
                                   tTrade = t4.tTrade,
                               }).ToListAsync();
            return daily;
        }
        public async Task<ActionResult<List<DailyRequestMaterial>>>? ApiDailyRequestMaterial(string drRefNo)
        {

            var daily1 = await (from t1 in _storeIDRContext.DailyRequestMaterial.Where(x => x.drsNoID == drRefNo).OrderByDescending(y => y.drsItem)
                               join t2 in _storeIDRContext.Store on t1.drsStockCode equals t2.sStockCode
                               select new DailyRequestMaterial()
                               {
                                   Id = t1.Id,
                                   drsID = t1.drsID,
                                   drsStockCode = t1.drsStockCode,
                                   drsNo = t1.drsNo == null ? 0 : t1.drsNo,
                                   drsQty = t1.drsQty,
                                   drsQtyBack = t1.drsQtyBack == null ? 0 : t1.drsQtyBack,
                                   drsRemark = t1.drsRemark,
                                   drsItem = t1.drsItem,
                                   drsPrice = t1.drsPrice,
                                   drsContractType = t1.drsContractType,
                                   drsNoID = t1.drsNoID,
                                   drsFlag = t1.drsFlag,
                                   drsSplit = t1.drsSplit,
                                   uDescription = Convert.ToString(t2.sUnit),
                                   sDescription = t2.sDescription,
                                   QtyUsed = Convert.ToDecimal(Convert.ToDecimal(t1.drsQty) - Convert.ToDecimal(((t1.drsQtyBack == null) ? Convert.ToDecimal(0.00) : Convert.ToDecimal(t1.drsQtyBack)))),
                               }).ToListAsync();
            var daily = (from t1 in daily1.OrderBy(y => y.drsItem)
                         join t2 in _storeIDRContext.Unit.OrderByDescending(x => x.Id) on t1.uDescription equals Convert.ToString(t2.uID) 
                         select new DailyRequestMaterial()
                         {
                             Id = t1.Id,
                             drsID = t1.drsID,
                             drsStockCode = t1.drsStockCode,
                             drsNo = t1.drsNo,
                             drsQty = t1.drsQty,
                             drsQtyBack = t1.drsQtyBack,
                             drsRemark = t1.drsRemark,
                             drsItem = t1.drsItem,
                             drsPrice = t1.drsPrice,
                             drsContractType = t1.drsContractType,
                             drsNoID = t1.drsNoID,
                             drsFlag = t1.drsFlag,
                             drsSplit = t1.drsSplit,
                             sDescription = t1.sDescription,
                             uDescription = t2.uDescription,
                             QtyUsed = t1.QtyUsed,
                         }).ToList();
            return daily;
        }
        public async Task<ResultJson>? ApiAssetRequestChecking(DailyRequest daily)
        {
            var getEnd = _lightingPlantContext.DailyRequest.Where(p => p.drRefNoID == daily.drRefNoID).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = daily.drRefNoID };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = daily.drRefNoID };
            }
        }
        public async Task<ActionResult<List<Store>>>? ApiStore()
        {
            var store = (from t1 in _storeIDRContext.Store
                         join t2 in _storeIDRContext.Unit.OrderBy(y => y.Id) on t1.sUnit equals t2.uID
                               select new Store()
                               {
                                   Id = t1.Id,
                                   sStockCode = t1.sStockCode,
                                   sDescription = t1.sDescription,
                                   uDescription = t2.uDescription,
                               }).ToList();
            return store;
        }
        public async Task<ActionResult<Store>>? ApiOneStore()
        {
            var store = await _storeIDRContext.Store.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (store != null)
            {
                var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                if (cCSCode != null)
                {
                    store.sCCSName = cCSCode.ccsDescription;
                }
            }
            return store;
        }
        public async Task<ActionResult<DailyRequest>>? ApiRequestBy(ActionModelData actionModel)
        {
            DailyRequest? daily = new DailyRequest();
            switch (actionModel.Typ)
            {
                case "keep":
                    //daily = _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id).FirstOrDefault();
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id)
                                   join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                   from t4 in t3.DefaultIfEmpty()
                                   join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                   from t7 in t6.DefaultIfEmpty()
                                   select new DailyRequest()
                                   {
                                       Id = t1.Id,
                                       drRefNo = t1.drRefNo,
                                       drDate = t1.drDate,
                                       drStoreMan = t1.drStoreMan,
                                       drForeman = t1.drForeman,
                                       drJobNo = t1.drJobNo,
                                       drPercent = t1.drPercent,
                                       drOriginator = t1.drOriginator,
                                       drRefNoID = t1.drRefNoID,
                                       drOrderTypeGroup = t1.drOrderTypeGroup,
                                       drJobNoID = t1.drJobNoID,
                                       drRemark = t1.drRemark,
                                       drCompleted = t1.drCompleted,
                                       sAssetID = t4.sAssetID,
                                       sLocation = t4.sLocation,
                                       sTypeService = t7.scName
                                   }).FirstOrDefaultAsync();
                    break;
                case "keep2":
                    //daily = _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id).FirstOrDefault();
                    daily = await (from t1 in _lightingPlantContext.Service.Where(x => x.sID == actionModel.Id)
                                   join t2 in _lightingPlantContext.DailyRequest on t1.sID equals t2.drJobNo into t3
                                   from t4 in t3.DefaultIfEmpty()
                                   join t5 in _lightingPlantContext.ServicePrice on t1.sTypeService equals t5.scId into t6
                                   from t7 in t6.DefaultIfEmpty()
                                   select new DailyRequest()
                                   {
                                       Id = t4.Id == null ? 0 : t4.Id,
                                       drRefNo = t4.drRefNo == null ? 0 : t4.drRefNo,
                                       drDate = t4.drDate == null ? DateTime.Now : t4.drDate,
                                       drStoreMan = t4.drStoreMan == null ? "" : t4.drStoreMan,
                                       drForeman = t4.drForeman == null ? "''" : t4.drForeman,
                                       drJobNo = t4.drJobNo == null ? 0 : t4.drJobNo,
                                       drPercent = t4.drPercent == null ? 0 : t4.drPercent,
                                       drOriginator = t4.drOriginator == null ? "" : t4.drOriginator,
                                       drRefNoID = t4.drRefNoID == null ? "" : t4.drRefNoID,
                                       drOrderTypeGroup = t4.drOrderTypeGroup == null ? "" : t4.drOrderTypeGroup,
                                       drJobNoID = t4.drJobNoID == null ? "" : t4.drJobNoID,
                                       drRemark = t4.drRemark == null ? "" : t4.drRemark,
                                       drCompleted = t4.drCompleted == null ? 0 : t4.drCompleted,
                                       sAssetID = t1.sAssetID,
                                       sLocation = t1.sLocation,
                                       sTypeService = t7.scName
                                   }).FirstOrDefaultAsync();
                    break;
                case "next":
                    var getEnd = _lightingPlantContext.DailyRequest.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    //daily = _lightingPlantContext.DailyRequest.Where(x => x.Id == (actionModel.Id + 1)).FirstOrDefault();
                    //daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id + 1)
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id)
                                   join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                   from t4 in t3.DefaultIfEmpty()
                                   join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                   from t7 in t6.DefaultIfEmpty()
                                   select new DailyRequest()
                                   {
                                       Id = t1.Id,
                                       drRefNo = t1.drRefNo,
                                       drDate = t1.drDate,
                                       drStoreMan = t1.drStoreMan,
                                       drForeman = t1.drForeman,
                                       drJobNo = t1.drJobNo,
                                       drPercent = t1.drPercent,
                                       drOriginator = t1.drOriginator,
                                       drRefNoID = t1.drRefNoID,
                                       drOrderTypeGroup = t1.drOrderTypeGroup,
                                       drJobNoID = t1.drJobNoID,
                                       drRemark = t1.drRemark,
                                       drCompleted = t1.drCompleted,
                                       sAssetID = t4.sAssetID,
                                       sLocation = t4.sLocation,
                                       sTypeService = t7.scName
                                   }).FirstOrDefaultAsync();
                    if (daily == null)
                    {
                        daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id)
                                       join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                       from t7 in t6.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           sAssetID = t4.sAssetID,
                                           sLocation = t4.sLocation,
                                           sTypeService = t7.scName
                                       }).FirstOrDefaultAsync();
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    //daily = _lightingPlantContext.DailyRequest.Where(x => x.Id == (actionModel.Id - 1)).FirstOrDefault();
                    //daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id - 1)
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id)
                                    join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                   from t4 in t3.DefaultIfEmpty()
                                   join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                   from t7 in t6.DefaultIfEmpty()
                                   select new DailyRequest()
                                   {
                                       Id = t1.Id,
                                       drRefNo = t1.drRefNo,
                                       drDate = t1.drDate,
                                       drStoreMan = t1.drStoreMan,
                                       drForeman = t1.drForeman,
                                       drJobNo = t1.drJobNo,
                                       drPercent = t1.drPercent,
                                       drOriginator = t1.drOriginator,
                                       drRefNoID = t1.drRefNoID,
                                       drOrderTypeGroup = t1.drOrderTypeGroup,
                                       drJobNoID = t1.drJobNoID,
                                       drRemark = t1.drRemark,
                                       drCompleted = t1.drCompleted,
                                       sAssetID = t4.sAssetID,
                                       sLocation = t4.sLocation,
                                       sTypeService = t7.scName
                                   }).FirstOrDefaultAsync();
                    if (daily == null)
                    {
                        daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id)
                                       join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                       from t7 in t6.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           sAssetID = t4.sAssetID,
                                           sLocation = t4.sLocation,
                                           sTypeService = t7.scName
                                       }).FirstOrDefaultAsync();
                    }
                    break;
                case "end":
                    //daily = _lightingPlantContext.DailyRequest.OrderByDescending(p => p.Id).First();
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.OrderByDescending(x => x.Id)
                                   join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                   from t4 in t3.DefaultIfEmpty()
                                   join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                   from t7 in t6.DefaultIfEmpty()
                                   select new DailyRequest()
                                   {
                                       Id = t1.Id,
                                       drRefNo = t1.drRefNo,
                                       drDate = t1.drDate,
                                       drStoreMan = t1.drStoreMan,
                                       drForeman = t1.drForeman,
                                       drJobNo = t1.drJobNo,
                                       drPercent = t1.drPercent,
                                       drOriginator = t1.drOriginator,
                                       drRefNoID = t1.drRefNoID,
                                       drOrderTypeGroup = t1.drOrderTypeGroup,
                                       drJobNoID = t1.drJobNoID,
                                       drRemark = t1.drRemark,
                                       drCompleted = t1.drCompleted,
                                       sAssetID = t4.sAssetID,
                                       sLocation = t4.sLocation,
                                       sTypeService = t7.scName
                                   }).FirstOrDefaultAsync();
                    break;
                default:
                    //daily = _lightingPlantContext.DailyRequest.GroupBy(x => x.Id).Select(g => g.OrderBy(x => x.Id).First()).FirstOrDefault();
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.OrderBy(x => x.Id)
                                   join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                   from t4 in t3.DefaultIfEmpty()
                                   join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                   from t7 in t6.DefaultIfEmpty()
                                   select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           sAssetID = t4.sAssetID,
                                           sLocation = t4.sLocation,
                                           sTypeService = t7.scName
                                       }).FirstOrDefaultAsync();
                    break;
            }
            return daily;
        }
        public async Task<ActionResult<List<DailyRequest>>>? ApiRequestFind()
        {
            var main = _lightingPlantContext.DailyRequest.ToList();
            //var main = await (from t0 in _lightingPlantContext.DailyRequest
            //            select new DailyRequest()
            //            {
            //                Id = t0.Id,
            //                drRefNo = t0.drRefNo,
            //                drDate = t0.drDate,
            //                drJobNo = t0.drJobNo,
            //                drRemark = t0.drRemark,
            //                sAssetID = t0.sAssetID,
            //                sLocation = t0.sLocation,
            //                Total1 = (from tX in this._lightingPlantContext.DailyRequestLabour.Where(c => c.drtRefNoID == t0.drRefNoID) select tX).Count(),
            //                Total2 = (from tX in this._lightingPlantContext.DailyRequestMaterial.Where(c => c.drsNoID == t0.drRefNoID) select tX).Count(),
            //            }).Distinct().ToListAsync();
            
            return main;
        }
        public async Task<ActionResult<List<ClientRef>>>? ApiClientRef()
        {
            var client = await _minorSystemContext.ClientRef.OrderBy(x => x.rClientRefType).ToListAsync();
            return client;
        }
        public async Task<ActionResult<List<Location>>>? ApiLocation()
        {
            var location = await _storeIDRContext.Location.OrderBy(x => x.lLocationName).ToListAsync();
            return location;
        }
        public async Task<ActionResult<List<Customer>>>? ApiCustomer()
        {
            var customer = await _storeIDRContext.Customer.OrderBy(x => x.cName).ToListAsync();
            return customer;
        }
        public async Task<ActionResult<List<Models.OrderType>>>? ApiOrderType()
        {
            var order = await _minorSystemContext.OrderType.OrderBy(x => x.rJobOrderType).ToListAsync();
            return order;
        }
        public async Task<ActionResult<List<CompleteBy>>>? ApiCompleteBy()
        {
            var complete = await _minorSystemContext.CompleteBy.OrderBy(x => x.rCompleteBy).ToListAsync();
            return complete;
        }
        public async Task<ActionResult<Repair>>? ApiOneRepair()
        {
            //var repair = await _lightingPlantContext.Repair.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var repair = await (from t1 in _lightingPlantContext.Repair.OrderByDescending(x => x.Id)
                            join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                            from t4 in t3.DefaultIfEmpty()
                            select new Repair()
                            {
                                Id = t1.Id,
                                rJobOrder = t1.rJobOrder,
                                rProject = t1.rProject,
                                rODGRefNo = t1.rODGRefNo,
                                rHeadTech = t1.rHeadTech,
                                rClientRefType = t1.rClientRefType,
                                rPRNo = t1.rPRNo,
                                rClientRefNo = t1.rClientRefNo,
                                rCostCenter = t1.rCostCenter,
                                rOriginator = t1.rOriginator,
                                rLocation = t1.rLocation,
                                rCustomer = t1.rCustomer,
                                rCompanyName = t1.rCompanyName,
                                rDateOrder = t1.rDateOrder,
                                rDateStart = t1.rDateStart,
                                rDateFinish = t1.rDateFinish,
                                rJobOrderType = t1.rJobOrderType,
                                rCompleteBy = t1.rCompleteBy,
                                rTenderNo = t1.rTenderNo,
                                rDescription = t1.rDescription,
                                rRemark = t1.rRemark,
                                rInvoiced = t1.rInvoiced,
                                rClose = t1.rClose,
                                rCancelled = t1.rCancelled,
                                rDateCancelled = t1.rDateCancelled,
                                rCancelledBy = t1.rCancelledBy,
                                rCancelledReason = t1.rCancelledReason,
                                rJobOrderGroup = t4.drRefNoID
                            }).FirstOrDefaultAsync();

            return repair;
        }
        public async Task<ResultJson>? ApiMainChecking(Repair main)
        {
            var getEnd = _lightingPlantContext.Repair.Where(p => p.rJobOrder == main.rJobOrder).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = Convert.ToString(main.rJobOrder) };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = Convert.ToString(main.rJobOrder) };
            }
        }
        public async Task<ActionResult<Repair>>? ApiMainBy(ActionModelData actionModel)
        {
            Repair repair = new Repair();
            switch (actionModel.Typ)
            {
                case "keep":
                    //repair = _lightingPlantContext.Repair.Where(x => x.Id == actionModel.Id).FirstOrDefault();
                   repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.Id == actionModel.Id)
                                    join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                    from t4 in t3.DefaultIfEmpty()
                                    select new Repair()
                                    {
                                        Id = t1.Id,
                                        rJobOrder = t1.rJobOrder,
                                        rProject = t1.rProject,
                                        rODGRefNo = t1.rODGRefNo,
                                        rHeadTech = t1.rHeadTech,
                                        rClientRefType = t1.rClientRefType,
                                        rPRNo = t1.rPRNo,
                                        rClientRefNo = t1.rClientRefNo,
                                        rCostCenter = t1.rCostCenter,
                                        rOriginator = t1.rOriginator,
                                        rLocation = t1.rLocation,
                                        rCustomer = t1.rCustomer,
                                        rCompanyName = t1.rCompanyName,
                                        rDateOrder = t1.rDateOrder,
                                        rDateStart = t1.rDateStart,
                                        rDateFinish = t1.rDateFinish,
                                        rJobOrderType = t1.rJobOrderType,
                                        rCompleteBy = t1.rCompleteBy,
                                        rTenderNo = t1.rTenderNo,
                                        rDescription = t1.rDescription,
                                        rRemark = t1.rRemark,
                                        rInvoiced = t1.rInvoiced,
                                        rClose = t1.rClose,
                                        rCancelled = t1.rCancelled,
                                        rDateCancelled = t1.rDateCancelled,
                                        rCancelledBy = t1.rCancelledBy,
                                        rCancelledReason = t1.rCancelledReason,
                                        rJobOrderGroup = t4.drRefNoID
                                    }).FirstOrDefaultAsync();
                    break;
                case "keep2":
                    //repair = _lightingPlantContext.Repair.Where(x => x.rJobOrder == Convert.ToInt32(actionModel.Idstr)).FirstOrDefault();
                    repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.rJobOrder == Convert.ToInt32(actionModel.Idstr))
                                    join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                    from t4 in t3.DefaultIfEmpty()
                                    select new Repair()
                                    {
                                        Id = t1.Id,
                                        rJobOrder = t1.rJobOrder,
                                        rProject = t1.rProject,
                                        rODGRefNo = t1.rODGRefNo,
                                        rHeadTech = t1.rHeadTech,
                                        rClientRefType = t1.rClientRefType,
                                        rPRNo = t1.rPRNo,
                                        rClientRefNo = t1.rClientRefNo,
                                        rCostCenter = t1.rCostCenter,
                                        rOriginator = t1.rOriginator,
                                        rLocation = t1.rLocation,
                                        rCustomer = t1.rCustomer,
                                        rCompanyName = t1.rCompanyName,
                                        rDateOrder = t1.rDateOrder,
                                        rDateStart = t1.rDateStart,
                                        rDateFinish = t1.rDateFinish,
                                        rJobOrderType = t1.rJobOrderType,
                                        rCompleteBy = t1.rCompleteBy,
                                        rTenderNo = t1.rTenderNo,
                                        rDescription = t1.rDescription,
                                        rRemark = t1.rRemark,
                                        rInvoiced = t1.rInvoiced,
                                        rClose = t1.rClose,
                                        rCancelled = t1.rCancelled,
                                        rDateCancelled = t1.rDateCancelled,
                                        rCancelledBy = t1.rCancelledBy,
                                        rCancelledReason = t1.rCancelledReason,
                                        rJobOrderGroup = t4.drRefNoID
                                    }).FirstOrDefaultAsync();
                    break;
                case "next":
                    var getEnd = _lightingPlantContext.Repair.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    //repair = _lightingPlantContext.Repair.Where(x => x.Id == (actionModel.Id + 1)).FirstOrDefault();
                    //repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.Id == (actionModel.Id + 1))
                    repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id)
                                    join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                    from t4 in t3.DefaultIfEmpty()
                                    select new Repair()
                                    {
                                        Id = t1.Id,
                                        rJobOrder = t1.rJobOrder,
                                        rProject = t1.rProject,
                                        rODGRefNo = t1.rODGRefNo,
                                        rHeadTech = t1.rHeadTech,
                                        rClientRefType = t1.rClientRefType,
                                        rPRNo = t1.rPRNo,
                                        rClientRefNo = t1.rClientRefNo,
                                        rCostCenter = t1.rCostCenter,
                                        rOriginator = t1.rOriginator,
                                        rLocation = t1.rLocation,
                                        rCustomer = t1.rCustomer,
                                        rCompanyName = t1.rCompanyName,
                                        rDateOrder = t1.rDateOrder,
                                        rDateStart = t1.rDateStart,
                                        rDateFinish = t1.rDateFinish,
                                        rJobOrderType = t1.rJobOrderType,
                                        rCompleteBy = t1.rCompleteBy,
                                        rTenderNo = t1.rTenderNo,
                                        rDescription = t1.rDescription,
                                        rRemark = t1.rRemark,
                                        rInvoiced = t1.rInvoiced,
                                        rClose = t1.rClose,
                                        rCancelled = t1.rCancelled,
                                        rDateCancelled = t1.rDateCancelled,
                                        rCancelledBy = t1.rCancelledBy,
                                        rCancelledReason = t1.rCancelledReason,
                                        rJobOrderGroup = t4.drRefNoID
                                    }).FirstOrDefaultAsync();
                    if (repair == null)
                    {
                        repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.Id == actionModel.Id)
                                        join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                        from t4 in t3.DefaultIfEmpty()
                                        select new Repair()
                                        {
                                            Id = t1.Id,
                                            rJobOrder = t1.rJobOrder,
                                            rProject = t1.rProject,
                                            rODGRefNo = t1.rODGRefNo,
                                            rHeadTech = t1.rHeadTech,
                                            rClientRefType = t1.rClientRefType,
                                            rPRNo = t1.rPRNo,
                                            rClientRefNo = t1.rClientRefNo,
                                            rCostCenter = t1.rCostCenter,
                                            rOriginator = t1.rOriginator,
                                            rLocation = t1.rLocation,
                                            rCustomer = t1.rCustomer,
                                            rCompanyName = t1.rCompanyName,
                                            rDateOrder = t1.rDateOrder,
                                            rDateStart = t1.rDateStart,
                                            rDateFinish = t1.rDateFinish,
                                            rJobOrderType = t1.rJobOrderType,
                                            rCompleteBy = t1.rCompleteBy,
                                            rTenderNo = t1.rTenderNo,
                                            rDescription = t1.rDescription,
                                            rRemark = t1.rRemark,
                                            rInvoiced = t1.rInvoiced,
                                            rClose = t1.rClose,
                                            rCancelled = t1.rCancelled,
                                            rDateCancelled = t1.rDateCancelled,
                                            rCancelledBy = t1.rCancelledBy,
                                            rCancelledReason = t1.rCancelledReason,
                                            rJobOrderGroup = t4.drRefNoID
                                        }).FirstOrDefaultAsync();
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    //repair = _lightingPlantContext.Repair.Where(x => x.Id == (actionModel.Id - 1)).FirstOrDefault();
                    //repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.Id == (actionModel.Id - 1))
                    repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id)
                                    join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                    from t4 in t3.DefaultIfEmpty()
                                    select new Repair()
                                    {
                                        Id = t1.Id,
                                        rJobOrder = t1.rJobOrder,
                                        rProject = t1.rProject,
                                        rODGRefNo = t1.rODGRefNo,
                                        rHeadTech = t1.rHeadTech,
                                        rClientRefType = t1.rClientRefType,
                                        rPRNo = t1.rPRNo,
                                        rClientRefNo = t1.rClientRefNo,
                                        rCostCenter = t1.rCostCenter,
                                        rOriginator = t1.rOriginator,
                                        rLocation = t1.rLocation,
                                        rCustomer = t1.rCustomer,
                                        rCompanyName = t1.rCompanyName,
                                        rDateOrder = t1.rDateOrder,
                                        rDateStart = t1.rDateStart,
                                        rDateFinish = t1.rDateFinish,
                                        rJobOrderType = t1.rJobOrderType,
                                        rCompleteBy = t1.rCompleteBy,
                                        rTenderNo = t1.rTenderNo,
                                        rDescription = t1.rDescription,
                                        rRemark = t1.rRemark,
                                        rInvoiced = t1.rInvoiced,
                                        rClose = t1.rClose,
                                        rCancelled = t1.rCancelled,
                                        rDateCancelled = t1.rDateCancelled,
                                        rCancelledBy = t1.rCancelledBy,
                                        rCancelledReason = t1.rCancelledReason,
                                        rJobOrderGroup = t4.drRefNoID
                                    }).FirstOrDefaultAsync();
                    if (repair == null)
                    {
                        repair = await (from t1 in _lightingPlantContext.Repair.Where(x => x.Id == actionModel.Id)
                                        join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                        from t4 in t3.DefaultIfEmpty()
                                        select new Repair()
                                        {
                                            Id = t1.Id,
                                            rJobOrder = t1.rJobOrder,
                                            rProject = t1.rProject,
                                            rODGRefNo = t1.rODGRefNo,
                                            rHeadTech = t1.rHeadTech,
                                            rClientRefType = t1.rClientRefType,
                                            rPRNo = t1.rPRNo,
                                            rClientRefNo = t1.rClientRefNo,
                                            rCostCenter = t1.rCostCenter,
                                            rOriginator = t1.rOriginator,
                                            rLocation = t1.rLocation,
                                            rCustomer = t1.rCustomer,
                                            rCompanyName = t1.rCompanyName,
                                            rDateOrder = t1.rDateOrder,
                                            rDateStart = t1.rDateStart,
                                            rDateFinish = t1.rDateFinish,
                                            rJobOrderType = t1.rJobOrderType,
                                            rCompleteBy = t1.rCompleteBy,
                                            rTenderNo = t1.rTenderNo,
                                            rDescription = t1.rDescription,
                                            rRemark = t1.rRemark,
                                            rInvoiced = t1.rInvoiced,
                                            rClose = t1.rClose,
                                            rCancelled = t1.rCancelled,
                                            rDateCancelled = t1.rDateCancelled,
                                            rCancelledBy = t1.rCancelledBy,
                                            rCancelledReason = t1.rCancelledReason,
                                            rJobOrderGroup = t4.drRefNoID
                                        }).FirstOrDefaultAsync();
                    }
                    break;
                case "end":
                    //repair = _lightingPlantContext.Repair.OrderByDescending(p => p.Id).First();
                    repair = await (from t1 in _lightingPlantContext.Repair.OrderByDescending(p => p.Id)
                                    join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                    from t4 in t3.DefaultIfEmpty()
                                    select new Repair()
                                    {
                                        Id = t1.Id,
                                        rJobOrder = t1.rJobOrder,
                                        rProject = t1.rProject,
                                        rODGRefNo = t1.rODGRefNo,
                                        rHeadTech = t1.rHeadTech,
                                        rClientRefType = t1.rClientRefType,
                                        rPRNo = t1.rPRNo,
                                        rClientRefNo = t1.rClientRefNo,
                                        rCostCenter = t1.rCostCenter,
                                        rOriginator = t1.rOriginator,
                                        rLocation = t1.rLocation,
                                        rCustomer = t1.rCustomer,
                                        rCompanyName = t1.rCompanyName,
                                        rDateOrder = t1.rDateOrder,
                                        rDateStart = t1.rDateStart,
                                        rDateFinish = t1.rDateFinish,
                                        rJobOrderType = t1.rJobOrderType,
                                        rCompleteBy = t1.rCompleteBy,
                                        rTenderNo = t1.rTenderNo,
                                        rDescription = t1.rDescription,
                                        rRemark = t1.rRemark,
                                        rInvoiced = t1.rInvoiced,
                                        rClose = t1.rClose,
                                        rCancelled = t1.rCancelled,
                                        rDateCancelled = t1.rDateCancelled,
                                        rCancelledBy = t1.rCancelledBy,
                                        rCancelledReason = t1.rCancelledReason,
                                        rJobOrderGroup = t4.drRefNoID
                                    }).FirstAsync();
                    break;
                default:
                    //repair = _lightingPlantContext.Repair.GroupBy(x => x.Id).Select(g => g.OrderBy(x => x.Id).First()).FirstOrDefault();
                    repair = await (from t1 in _lightingPlantContext.Repair.OrderBy(x => x.Id)
                                    join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                                    from t4 in t3.DefaultIfEmpty()
                                    select new Repair()
                                    {
                                        Id = t1.Id,
                                        rJobOrder = t1.rJobOrder,
                                        rProject = t1.rProject,
                                        rODGRefNo = t1.rODGRefNo,
                                        rHeadTech = t1.rHeadTech,
                                        rClientRefType = t1.rClientRefType,
                                        rPRNo = t1.rPRNo,
                                        rClientRefNo = t1.rClientRefNo,
                                        rCostCenter = t1.rCostCenter,
                                        rOriginator = t1.rOriginator,
                                        rLocation = t1.rLocation,
                                        rCustomer = t1.rCustomer,
                                        rCompanyName = t1.rCompanyName,
                                        rDateOrder = t1.rDateOrder,
                                        rDateStart = t1.rDateStart,
                                        rDateFinish = t1.rDateFinish,
                                        rJobOrderType = t1.rJobOrderType,
                                        rCompleteBy = t1.rCompleteBy,
                                        rTenderNo = t1.rTenderNo,
                                        rDescription = t1.rDescription,
                                        rRemark = t1.rRemark,
                                        rInvoiced = t1.rInvoiced,
                                        rClose = t1.rClose,
                                        rCancelled = t1.rCancelled,
                                        rDateCancelled = t1.rDateCancelled,
                                        rCancelledBy = t1.rCancelledBy,
                                        rCancelledReason = t1.rCancelledReason,
                                        rJobOrderGroup = t4.drRefNoID
                                    }).FirstOrDefaultAsync();
                    break;
            }
            return repair;
        }
        public async Task<ActionResult<List<Repair>>>? ApiMainFind()
        {

            var main = _lightingPlantContext.Repair.ToList();
            
            return main;
        }
        public async Task<ActionResult<Repair>>? ApiRepairRequest()
        {
            var daily = await (from t1 in _lightingPlantContext.Repair.OrderByDescending(x => x.Id)
                                join t2 in _lightingPlantContext.DailyRequest on t1.rJobOrder equals t2.drJobNo into t3
                               from t4 in t3.DefaultIfEmpty()
                               select new Repair()
                               {
                                   Id = t1.Id,
                                   rJobOrder = t1.rJobOrder,
                                   rProject = t1.rProject,
                                   rODGRefNo = t1.rODGRefNo,
                                   rDescription = t1.rDescription,
                                   rClose = t1.rClose,
                                   drRefNoID = t4.drRefNoID,
                                   drDate = t4.drDate,
                                   drOriginator = t4.drOriginator,
                                   drPercent = t4.drPercent
                               }).FirstOrDefaultAsync();
            return daily;
        }
        public async Task<ActionResult<List<Repair>>>? ApiRepairFind()
        {
            var main = _lightingPlantContext.Repair.ToList();

            return main;
        }
        public async Task<ActionResult<List<CCSCode>>>? ApiCCSCode()
        {
            var client = _storeIDRContext.CCSCode.OrderBy(x => x.ccsID).ToList();
            return client;
        }
        public async Task<ActionResult<List<CCSCodeNew>>>? ApiCCSCodeNew()
        {
            var client = _storeIDRContext.CCSCodeNew.OrderBy(x => x.sCCSNew).ToList();
            return client;
        }
        public async Task<ActionResult<List<Unit>>>? ApiUnit()
        {
            var client = _storeIDRContext.Unit.ToList();
            return client;
        }
        public async Task<ActionResult<List<Rack>>>? ApiRack()
        {
            var client = _storeIDRContext.Rack.ToList();
            return client;
        }
        public async Task<ActionResult<List<Bin>>>? ApiBin()
        {
            var client = _storeIDRContext.Bin.ToList();
            return client;
        }
        public async Task<ActionResult<StoreSum>>? ApiStoreSum(string sStockCode)
        {
            //var client = _storeIDRContext.StoreSum.Where(x => x.sStockCode == sStockCode).FirstOrDefault();
            //var clients = _storeIDRContext.Database.SqlQuery<string>($"select top 1 ftEmplName from tblForeman"); //bisa
            var client = await _storeIDRContext.StoreSum.FromSql<StoreSum>($"exec dbo.getStoreSum {sStockCode}").ToListAsync();
            return client.FirstOrDefault();
        }
        public async Task<ResultJson>? ApiStoreChecking(Store main)
        {
            var getEnd = _storeIDRContext.Store.Where(p => p.sCCSCode == main.sCCSCode).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = Convert.ToString(main.sCCSCode) };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = Convert.ToString(main.sCCSCode) };
            }
        }
        public async Task<ActionResult<Store>>? ApiStoreBy(ActionModelData actionModel)
        {
            Store store = new Store();
            switch (actionModel.Typ)
            {
                case "keep":
                    store = await _storeIDRContext.Store.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    if (store != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "keep2":
                    store = await _storeIDRContext.Store.Where(x => x.sCCSCode == Convert.ToInt32(actionModel.Idstr)).FirstOrDefaultAsync();
                    if (store != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "next":
                    var getEnd = _storeIDRContext.Store.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    store = await _storeIDRContext.Store.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (store != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    else
                    {
                        store = await _storeIDRContext.Store.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    store = await _storeIDRContext.Store.Where(x => x.Id  < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (store != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    else
                    {
                        store = await _storeIDRContext.Store.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "end":
                    store = await _storeIDRContext.Store.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (store != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                default:
                    store = await _storeIDRContext.Store.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (store != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == store.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            store.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
            }
            return store;
        }
        public async Task<ActionResult<List<Store>>>? ApiStoreFind()
        {

            var main = _storeIDRContext.Store.ToList();

            return main;
        }
        public async Task<ActionResult<DailyRequest>>? ApiStoreRequest()
        {

            var daily = await (from t1 in _lightingPlantContext.DailyRequest.OrderByDescending(x => x.Id)
                               join t2 in _lightingPlantContext.Repair on t1.drJobNo equals t2.rJobOrder into t3
                               from t4 in t3.DefaultIfEmpty()
                               select new DailyRequest()
                               {
                                   Id = t1.Id,
                                   drRefNo = t1.drRefNo,
                                   drDate = t1.drDate,
                                   drStoreMan = t1.drStoreMan,
                                   drForeman = t1.drForeman,
                                   drJobNo = t1.drJobNo,
                                   drPercent = t1.drPercent,
                                   drOriginator = t1.drOriginator,
                                   drRefNoID = t1.drRefNoID,
                                   drOrderTypeGroup = t1.drOrderTypeGroup,
                                   drJobNoID = t1.drJobNoID,
                                   drRemark = t1.drRemark,
                                   drCompleted = t1.drCompleted,
                                   rDateFinish = t4.rDateFinish,
                                   rLocation = t4.rLocation,
                                   rDescription = t4.rDescription
                               }).FirstOrDefaultAsync();
            return daily;
        }
        public async Task<ActionResult<List<vWINo>>>? ApiJob(int id)
        {

            List<vWINo> main = new List<vWINo>();
            //var _job = (from pro in _lightingPlantContext.Repair select pro.rJobOrder)
            // .Union(from cat in _lightingPlantContext.Service select cat.sID).Select(x => new Job
            // {
            //     rJobOrder = x.Value
            // });

            //var main = await (from t1 in _job.OrderByDescending(x => x.rJobOrder)
            //                   join t2 in _lightingPlantContext.Repair on t1.rJobOrder equals t2.rJobOrder into t3
            //                   from t4 in t3.DefaultIfEmpty()
            //                   select new Job()
            //                   {
            //                       rJobOrder = t1.rJobOrder,
            //                       rDateFinish = t4.rDateFinish,
            //                       rDescription = t4.rDescription,
            //                       rLocation = t4.rLocation
            //                   }).ToListAsync();
            if (id == 0)
            {
                main = await _storeIDRContext.vWINo.OrderByDescending(x => x.rJobOrder).ToListAsync();
            }
            else
            {
                main = await _storeIDRContext.vWINo.Where(x => x.rJobOrder == id).OrderByDescending(x => x.rJobOrder).ToListAsync();
            }
            
            return main;
        }
        public async Task<ActionResult<vWINo>>? ApiJobOne(int id)
        {

            vWINo main = new vWINo();
            //var _job = (from pro in _lightingPlantContext.Repair select pro.rJobOrder)
            // .Union(from cat in _lightingPlantContext.Service select cat.sID).Select(x => new Job
            // {
            //     rJobOrder = x.Value
            // });

            //var main = await (from t1 in _job.OrderByDescending(x => x.rJobOrder)
            //                   join t2 in _lightingPlantContext.Repair on t1.rJobOrder equals t2.rJobOrder into t3
            //                   from t4 in t3.DefaultIfEmpty()
            //                   select new Job()
            //                   {
            //                       rJobOrder = t1.rJobOrder,
            //                       rDateFinish = t4.rDateFinish,
            //                       rDescription = t4.rDescription,
            //                       rLocation = t4.rLocation
            //                   }).ToListAsync();
            if (id == 0)
            {
                main = await _storeIDRContext.vWINo.OrderByDescending(x => x.rJobOrder).FirstOrDefaultAsync();
            }
            else
            {
                main = await _storeIDRContext.vWINo.Where(x => x.rJobOrder == id).FirstOrDefaultAsync();
            }

            return main;
        }
        public async Task<ActionResult<List<Foreman>>>? ApiForeman()
        {

            var main = _storeIDRContext.Foreman.ToList();

            return main;
        }
        public async Task<ActionResult<List<StoreMan>>>? ApiStoreMan()
        {

            var main = _storeIDRContext.StoreMan.ToList();

            return main;
        }
        public async Task<ActionResult<DailyRequest>>? ApiRequestStoreBy(ActionModelData actionModel)
        {
            DailyRequest? daily = new DailyRequest();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id)
                                   join t2 in _lightingPlantContext.Repair on t1.drJobNo equals t2.rJobOrder into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           rDateFinish = t4.rDateFinish,
                                           rLocation = t4.rLocation,
                                           rDescription = t4.rDescription
                                       }).FirstOrDefaultAsync();
                    break;
                case "keep2":
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.drRefNoID == actionModel.Idstr)
                                       join t2 in _lightingPlantContext.Repair on t1.drJobNo equals t2.rJobOrder into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           rDateFinish = t4.rDateFinish,
                                           rLocation = t4.rLocation,
                                           rDescription = t4.rDescription
                                       }).FirstOrDefaultAsync();
                    break;
                case "next":
                    var getEnd = _lightingPlantContext.DailyRequest.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id)
                                       join t2 in _lightingPlantContext.Repair on t1.drJobNo equals t2.rJobOrder into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           rDateFinish = t4.rDateFinish,
                                           rLocation = t4.rLocation,
                                           rDescription = t4.rDescription
                                       }).FirstOrDefaultAsync();
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id)
                                       join t2 in _lightingPlantContext.Repair on t1.drJobNo equals t2.rJobOrder into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           rDateFinish = t4.rDateFinish,
                                           rLocation = t4.rLocation,
                                           rDescription = t4.rDescription
                                       }).FirstOrDefaultAsync();
                    if (daily == null)
                    {
                        daily = await (from t1 in _lightingPlantContext.DailyRequest.Where(x => x.Id == actionModel.Id)
                                       join t2 in _lightingPlantContext.Service on t1.drJobNo equals t2.sID into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       join t5 in _lightingPlantContext.ServicePrice on t4.sTypeService equals t5.scId into t6
                                       from t7 in t6.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           sAssetID = t4.sAssetID,
                                           sLocation = t4.sLocation,
                                           sTypeService = t7.scName
                                       }).FirstOrDefaultAsync();
                    }
                    break;
                case "end":
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.OrderByDescending(x => x.Id)
                                       join t2 in _lightingPlantContext.Repair on t1.drJobNo equals t2.rJobOrder into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           rDateFinish = t4.rDateFinish,
                                           rLocation = t4.rLocation,
                                           rDescription = t4.rDescription
                                       }).FirstOrDefaultAsync();
                    break;
                default:
                    daily = await (from t1 in _lightingPlantContext.DailyRequest.OrderBy(x => x.Id)
                                       join t2 in _lightingPlantContext.Repair on t1.drJobNo equals t2.rJobOrder into t3
                                       from t4 in t3.DefaultIfEmpty()
                                       select new DailyRequest()
                                       {
                                           Id = t1.Id,
                                           drRefNo = t1.drRefNo,
                                           drDate = t1.drDate,
                                           drStoreMan = t1.drStoreMan,
                                           drForeman = t1.drForeman,
                                           drJobNo = t1.drJobNo,
                                           drPercent = t1.drPercent,
                                           drOriginator = t1.drOriginator,
                                           drRefNoID = t1.drRefNoID,
                                           drOrderTypeGroup = t1.drOrderTypeGroup,
                                           drJobNoID = t1.drJobNoID,
                                           drRemark = t1.drRemark,
                                           drCompleted = t1.drCompleted,
                                           rDateFinish = t4.rDateFinish,
                                           rLocation = t4.rLocation,
                                           rDescription = t4.rDescription
                                       }).FirstOrDefaultAsync();
                    break;
            }
            return daily;
        }
        public async Task<ActionResult<List<PreparedBy>>>? ApiPreparedBy()
        {
            var client = _storeIDRContext.PreparedBy.ToList();
            return client;
        }
        public async Task<ActionResult<List<SupplierList>>>? ApiSupplierList()
        {
            var client = _storeIDRContext.SupplierList.ToList();
            return client;
        }
        public async Task<ActionResult<InternalReqOUT>>? ApiOneRequest()
        {
            var store = await _storeIDRContext.InternalReqOUT.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (store != null)
            {
                store.iroJobName = await _lightingPlantContext.Project.Where(y => y.pNo == store.iroJobNo).Select(x => x.pName).FirstOrDefaultAsync();
            }
            return store;
        }
        public async Task<ActionResult<InternalReq>>? ApiOneRequestIn()
        {
            var store = await _storeIDRContext.InternalReq.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (store != null)
            {
                store.irJobName = await _lightingPlantContext.Project.Where(y => y.pNo == store.irJobNo).Select(x => x.pName).FirstOrDefaultAsync();
            }
            return store;
        }
        public async Task<ActionResult<List<Currency>>>? ApiCurrency()
        {
            var client = _storeIDRContext.Currency.ToList();
            return client;
        }
        public async Task<ActionResult<List<RecBy>>>? ApiRecBy()
        {
            var client = _storeIDRContext.RecBy.ToList();
            return client;
        }
        public async Task<ResultJson>? ApiAssetIRequestOutChecking(InternalReqOUT daily)
        {
            var getEnd = _storeIDRContext.InternalReqOUT.Where(p => p.iroID == daily.iroID).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = daily.iroID };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = daily.iroID };
            }
        }
        public async Task<ResultJson>? ApiAssetIRequestInChecking(InternalReq daily)
        {
            var getEnd = _storeIDRContext.InternalReq.Where(p => p.irID == daily.irID).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = daily.irID };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = daily.irID };
            }
        }
        public async Task<ActionResult<List<InternalReqSubOUT>>>? ApiDailyIRequestSubOut(string irosNo)
        {

            var daily1 = await (from t1 in _storeIDRContext.InternalReqSubOUT.Where(x => x.irosNo == irosNo).OrderByDescending(y => y.irosID)
                               select new InternalReqSubOUT()
                               {
                                   Id = t1.Id,
                                   irosID = t1.irosID,
                                   irosNo = t1.irosNo,
                                   irosOrReqNo = t1.irosOrReqNo,
                                   irosQtyIN = t1.irosQtyIN,
                                   irosQtyBack = t1.irosQtyBack == null ? 0 : t1.irosQtyBack,
                                   irosStockCode = t1.irosStockCode,
                                   irosRemark = t1.irosRemark,
                                   irItem = t1.irItem,
                                   QtyUsed = Convert.ToDecimal(Convert.ToDecimal(t1.irosQtyIN) - Convert.ToDecimal(((t1.irosQtyBack == null) ? Convert.ToDecimal(0.00) : Convert.ToDecimal(t1.irosQtyBack)))),
                               }).ToListAsync();
            var daily2 = await (from t1 in _storeIDRContext.Store.OrderBy(y => y.sStockCode)
                                join t2 in _storeIDRContext.Unit.OrderBy(y => y.Id) on t1.sUnit equals t2.uID
                                select new Store()
                                {
                                    Id = t1.Id,
                                    sStockCode = t1.sStockCode,
                                    uDescription = t2.uDescription,
                                    sDescription = t1.sDescription
                                }).ToListAsync();
            var daily = (from t1 in daily1.OrderBy(y => y.irosStockCode) 
                               join t2 in daily2 on t1.irosStockCode equals t2.sStockCode into t3
                               from t4 in t3.DefaultIfEmpty()
                               select new InternalReqSubOUT()
                               {
                                   Id = t1.Id,
                                   irosID = t1.irosID,
                                   irosNo = t1.irosNo,
                                   irosOrReqNo = t1.irosOrReqNo,
                                   irosQtyIN = t1.irosQtyIN,
                                   irosQtyBack = t1.irosQtyBack,
                                   irosStockCode = t1.irosStockCode,
                                   irosRemark = t1.irosRemark,
                                   irItem = t1.irItem,
                                   QtyUsed = t1.QtyUsed,
                                   sDescription = ((t4 == null) ? "" : t4.sDescription),
                                   uDescription = ((t4 == null) ? "" : t4.uDescription)
                               }).ToList();
            return daily;
        }
        public async Task<ActionResult<List<InternalReqSub>>>? ApiDailyIRequestSubIn(string irsNo)
        {
            var daily1 = await (from t1 in _storeIDRContext.InternalReqSub.Where(x => x.irsNo == irsNo).OrderByDescending(y => y.irsItem)
                                select new InternalReqSub()
                                {
                                    Id = t1.Id,
                                    irsID = t1.irsID,
                                    irsNo = t1.irsNo,
                                    irsQtyIN = t1.irsQtyIN,
                                    irsQtyBack = t1.irsQtyBack == null ? 0 : t1.irsQtyBack,
                                    irsStockCode = t1.irsStockCode,
                                    irsUnitCost = t1.irsUnitCost,
                                    irsItem = t1.irsItem,
                                    QtyUsed = Convert.ToDecimal(Convert.ToDecimal(t1.irsQtyIN) - Convert.ToDecimal(((t1.irsQtyBack == null) ? Convert.ToDecimal(0.00) : Convert.ToDecimal(t1.irsQtyBack)))),
                                }).ToListAsync();
            var daily2 = await (from t1 in _storeIDRContext.Store.OrderBy(y => y.sStockCode)
                                join t2 in _storeIDRContext.Unit.OrderBy(y => y.Id) on t1.sUnit equals t2.uID
                                select new Store()
                                {
                                    Id = t1.Id,
                                    sStockCode = t1.sStockCode,
                                    uDescription = t2.uDescription,
                                    sDescription = t1.sDescription
                                }).ToListAsync();
            var daily = (from t1 in daily1.OrderBy(y => y.irsItem)
                         join t2 in daily2 on t1.irsStockCode equals t2.sStockCode into t3
                         from t4 in t3.DefaultIfEmpty()
                         select new InternalReqSub()
                         {
                             Id = t1.Id,
                             irsID = t1.irsID,
                             irsNo = t1.irsNo,
                             irsQtyIN = t1.irsQtyIN,
                             irsQtyBack = t1.irsQtyBack,
                             irsStockCode = t1.irsStockCode,
                             irsUnitCost = t1.irsUnitCost,
                             irsItem = t1.irsItem,
                             QtyUsed = t1.QtyUsed,
                             sDescription = ((t4 == null) ? "" : t4.sDescription),
                             uDescription = ((t4 == null) ? "" : t4.uDescription)
                         }).ToList();
            return daily;
        }
        public async Task<ActionResult<List<InternalReqOUT>>>? ApiIRequestOutFind()
        {
            var main = _storeIDRContext.InternalReqOUT.ToList();
            //var main = await (from t0 in _lightingPlantContext.DailyRequest
            //            select new DailyRequest()
            //            {
            //                Id = t0.Id,
            //                drRefNo = t0.drRefNo,
            //                drDate = t0.drDate,
            //                drJobNo = t0.drJobNo,
            //                drRemark = t0.drRemark,
            //                sAssetID = t0.sAssetID,
            //                sLocation = t0.sLocation,
            //                Total1 = (from tX in this._lightingPlantContext.DailyRequestLabour.Where(c => c.drtRefNoID == t0.drRefNoID) select tX).Count(),
            //                Total2 = (from tX in this._lightingPlantContext.DailyRequestMaterial.Where(c => c.drsNoID == t0.drRefNoID) select tX).Count(),
            //            }).Distinct().ToListAsync();

            return main;
        }
        public async Task<ActionResult<List<InternalReq>>>? ApiIRequestInFind()
        {
            var main = _storeIDRContext.InternalReq.ToList();
            return main;
        }
        public async Task<ActionResult<InternalReqOUT>>? ApiIRequestOutBy(ActionModelData actionModel)
        {
            InternalReqOUT? daily = new InternalReqOUT();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await _storeIDRContext.InternalReqOUT.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.iroJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.iroJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "keep2":
                    daily = await _storeIDRContext.InternalReqOUT.Where(x => x.iroID == actionModel.Idstr).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.iroJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.iroJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "next":
                    var getEnd = _storeIDRContext.InternalReqOUT.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);

                    daily = await _storeIDRContext.InternalReqOUT.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.iroJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.iroJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await _storeIDRContext.InternalReqOUT.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.iroJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.iroJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "end":
                    daily = await _storeIDRContext.InternalReqOUT.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.iroJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.iroJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }

                    break;
                default:
                    daily = await _storeIDRContext.InternalReqOUT.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.iroJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.iroJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }

                    break;
            }
            return daily;
        }
        public async Task<ActionResult<InternalReq>>? ApiIRequestInBy(ActionModelData actionModel)
        {
            InternalReq? daily = new InternalReq();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await _storeIDRContext.InternalReq.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.irJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.irJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "keep2":
                    daily = await _storeIDRContext.InternalReq.Where(x => x.irID == actionModel.Idstr).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.irJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.irJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "next":
                    var getEnd = _storeIDRContext.InternalReq.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);

                    daily = await _storeIDRContext.InternalReq.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.irJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.irJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await _storeIDRContext.InternalReq.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.irJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.irJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "end":
                    daily = await _storeIDRContext.InternalReq.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.irJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.irJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }

                    break;
                default:
                    daily = await _storeIDRContext.InternalReq.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.irJobName = await _lightingPlantContext.Project.Where(y => y.pNo == daily.irJobNo).Select(x => x.pName).FirstOrDefaultAsync();
                    }

                    break;
            }
            return daily;
        }
        public async Task<ActionResult<List<ListMaterialOutSummary>>>? ApiListMaterialOutSummary(string key = "")
        {
            List<ListMaterialOutSummary> project = new List<ListMaterialOutSummary>();
            if (key=="")
            {
                project = _storeIDRContext.ListMaterialOutSummary.OrderBy(x => x.JobNo).ToList();
            }
            else
            {
                project = _storeIDRContext.ListMaterialOutSummary.Where(x => x.JobNo.Contains(key)).ToList();
            }
            return project;
        }
        public async Task<ActionResult<List<ListMaterialOut>>>? ApiListMaterialOut(string key = "")
        {
            var project = _storeIDRContext.ListMaterialOut.OrderBy(x => x.JobNo).ToList();
            return project;
        }
        public async Task<ActionResult<Adjustment>>? ApiAdjustment()
        {
            var adjustment = await _storeIDRContext.Adjustment.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return adjustment;
        }
        public async Task<ActionResult<List<AdjustmentSub>>>? ApiAdjustmentSub(string adNo)
        {
            
            var daily1 = await (from t1 in _storeIDRContext.AdjustmentSub.Where(x => x.adsMainNo == Convert.ToInt32(adNo)).OrderByDescending(y => y.adsItem)
                                select new AdjustmentSub()
                                {
                                    Id = t1.Id,
                                    adsNo = t1.adsNo,
                                    adsMainNo = t1.adsMainNo,
                                    adsStockCode = t1.adsStockCode,
                                    adsQty = t1.adsQty,
                                    adsQtyBack = t1.adsQtyBack,
                                    adsRemark = t1.adsRemark,
                                    adsItem = t1.adsItem,
                                    QtyUsed = Convert.ToDecimal(Convert.ToDecimal(t1.adsQty) - Convert.ToDecimal(((t1.adsQtyBack == null) ? Convert.ToDecimal(0.00) : Convert.ToDecimal(t1.adsQtyBack))))
                                }).ToListAsync();
            var daily2 = await (from t1 in _storeIDRContext.Store.OrderBy(y => y.sStockCode)
                                join t2 in _storeIDRContext.Unit.OrderBy(y => y.Id) on t1.sUnit equals t2.uID
                                select new Store()
                                {
                                    Id = t1.Id,
                                    sStockCode = t1.sStockCode,
                                    uDescription = t2.uDescription,
                                    sDescription = t1.sDescription
                                }).ToListAsync();
            var daily = (from t1 in daily1.OrderBy(y => y.adsStockCode)
                         join t2 in daily2 on t1.adsStockCode equals t2.sStockCode into t3
                         from t4 in t3.DefaultIfEmpty()
                         select new AdjustmentSub()
                         {
                             Id = t1.Id,
                             adsNo = t1.adsNo,
                             adsMainNo = t1.adsMainNo,
                             adsStockCode = t1.adsStockCode,
                             adsQty = t1.adsQty,
                             adsQtyBack = t1.adsQtyBack,
                             adsRemark = t1.adsRemark,
                             adsItem = t1.adsItem,
                             QtyUsed = t1.QtyUsed,
                             sDescription = ((t4 == null) ? "" : t4.sDescription),
                             uDescription = ((t4 == null) ? "" : t4.uDescription)
                         }).ToList();
            return daily;
        }
        public async Task<ActionResult<List<Adjustment>>>? ApiAdjustmentFind()
        {
            var main = _storeIDRContext.Adjustment.ToList();
            return main;
        }
        public async Task<ResultJson>? ApiAdjustmentChecking(Adjustment daily)
        {
            var getEnd = _storeIDRContext.Adjustment.Where(p => p.adNo == daily.adNo).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = daily.adNo };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = daily.adNo };
            }
        }
        public async Task<ActionResult<Adjustment>>? ApiAdjustmentBy(ActionModelData actionModel)
        {
            Adjustment? daily = new Adjustment();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await _storeIDRContext.Adjustment.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    break;
                case "keep2":
                    daily = await _storeIDRContext.Adjustment.Where(x => x.adNo == actionModel.Idstr).FirstOrDefaultAsync();
                    break;
                case "next":
                    var getEnd = _storeIDRContext.Adjustment.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    daily = await _storeIDRContext.Adjustment.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await _storeIDRContext.Adjustment.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    break;
                case "end":
                    daily = await _storeIDRContext.Adjustment.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    break;
                default:
                    daily = await _storeIDRContext.Adjustment.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    break;
            }
            return daily;
        }
        public async Task<ActionResult<Shipping>>? ApiShipping()
        {
            var shipping = await _storeIDRContext.Shipping.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return shipping;
        }
        public async Task<ActionResult<List<vShippingSub>>>? ApiShippingSub(string sisNo)
        {
            var daily = await _storeIDRContext.vShippingSub.Where(x => x.sisNo == sisNo).OrderBy(y => y.sisItem).ToListAsync();
                             
            return daily;
        }
        public async Task<ActionResult<List<FreightType>>>? ApiFreightType()
        {
            var client = _storeIDRContext.FreightType.ToList();
            return client;
        }
        public async Task<ActionResult<List<vShippingPONo>>>? ApiShippingPONo()
        {
            var client = _storeIDRContext.vShippingPONo.ToList();
            return client;
        }
        public async Task<ActionResult<List<Shipping>>>? ApiShippingFind()
        {
            var main = _storeIDRContext.Shipping.ToList();
            return main;
        }
        public async Task<ActionResult<List<vStoreIssueSub>>>? ApiStoreIssueSub(string sisNo)
        {
            var daily = await _storeIDRContext.vStoreIssueSub.Where(x => x.siNo == sisNo).OrderByDescending(y => y.siItem).ToListAsync();

            return daily;
        }
        public async Task<ActionResult<List<vOrReqNo>>>? ApiOrReqNo()
        {
            var client = _storeIDRContext.vOrReqNo.ToList();
            return client;
        }
        public async Task<ResultJson>? ApiShippingChecking(Shipping daily)
        {
            var getEnd = _storeIDRContext.Shipping.Where(p => p.siID == daily.siID).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = daily.siID };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = daily.siID };
            }
        }
        public async Task<ActionResult<Shipping>>? ApiShippingBy(ActionModelData actionModel)
        {
            Shipping? daily = new Shipping();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await _storeIDRContext.Shipping.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    break;
                case "keep2":
                    daily = await _storeIDRContext.Shipping.Where(x => x.siID == actionModel.Idstr).FirstOrDefaultAsync();
                    break;
                case "next":
                    var getEnd = _storeIDRContext.Shipping.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    daily = await _storeIDRContext.Shipping.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await _storeIDRContext.Shipping.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    break;
                case "end":
                    daily = await _storeIDRContext.Shipping.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    break;
                default:
                    daily = await _storeIDRContext.Shipping.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    break;
            }
            return daily;
        }
        public async Task<ActionResult<Docket>>? ApiDocket()
        {
            var docket = await _storeIDRContext.Docket.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return docket;
        }
        public async Task<ActionResult<List<vDocketSub>>>? ApiDocketSub(string ddsNo)
        {
            var daily = await _storeIDRContext.vDocketSub.Where(x => x.ddsNo == ddsNo).OrderBy(y => y.ddsItem).ToListAsync();

            return daily;
        }
        public async Task<ActionResult<List<Docket>>>? ApiDocketFind()
        {
            var main = _storeIDRContext.Docket.ToList();
            return main;
        }
        public async Task<ResultJson>? ApiDocketChecking(Docket daily)
        {
            var getEnd = _storeIDRContext.Docket.Where(p => p.ddID == daily.ddID).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = daily.ddID };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = daily.ddID };
            }
        }
        public async Task<ActionResult<Docket>>? ApiDocketBy(ActionModelData actionModel)
        {
            Docket? daily = new Docket();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await _storeIDRContext.Docket.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    break;
                case "keep2":
                    daily = await _storeIDRContext.Docket.Where(x => x.ddID == actionModel.Idstr).FirstOrDefaultAsync();
                    break;
                case "next":
                    var getEnd = _storeIDRContext.Docket.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    daily = await _storeIDRContext.Docket.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await _storeIDRContext.Docket.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    break;
                case "end":
                    daily = await _storeIDRContext.Docket.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    break;
                default:
                    daily = await _storeIDRContext.Docket.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    break;
            }
            return daily;
        }
        public async Task<ActionResult<BackCharge>>? ApiBackCharge()
        {
            var back = await _storeIDRContext.BackCharge.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (back != null)
            {
                back.bJobName = await _lightingPlantContext.Project.Where(y => y.pNo == Convert.ToString(back.bJobNo)).Select(x => x.pName).FirstOrDefaultAsync();
            }
            return back;
        }
        public async Task<ActionResult<List<BackChargeSub>>>? ApiBackChargeSub(int? bsKPCReqNo)
        {
            var daily1 = await (from t1 in _storeIDRContext.BackChargeSub.Where(x => x.bsKPCReqNo == bsKPCReqNo).OrderByDescending(y => y.bsKPCReqNo)
                                select new BackChargeSub()
                                {
                                    Id = t1.Id,
                                    bsID = t1.bsID,
                                    bsKPCReqNo = t1.bsKPCReqNo,
                                    bsKPCStoreNo = t1.bsKPCStoreNo,
                                    bsODGStockCode = t1.bsODGStockCode,
                                    bsQty = t1.bsQty,
                                    bsUnitCost = t1.bsUnitCost,
                                    bsQtyUsed = t1.bsQtyUsed,
                                    bsQtyBack = Convert.ToDecimal(Convert.ToDecimal(t1.bsQty) - Convert.ToDecimal(((t1.bsQtyUsed == null) ? Convert.ToDecimal(0.00) : Convert.ToDecimal(t1.bsQtyUsed)))),
                                }).ToListAsync();
            var daily2 = await (from t1 in _storeIDRContext.Store.OrderBy(y => y.sStockCode)
                                join t2 in _storeIDRContext.Unit.OrderBy(y => y.Id) on t1.sUnit equals t2.uID
                                select new Store()
                                {
                                    Id = t1.Id,
                                    sStockCode = t1.sStockCode,
                                    uDescription = t2.uDescription,
                                    sDescription = t1.sDescription
                                }).ToListAsync();
            var daily = (from t1 in daily1.OrderBy(y => y.bsKPCStoreNo)
                         join t2 in daily2 on t1.bsODGStockCode equals t2.sStockCode into t3
                         from t4 in t3.DefaultIfEmpty()
                         select new BackChargeSub()
                         {
                             Id = t1.Id,
                             bsID = t1.bsID,
                             bsKPCReqNo = t1.bsKPCReqNo,
                             bsKPCStoreNo = t1.bsKPCStoreNo,
                             bsODGStockCode = t1.bsODGStockCode,
                             bsQty = t1.bsQty,
                             bsUnitCost = t1.bsUnitCost,
                             bsQtyUsed = t1.bsQtyUsed,
                             bsQtyBack = t1.bsQtyBack,
                             sDescription = ((t4 == null) ? "" : t4.sDescription),
                             uDescription = ((t4 == null) ? "" : t4.uDescription)
                         }).ToList();
            return daily;
        }
        public async Task<ActionResult<List<BackCharge>>>? ApiBackChargeFind()
        {
            var main = _storeIDRContext.BackCharge.ToList();
            return main;
        }
        public async Task<ResultJson>? ApiBackChargeChecking(BackCharge daily)
        {
            var getEnd = _storeIDRContext.BackCharge.Where(p => p.bKPCReqNo == daily.bKPCReqNo).FirstOrDefault();
            if (getEnd != null)
            {
                return new ResultJson { success = true, result = getEnd.Id, typ_ = Convert.ToString(daily.bKPCReqNo) };
            }
            else
            {

                return new ResultJson { success = false, result = 0, typ_ = Convert.ToString(daily.bKPCReqNo) };
            }
        }
        public async Task<ActionResult<BackCharge>>? ApiBackChargeBy(ActionModelData actionModel)
        {
            BackCharge? daily = new BackCharge();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await _storeIDRContext.BackCharge.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.bJobName = await _lightingPlantContext.Project.Where(y => y.pNo == Convert.ToString(daily.bJobNo)).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "keep2":
                    daily = await _storeIDRContext.BackCharge.Where(x => x.bKPCReqNo == Convert.ToInt32(actionModel.Idstr)).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.bJobName = await _lightingPlantContext.Project.Where(y => y.pNo == Convert.ToString(daily.bJobNo)).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "next":
                    var getEnd = _storeIDRContext.InternalReq.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);

                    daily = await _storeIDRContext.BackCharge.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.bJobName = await _lightingPlantContext.Project.Where(y => y.pNo == Convert.ToString(daily.bJobNo)).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await _storeIDRContext.BackCharge.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.bJobName = await _lightingPlantContext.Project.Where(y => y.pNo == Convert.ToString(daily.bJobNo)).Select(x => x.pName).FirstOrDefaultAsync();
                    }
                    break;
                case "end":
                    daily = await _storeIDRContext.BackCharge.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.bJobName = await _lightingPlantContext.Project.Where(y => y.pNo == Convert.ToString(daily.bJobNo)).Select(x => x.pName).FirstOrDefaultAsync();
                    }

                    break;
                default:
                    daily = await _storeIDRContext.BackCharge.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        daily.bJobName = await _lightingPlantContext.Project.Where(y => y.pNo == Convert.ToString(daily.bJobNo)).Select(x => x.pName).FirstOrDefaultAsync();
                    }

                    break;
            }
            return daily;
        }
        public async Task<ActionResult<List<StockIn>>>? ApiStockIn(string code)
        {
            decimal? _inSum = _storeIDRContext.StockIn.Where(x => x.StockCode == code).Sum(x => x.Quantity);
            var daily = await (from t1 in _storeIDRContext.StockIn.Where(x => x.StockCode == code).OrderByDescending(y => y.Date)
                               select new StockIn()
                               {
                                   StockCode = t1.StockCode,
                                   Date = t1.Date,
                                   FreightType = t1.FreightType,
                                   NoRef = t1.NoRef,
                                   Quantity = t1.Quantity,
                                   UnitCost = t1.UnitCost,
                                   FreightAlloc = t1.FreightAlloc,
                                   Supplier = t1.Supplier,
                                   Currency = t1.Currency,
                                   Type = t1.Type,
                                   PONo = t1.PONo,
                                   Project = t1.Project,
                                   inSum = _inSum
                               }).ToListAsync();
            return daily;
        }
        public async Task<ActionResult<List<StockOut>>>? ApiStockOut(string code)
        {
            decimal? _outSum = _storeIDRContext.StockOut.Where(x => x.StockCode == code).Sum(x => x.QtyUsed);
            var daily = await (from t1 in _storeIDRContext.StockOut.Where(x => x.StockCode == code).OrderByDescending(y => y.Date)
                               select new StockOut()
                               {
                                   StoreMan = t1.StoreMan,
                                   Order = t1.Order,
                                   JobNo = t1.JobNo,
                                   Job = t1.Job,
                                   Date = t1.Date,
                                   RefNo = t1.RefNo,
                                   StockCode = t1.StockCode,
                                   Type = t1.Type,
                                   QtyUsed = t1.QtyUsed,
                                   Project = t1.Project,
                                   pName = t1.pName,
                                   outSum = _outSum
                               }).ToListAsync();

            return daily;
        }
        public async Task<ActionResult<Store>>? ApiStockDetailBy(ActionModelData actionModel)
        {
            Store? daily = new Store();
            switch (actionModel.Typ)
            {
                case "keep":
                    daily = await _storeIDRContext.Store.Where(x => x.Id == actionModel.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == daily.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            daily.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "keep2":
                    daily = await _storeIDRContext.Store.Where(x => x.sStockCode == actionModel.Idstr).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == daily.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            daily.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "next":
                    var getEnd = _storeIDRContext.Store.OrderByDescending(p => p.Id).First();
                    actionModel.Id = ((getEnd.Id == actionModel.Id) ? getEnd.Id - 1 : actionModel.Id);
                    daily = await _storeIDRContext.Store.Where(x => x.Id > actionModel.Id).OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == daily.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            daily.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "prev":
                    actionModel.Id = ((actionModel.Id == 1) ? actionModel.Id = 2 : actionModel.Id);
                    daily = await _storeIDRContext.Store.Where(x => x.Id < actionModel.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == daily.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            daily.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                case "end":
                    daily = await _storeIDRContext.Store.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == daily.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            daily.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
                default:
                    daily = await _storeIDRContext.Store.OrderBy(x => x.Id).FirstOrDefaultAsync();
                    if (daily != null)
                    {
                        var cCSCode = await _storeIDRContext.CCSCode.Where(x => x.ccsID == daily.sCCSCode).FirstOrDefaultAsync();
                        if (cCSCode != null)
                        {
                            daily.sCCSName = cCSCode.ccsDescription;
                        }
                    }
                    break;
            }
            return daily;
        }
        public async Task<ActionResult<List<ReportMaster>>>? ApiReportMaster()
        {
            var main = _storeIDRContext.ReportMaster.ToList();
            return main;
        }
        public async Task<ActionResult<ReportMaster>>? ApiOneReportMaster(int Id)
        {
            var main = _storeIDRContext.ReportMaster.Where(x => x.Id == Id).FirstOrDefault();
            return main;
        }
        public async Task<ActionResult<List<DropDownReport>>>? ApiDropDownReport(string tbl, string clm1, string clm2)
        {
            var sql = System.String.Format("select Convert(varchar,{0}) AS Code,Convert(varchar,{1}) AS Description from {2} order by [{3}]", clm1, clm2, tbl, clm1);
            var client = _storeIDRContext.DropDownReport.FromSqlRaw($"{sql}");
            return client.ToList();
        }
        public async Task<IQueryable<string>>? ApiListReport(string query)
        {
            //var client = _storeIDRContext.Database.SqlQueryRaw<string>($"{query}");
            var client = _storeIDRContext.Database.SqlQueryRaw<string>($"{query}");
            return client;
        }
    }   

}
