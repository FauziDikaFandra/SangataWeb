using Microsoft.AspNetCore.Mvc;
using SangataWeb.Models;
using System.Collections.Generic;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using SangataWeb.Class;
using System.Security.Cryptography;
using System;
using Newtonsoft.Json;
using Azure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using static System.Collections.Specialized.BitVector32;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Http;


namespace SangataWeb.Controllers
{
    public class LightingPlantController : Controller
    {
        private readonly IGetData? _getData;
        private readonly ISetData? _setData;
        private readonly IDelData? _delData;
        public LightingPlantController(IGetData? getData, ISetData? setData, IDelData? delData)
        {
            _getData = getData;
            _setData = setData;
            _delData = delData;
        }
        public IActionResult Index()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vAssetType = GetAssetMain("Type","");
                mymodel.vAssetModel = GetAssetMain("Model","");
                mymodel.vServicePrice = GetServicePrice();
                mymodel.vTechnician = GetTechnician();
                mymodel.vProject = GetProject();
                mymodel.vRepairGroup = GetRepairGroup();
                mymodel.vAssetMain = GetAssetMainBy(new ActionModelData { Typ= "end", Id = 0 });
                mymodel.vAssetMainFind = GetAssetMainFindLoad();
                if (mymodel.vAssetMain != null)
                {
                    //mymodel.vServices = new List<Service>();
                    //mymodel.vRepair = new List<Repair>();
                    mymodel.vServices = GetService(mymodel.vAssetMain.aID);
                    mymodel.vRepair = GetRepair(mymodel.vAssetMain.aID);
                }
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult MaterialRequest()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vDailyRequest = GetDailyRequest();
                if (mymodel.vDailyRequest != null)
                {
                    mymodel.vDailyRequestLabour = GetDailyRequestLabour(mymodel.vDailyRequest.drRefNoID);
                    mymodel.vDailyRequestMaterial = GetDailyRequestMaterial(mymodel.vDailyRequest.drRefNoID);
                }
                mymodel.vStore = GetStore();
                mymodel.vTechnician = GetTechnician();
                mymodel.vTechnicianRate = GetTechnicianRate();
                mymodel.vRequestFind = GetRequestFindLoad();

                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public Users GetUser()
        {

            Users header = new Users();
            header.UserName = HttpContext.Session.GetString("UserName");
            return header;
        }

        public List<AssetMain> GetAssetMain(string typ, string aID)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiAssetMain(typ, aID);
            var query = mdlAssetMain.Result.Value.ToList();
            return query;
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> GetAssetMainAll([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlAssetMain = _getData.ApiAssetMainBy(actionModel);
            var rslAssetMain = mdlAssetMain.Result.Value;

            var mdlService = _getData.ApiService(rslAssetMain.aID);
            var rslService = mdlService.Result.Value;

            var mdlRepair = _getData.ApiRepair(rslAssetMain.aID);
            var rslRepair = mdlRepair.Result.Value;

            viewModel.vAssetMain = rslAssetMain;
            viewModel.vServices = rslService;
            viewModel.vRepair = rslRepair;
            return new JsonResult(viewModel);

        }

        public AssetMain GetAssetMainBy(ActionModelData actionModel)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiAssetMainBy(actionModel);
            var query = mdlAssetMain.Result.Value;
            return query;
        }

        public List<ServicePrice> GetServicePrice()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlServicePrice = _getData.ApiServicePrice();
            var query = mdlServicePrice.Result.Value.ToList();
            //List<ServicePrice> newquery = new List<ServicePrice>();
            //foreach (ServicePrice srv in query)
            //{
            //    ServicePrice srvdtl = new ServicePrice();
            //    srvdtl.scId = srv.scId;
            //    srvdtl.scName = srv.scName;
            //    srvdtl.scPrice = srv.scPrice;
            //    srvdtl.scDescription = srv.scName.ToString().Trim().PadRight(10 - srv.scName.ToString().Trim().Length) + " | " + srv.scPrice.ToString();
            //    srvdtl.scDescription = srvdtl.scDescription.Replace(" ", "&nbsp;");
            //    newquery.Add(srvdtl);
            //}
            return query;
        }

        public List<Technician> GetTechnician()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlTechnicianDD = _getData.ApiTechnician();
            var query = mdlTechnicianDD.Result.Value.ToList();
            return query;
        }

        public List<Technician> GetTechnicianRate()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlTechnician = _getData.ApiTechnicianRate();
            var query = mdlTechnician.Result.Value.ToList();
            return query;
        }

        public List<Project> GetProject()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlProject = _getData.ApiProject();
            var query = mdlProject.Result.Value.ToList();
            return query;
        }

        public List<RepairGroup> GetRepairGroup()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRepairGroup = _getData.ApiRepairGroup();
            var query = mdlRepairGroup.Result.Value.ToList();
            return query;
        }

        public List<Service> GetService(string sAssetID)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlService = _getData.ApiService(sAssetID);
            var query = mdlService.Result.Value.ToList();
            return query;
        }
        public List<Repair> GetRepair(string sAssetID)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRepair = _getData.ApiRepair(sAssetID);
            var query = mdlRepair.Result.Value.ToList();
            return query;
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveServices([FromBody] Service service)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svService = null;
            string typ = ((service.Id == 0) ? "Create" : "Update") ;
            svService = _setData.ApiCreateService(service, typ).Result;
            return new JsonResult(svService);
        }

        [HttpPost]
        public async Task<ActionResult<ResultJson>> GetExistingAssetMain([FromBody] AssetMain main)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiAssetMainChecking(main).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateAssetMain(main, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateAssetMain([FromBody] AssetMain main)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svMain = new ResultJson();
            svMain = _setData.ApiCreateAssetMain(main, "Update").Result;
            return svMain;
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveBreakdowns([FromBody] Repair repair)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svRepair = null;
            string typ = ((repair.Id == 0) ? "Create" : "Update");
            svRepair = _setData.ApiCreateBreakdowns(repair, typ).Result;
            return new JsonResult(svRepair);
        }

        [HttpPost]
        public async Task<ActionResult<string>> DeleteMain([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            switch (actionModel.Typ) {
                case "service":
                    svAction = _delData.ApiDeleteService(actionModel.Id).Result;
                    break;
                case "Breakdowns":
                    svAction = _delData.ApiDeleteBreakdowns(actionModel.Id).Result;
                    break;
                case "Labour":
                    svAction = _delData.ApiDeleteLabour(actionModel.Id).Result;
                    break;
                default:
                    svAction = _delData.ApiDeleteMaterial(actionModel.Id).Result;
                    break;
            }
            return new JsonResult(svAction);
        }

        
        [HttpPost]
        public async Task<ActionResult<List<AssetMain>>> GetAssetMainFind()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiAssetMainFind();
            var query = mdlAssetMain.Result.Value.ToList();
            return query;
        }

        public List<AssetMain> GetAssetMainFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiAssetMainFind().Result;
            var query = mdlAssetMain.Value;
            return query;
        }

        public DailyRequest GetDailyRequest()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDailyRequest = _getData.ApiDailyRequest();
            var query = mdlDailyRequest.Result.Value;
            return query;
        }

        public List<DailyRequestLabour> GetDailyRequestLabour(string drRefNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDailyRequestLabour(drRefNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }

        public List<DailyRequestMaterial> GetDailyRequestMaterial(string drRefNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDailyRequestMaterial(drRefNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }

        [HttpPost]
        public async Task<ActionResult<ResultJson>> GetExistingRequest([FromBody] DailyRequest daily)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiAssetRequestChecking(daily).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateRequest(daily, "Create").Result;
            }
            return svMain;
        }

        public List<Store> GetStore()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlStore = _getData.ApiStore();
            var query = mdlStore.Result.Value.ToList();
            return query;
        }

        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateRequest([FromBody] DailyRequest daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateRequest(daily, "Update").Result;
            return svDaily;
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveLabour([FromBody] DailyRequestLabour daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svLabour = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svLabour = _setData.ApiCreateLabour(daily, typ).Result;
            return new JsonResult(svLabour);
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveMaterial([FromBody] DailyRequestMaterial daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateMaterial(daily, typ).Result;
            return new JsonResult(svMaterial);
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> GetRequestAll([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiRequestBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;

            List<DailyRequestLabour> rslLabour = new List<DailyRequestLabour>();
            List<DailyRequestMaterial> rslMaterial = new List<DailyRequestMaterial>();

            if (rslRequest != null)
            {
                var mdlLabour = _getData.ApiDailyRequestLabour(rslRequest.drRefNoID);
                rslLabour = mdlLabour.Result.Value;

                var mdlMaterial = _getData.ApiDailyRequestMaterial(rslRequest.drRefNoID);
                rslMaterial = mdlMaterial.Result.Value;

            }

            viewModel.vDailyRequestLabour = rslLabour;
            viewModel.vDailyRequestMaterial = rslMaterial;
            viewModel.vDailyRequest = rslRequest;

            return new JsonResult(viewModel);

        }

        public List<DailyRequest> GetRequestFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiRequestFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
    }
}
