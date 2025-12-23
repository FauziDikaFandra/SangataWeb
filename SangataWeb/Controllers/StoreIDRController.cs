using Microsoft.AspNetCore.Mvc;
using SangataWeb.Class;
using SangataWeb.Models;
using ClosedXML.Excel;
using Spire.Xls;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq.Expressions;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json.Linq;

namespace SangataWeb.Controllers
{
    public class StoreIDRController : Controller
    {
        private readonly IGetData? _getData;
        private readonly ISetData? _setData;
        private readonly IDelData? _delData;
        private readonly IHostingEnvironment _env;
        public StoreIDRController(IGetData? getData, ISetData? setData, IDelData? delData, IHostingEnvironment env)
        {
            _getData = getData;
            _setData = setData;
            _delData = delData;
            _env = env;
        }
        public IActionResult Index()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                Store _store = new Store();
                List<Store> _storeList = new List<Store>();
                mymodel.User_Login = GetUser();
                mymodel.vOneStore = GetOneStore();
                mymodel.vCCSCode = GetCCSCode();
                mymodel.vCCSCodeNew = GetCCSCodeNew();
                mymodel.vUnit = GetUnit();
                mymodel.vRack = GetRack();
                mymodel.vBin = GetBin();
                mymodel.vStoreFind = GetStoreFindLoad();
                if (mymodel.vOneStore != null)
                {
                    //mymodel.vStoreSum = GetStoreSum(mymodel.vOneStore.sStockCode);
                }
                else
                {
                    mymodel.vStoreFind = _storeList;
                    mymodel.vOneStore = _store;
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
                DailyRequest _daily = new DailyRequest();
                List<DailyRequest> _dailyList = new List<DailyRequest>();
                mymodel.User_Login = GetUser();
                mymodel.vDailyRequest = GetStoreRequest();
                mymodel.vRequestFind = GetRequestFindLoad();
                if (mymodel.vDailyRequest != null)
                {
                    mymodel.vDailyRequestMaterial = GetDailyRequestMaterial(mymodel.vDailyRequest.drRefNoID);
                }
                else
                {
                    mymodel.vDailyRequest = _daily;
                    mymodel.vRequestFind = _dailyList;
                }
                //mymodel.vJob = GetJob();
                mymodel.vForeman = GetForeman(); 
                mymodel.vStoreMan = GetStoreman();
                mymodel.vStore = GetStore();
                
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult InternalRequestOut()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vInternalReqOUT = GetOneRequest();
                if (mymodel.vInternalReqOUT != null)
                {
                    mymodel.vInternalReqSubOUT = GetDailyIRequestSubOut(mymodel.vInternalReqOUT.iroID);
                }
                mymodel.vCurrency = GetCurrency();
                mymodel.vForeman = GetForeman();
                mymodel.vPreparedBy = GetPreparedBy();
                mymodel.vRecBy = GetRecBy();
                mymodel.vProject = GetProject();
                mymodel.vStore = GetStore();
                mymodel.vIRequestOutFind = GetIRequestOutFindLoad();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult InternalRequestIn()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vInternalReq = GetOneRequestIn();
                if (mymodel.vInternalReq != null)
                {
                    mymodel.vInternalReqSub = GetDailyIRequestSubIn(mymodel.vInternalReq.irID);
                }
                mymodel.vCurrency = GetCurrency();
                mymodel.vForeman = GetForeman();
                mymodel.vSupplierList = GetSupplierList();
                mymodel.vRecBy = GetRecBy();
                mymodel.vProject = GetProject();
                mymodel.vStore = GetStore();
                mymodel.vIRequestInFind = GetIRequestFindLoad();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult StockAdjustment()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vAdjustment = GetAdjustment();
                if (mymodel.vAdjustment != null)
                {
                    mymodel.vAdjustmentSub = GetAdjustmentSub(mymodel.vAdjustment.adNo);
                }
                mymodel.vStoreMan = GetStoreman();
                mymodel.vForeman = GetForeman();
                mymodel.vStore = GetStore();
                mymodel.vAdjustmentFind = GetAdjustmentFindLoad();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult Shipping()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vShipping = GetShipping();
                if (mymodel.vShipping != null)
                {
                    mymodel.vShippingSub = GetShippingSub(mymodel.vShipping.siID);
                    mymodel.vStoreIssueSub = GetStoreIssueSub(mymodel.vShipping.siID);
                }
                mymodel.vFreightType = GetFreightType();
                mymodel.vShippingPONo = GetShippingPONo();
                mymodel.vStoreMan = GetStoreman();
                mymodel.vStore = GetStore();
                mymodel.vOrReqNo = GetOrReqNo();
                mymodel.vCurrency = GetCurrency();
                mymodel.vShippingFind = GetShippingFindLoad();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Docket()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vDocket = GetDocket();
                if (mymodel.vDocket != null)
                {
                    mymodel.vDocketSub = GetDocketSub(mymodel.vDocket.ddID);
                }
                mymodel.vFreightType = GetFreightType();
                mymodel.vDocketPONo = GetShippingPONo();
                mymodel.vStoreMan = GetStoreman();
                mymodel.vStore = GetStore();
                mymodel.vOrReqNo = GetOrReqNo();
                mymodel.vCurrency = GetCurrency();
                mymodel.vDocketFind = GetDocketFindLoad();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult BackCharge()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vBackCharge = GetOneBackCharge();
                if (mymodel.vBackCharge != null)
                {
                    mymodel.vBackChargeSub = GetBackChargeSub(mymodel.vBackCharge.bKPCReqNo);
                }
                mymodel.vCurrency = GetCurrency();
                //mymodel.vForeman = GetForeman();
                mymodel.vStoreMan = GetStoreman();
                mymodel.vProject = GetProject();
                mymodel.vStore = GetStore();
                mymodel.vBackChargeFind = GetBackChargeFindLoad();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult StockDetail()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vStockDetail = GetOneStore();
                if (mymodel.vStockDetail != null)
                {
                    mymodel.vStockIn = GetStockIn(mymodel.vStockDetail.sStockCode);
                    mymodel.vStockOut = GetStockOut(mymodel.vStockDetail.sStockCode);
                }
                mymodel.vStoreFind = GetStoreFindLoad();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult ListMaterialOut()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vListMaterialOutSummary = GetListMaterialOutSummary();
                mymodel.vListMaterialOut = GetListMaterialOut();
                mymodel.vJob = GetJob();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult Reports()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = GetUser();
                mymodel.vReportMaster = GetReportMaster();
                return View(mymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public Models.Users GetUser()
        {

            Models.Users header = new Models.Users();
            header.UserName = HttpContext.Session.GetString("UserName");
            return header;
        }
        public Store GetOneStore()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiOneStore();
            var query = mdlAssetMain.Result.Value;
            return query;
        }
        public List<CCSCode> GetCCSCode()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiCCSCode().Result;
            var query = mdlRequest.Value;
            return query;
        }
        public List<CCSCodeNew> GetCCSCodeNew()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiCCSCodeNew().Result;
            var query = mdlRequest.Value;
            return query;
        }
        public List<Unit> GetUnit()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiUnit().Result;
            var query = mdlRequest.Value;
            return query;
        }
        public List<Rack> GetRack()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiRack().Result;
            var query = mdlRequest.Value;
            return query;
        }
        public List<Bin> GetBin()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiBin().Result;
            var query = mdlRequest.Value;
            return query;
        }
        public StoreSum GetStoreSum(string sStockCode)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiStoreSum(sStockCode).Result;
            var query = mdlRequest.Value;
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> GetExistingMain([FromBody] Store main)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiStoreChecking(main).Result;
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateStore(main, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateMain([FromBody] Store main)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svMain = new ResultJson();
            svMain = _setData.ApiCreateStore(main, "Update").Result;
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetMainAll([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlMain = _getData.ApiStoreBy(actionModel);
            var rslMain = mdlMain.Result.Value;
            viewModel.vOneStore = rslMain;
            if (viewModel.vOneStore != null)
            {
                //viewModel.vStoreSum = GetStoreSum(viewModel.vOneStore.sStockCode);
            }
            return new JsonResult(viewModel);

        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetLastEntry([FromBody] ActionModelData actionModel)
        {
            
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            StoreSum viewModel = new StoreSum();
            viewModel = GetStoreSum(actionModel.Typ);
            return new JsonResult(viewModel);

        }
        public List<Store> GetStoreFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiStoreFind().Result;
            var query = mdlMain.Value;
            return query;
        }
        public DailyRequest GetStoreRequest()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDailyRequest = _getData.ApiStoreRequest();
            var query = mdlDailyRequest.Result.Value;
            return query;
        }
        public List<vWINo> GetJob()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiJob(0).Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<DailyRequestMaterial> GetDailyRequestMaterial(string drRefNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDailyRequestMaterial(drRefNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<Foreman> GetForeman()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiForeman().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<StoreMan> GetStoreman()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiStoreMan().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<DailyRequest> GetRequestFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            //var mdlRequest = _getData.ApiRequestFind().Result;
            //var query = mdlRequest.Value;
            var query = new List<DailyRequest>();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteMain([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            switch (actionModel.Typ)
            {
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
        public async Task<ActionResult<List<string>>> SaveMaterial([FromBody] DailyRequestMaterial daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateMaterialStore(daily, typ).Result;
            return new JsonResult(svMaterial);
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
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateRequest([FromBody] DailyRequest daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateRequest(daily, "Update").Result;
            return svDaily;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetRequestAll([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiRequestStoreBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;

            List<DailyRequestMaterial> rslMaterial = new List<DailyRequestMaterial>();

            if (rslRequest != null)
            {

                var mdlMaterial = _getData.ApiDailyRequestMaterial(rslRequest.drRefNoID);
                rslMaterial = mdlMaterial.Result.Value;

            }

            viewModel.vDailyRequestMaterial = rslMaterial;
            viewModel.vDailyRequest = rslRequest;

            return new JsonResult(viewModel);

        }
        public List<Store> GetStore()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlStore = _getData.ApiStore();
            var query = mdlStore.Result.Value.ToList();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<vWINo>>> GetJobNo([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlStore = _getData.ApiJob(actionModel.Id);
            var query = mdlStore.Result.Value.ToList();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<vWINo>> GetJobNoOne([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlStore = _getData.ApiJobOne(actionModel.Id);
            var query = mdlStore.Result.Value;
            return query;
        }
        public List<PreparedBy> GetPreparedBy()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiPreparedBy().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<SupplierList> GetSupplierList()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiSupplierList().Result;
            var query = mdlMain.Value;
            return query;
        }
        public InternalReqOUT GetOneRequest()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiOneRequest();
            var query = mdlAssetMain.Result.Value;
            return query;
        }
        public InternalReq GetOneRequestIn()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiOneRequestIn();
            var query = mdlAssetMain.Result.Value;
            return query;
        }
        public List<Currency> GetCurrency()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiCurrency().Result;
            var query = mdlRequest.Value;
            return query;
        }
        public List<RecBy> GetRecBy()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiRecBy().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<Project> GetProject()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlProject = _getData.ApiProject();
            var query = mdlProject.Result.Value.ToList();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> GetExistingIRequestOut([FromBody] InternalReqOUT daily)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiAssetIRequestOutChecking(daily).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateIRequestOut(daily, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> GetExistingIRequestIn([FromBody] InternalReq daily)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiAssetIRequestInChecking(daily).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateIRequestIn(daily, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateIRequestOut([FromBody] InternalReqOUT daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateIRequestOut(daily, "Update").Result;
            return svDaily;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateIRequestIn([FromBody] InternalReq daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateIRequestIn(daily, "Update").Result;
            return svDaily;
        }
        public List<InternalReqSubOUT> GetDailyIRequestSubOut(string irosNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDailyIRequestSubOut(irosNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<InternalReqSub> GetDailyIRequestSubIn(string irsNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDailyIRequestSubIn(irsNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<InternalReqOUT> GetIRequestOutFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiIRequestOutFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
        public List<InternalReq> GetIRequestFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiIRequestInFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteIRequestOut([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            svAction = _delData.ApiDeleteIRequestSubOut(actionModel.Id).Result;
            return new JsonResult(svAction);
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteIRequestIn([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            svAction = _delData.ApiDeleteIRequestSubIn(actionModel.Id).Result;
            return new JsonResult(svAction);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveIRequestSubOut([FromBody] InternalReqSubOUT daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateIRequestSubOut(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveIRequestSubIn([FromBody] InternalReqSub daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateIRequestSubIn(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetIRequestOut([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiIRequestOutBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;
            viewModel.vInternalReqOUT = rslRequest;
            if (viewModel.vInternalReqOUT != null)
            {
                var mdlInternal = _getData.ApiDailyIRequestSubOut(viewModel.vInternalReqOUT.iroID);
                var rslInternal = mdlInternal.Result.Value;
                viewModel.vInternalReqSubOUT = rslInternal;
            }
            return new JsonResult(viewModel);

        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetIRequestIn([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiIRequestInBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;
            viewModel.vInternalReq = rslRequest;
            if (viewModel.vInternalReq != null)
            {
                var mdlInternal = _getData.ApiDailyIRequestSubIn(viewModel.vInternalReq.irID);
                var rslInternal = mdlInternal.Result.Value;
                viewModel.vInternalReqSub = rslInternal;
            }
            return new JsonResult(viewModel);

        }
        public List<ListMaterialOut> GetListMaterialOut()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlProject = _getData.ApiListMaterialOut();
            var query = mdlProject.Result.Value.ToList();
            return query;
        }
        public List<ListMaterialOutSummary> GetListMaterialOutSummary()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlProject = _getData.ApiListMaterialOutSummary();
            var query = mdlProject.Result.Value.ToList();
            return query;
        }
        public IActionResult PrintListMaterial(ActionModelData actionModel)
        {
            using (var workbook = new XLWorkbook())
            {
                actionModel.Idstr = ((actionModel.Idstr == null) ? "" : actionModel.Idstr);
                List <ListMaterialOutSummary> _summary = new List<ListMaterialOutSummary>();
                List<ListMaterialOut> _detail = new List<ListMaterialOut>();
                var worksheet = workbook.Worksheets.Add("List Stock Out " + actionModel.Typ);
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "List Stock Out " + actionModel.Typ;
                var range = worksheet.Range("A1:G1");
                range.Merge().Style.Font.SetBold().Font.FontSize = 16;
                currentRow += 2;
                if (actionModel.Typ == "Summary")
                {
                    Debug.Assert(_getData != null, nameof(_getData) + " != null");
                    var mdlProject = _getData.ApiListMaterialOutSummary(actionModel.Idstr);
                    _summary = mdlProject.Result.Value.ToList();
                    worksheet.Cell(currentRow, 1).Value = "WO ID";
                    worksheet.Cell(currentRow, 2).Value = "Ref No";
                    worksheet.Cell(currentRow, 3).Value = "DR Date";
                    worksheet.Cell(currentRow, 4).Value = "Stock Code";
                    worksheet.Cell(currentRow, 5).Value = "Description";
                    worksheet.Cell(currentRow, 6).Value = "Qty Used";
                    worksheet.Cell(currentRow, 7).Value = "Foreman";
                    worksheet.Column("F").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    foreach (ListMaterialOutSummary _record in _summary)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = _record.JobNo;
                        worksheet.Cell(currentRow, 2).Value = _record.RefNo;
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDateTime(_record.Date).ToString("dd-MMM-yyyy");
                        worksheet.Cell(currentRow, 4).Value = _record.StockCode;
                        worksheet.Cell(currentRow, 5).Value = _record.sDescription;
                        worksheet.Cell(currentRow, 6).Value = _record.QtyUsed;
                        worksheet.Cell(currentRow, 7).Value = _record.Foreman;

                    }
                }
                else if (actionModel.Typ == "Detail")
                {
                    Debug.Assert(_getData != null, nameof(_getData) + " != null");
                    var mdlProject = _getData.ApiListMaterialOut(actionModel.Idstr);
                    _detail = mdlProject.Result.Value.ToList();
                    worksheet.Cell(currentRow, 1).Value = "WO ID";
                    worksheet.Cell(currentRow, 2).Value = "Stock Code";
                    worksheet.Cell(currentRow, 3).Value = "Description";
                    worksheet.Cell(currentRow, 4).Value = "Qty Used";
                    worksheet.Cell(currentRow, 5).Value = "Last Foreman";
                    worksheet.Cell(currentRow, 6).Value = "Last DRNo";
                    worksheet.Cell(currentRow, 7).Value = "Last DRDate";
                    worksheet.Column("D").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    foreach (ListMaterialOut _record in _detail)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = _record.JobNo;
                        worksheet.Cell(currentRow, 2).Value = _record.StockCode;
                        worksheet.Cell(currentRow, 3).Value = _record.sDescription;
                        worksheet.Cell(currentRow, 4).Value = _record.QtyUsed;
                        worksheet.Cell(currentRow, 5).Value = _record.Foreman;
                        worksheet.Cell(currentRow, 6).Value = _record.RefNo;
                        worksheet.Cell(currentRow, 7).Value = Convert.ToDateTime(_record.Date).ToString("dd-MMM-yyyy");
                    }
                }

                //worksheet.Columns("A", "L").AdjustToContents();
                worksheet.Range("A3:G3").Style.Font.Bold = true;
                worksheet.Range("A3:G3").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                
                worksheet.Column("B").Width = 20;
                worksheet.Column("C").Width = 20;
                worksheet.Column("D").Width = 20;
                worksheet.Column("E").Width = 20;
                worksheet.Column("F").Width = 20;
                
                worksheet.Column("G").Width = 20;
                //worksheet.Range("A1:AB").Style.Fill.BackgroundColor = XLColor.Orange;
                string fileName = "StockOut_" + DateTime.Now.ToString("yyyyMMddhhMMss");
                string workingDirectory = _env.ContentRootPath;
                DirectoryInfo di = new DirectoryInfo(workingDirectory);
                FileInfo[] files = di.GetFiles("*.pdf").Where(p => p.Extension.ToLowerInvariant() == ".pdf").ToArray();
                foreach (FileInfo file in files)
                {
                    file.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(file.FullName);
                }
                string workbookPath = workingDirectory + @"\" + fileName;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(workbookPath + ".xlsx");
                    Spire.Xls.Workbook workbookpdf = new Spire.Xls.Workbook();
                    workbookpdf.LoadFromFile(workbookPath + ".xlsx");
                    workbookpdf.ConverterSetting.SheetFitToPage = true;
                    workbookpdf.SaveToFile(workbookPath + ".pdf", FileFormat.PDF);
                    System.IO.File.Delete(workbookPath + ".xlsx");

                    //return File(workbookPath + ".pdf", "application/pdf");
                }
                var stream2 = new FileStream(workbookPath + ".pdf", FileMode.Open);
                return File(stream2, "application/pdf", fileName + ".pdf");
            }
        }
        public Adjustment GetAdjustment()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiAdjustment();
            var query = mdlAssetMain.Result.Value;
            return query;
        }
        public List<AdjustmentSub> GetAdjustmentSub(string adNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiAdjustmentSub(adNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<Adjustment> GetAdjustmentFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiAdjustmentFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
        public async Task<ActionResult<ResultJson>> GetExistingAdjustment([FromBody] Adjustment daily)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiAdjustmentChecking(daily).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateAdjustment(daily, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateAdjustment([FromBody] Adjustment daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateAdjustment(daily, "Update").Result;
            return svDaily;
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteAdjustmentSub([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            svAction = _delData.ApiDeleteAdjustmentSub(actionModel.Id).Result;
            return new JsonResult(svAction);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveAdjustmentSub([FromBody] AdjustmentSub daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateAdjustmentSub(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetAdjustmentBy([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiAdjustmentBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;
            viewModel.vAdjustment = rslRequest;
            if (viewModel.vAdjustment != null)
            {
                var mdlInternal = _getData.ApiAdjustmentSub(viewModel.vAdjustment.adNo);
                var rslInternal = mdlInternal.Result.Value;
                viewModel.vAdjustmentSub = rslInternal;
            }
            return new JsonResult(viewModel);

        }
        public Shipping GetShipping()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiShipping();
            var query = mdlAssetMain.Result.Value;
            return query;
        }
        public List<vShippingSub> GetShippingSub(string sisNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiShippingSub(sisNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<FreightType> GetFreightType()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiFreightType().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<vShippingPONo> GetShippingPONo()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiShippingPONo().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<vOrReqNo> GetOrReqNo()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiOrReqNo().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<Shipping> GetShippingFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiShippingFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
        public List<vStoreIssueSub> GetStoreIssueSub(string sisNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiStoreIssueSub(sisNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public async Task<ActionResult<ResultJson>> GetExistingShipping([FromBody] Shipping daily)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiShippingChecking(daily).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateShipping(daily, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateShipping([FromBody] Shipping daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateShipping(daily, "Update").Result;
            return svDaily;
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteShippingSub([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            if (actionModel.Typ == "Material")
            {
                svAction = _delData.ApiDeleteShippingSub(actionModel.Id).Result;
            }
            else
            {
                svAction = _delData.ApiDeleteStoreIssueSub(actionModel.Id).Result;
            }
            return new JsonResult(svAction);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveShippingSub([FromBody] ShippingSub daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateShippingSub(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveStoreIssueSub([FromBody] StoreIssueSub daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiStoreIssueSub(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetShippingBy([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiShippingBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;
            viewModel.vShipping = rslRequest;
            if (viewModel.vShipping != null)
            {
                var mdlInternal = _getData.ApiShippingSub(viewModel.vShipping.siID);
                var rslInternal = mdlInternal.Result.Value;
                viewModel.vShippingSub = rslInternal;
                viewModel.vStoreIssueSub = GetStoreIssueSub(viewModel.vShipping.siID);
            }
            return new JsonResult(viewModel);

        }
        public Docket GetDocket()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiDocket();
            var query = mdlAssetMain.Result.Value;
            return query;
        }
        public List<vDocketSub> GetDocketSub(string ddNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDocketSub(ddNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<Docket> GetDocketFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiDocketFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
        public async Task<ActionResult<ResultJson>> GetExistingDocket([FromBody] Docket daily)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiDocketChecking(daily).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateDocket(daily, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateDocket([FromBody] Docket daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateDocket(daily, "Update").Result;
            return svDaily;
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteDockerSub([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            if (actionModel.Typ == "Material")
            {
                svAction = _delData.ApiDeleteShippingSub(actionModel.Id).Result;
            }
            else
            {
                svAction = _delData.ApiDeleteStoreIssueSub(actionModel.Id).Result;
            }
            return new JsonResult(svAction);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveDocketSub([FromBody] DocketSub daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateDocketSub(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetDocketBy([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiDocketBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;
            viewModel.vDocket = rslRequest;
            if (viewModel.vDocket != null)
            {
                var mdlInternal = _getData.ApiDocketSub(viewModel.vDocket.ddID);
                var rslInternal = mdlInternal.Result.Value;
                viewModel.vDocketSub = rslInternal;
            }
            return new JsonResult(viewModel);

        }
        public BackCharge GetOneBackCharge()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlAssetMain = _getData.ApiBackCharge();
            var query = mdlAssetMain.Result.Value;
            return query;
        }
        public List<BackChargeSub> GetBackChargeSub(int? bsKPCReqNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiBackChargeSub(bsKPCReqNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<BackCharge> GetBackChargeFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiBackChargeFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> GetExistingBackCharge([FromBody] BackCharge daily)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            //ActionResult? svMain = null;
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiBackChargeChecking(daily).Result;
            //var Value = JsonConvert.SerializeObject(svMain);
            //ResultJson myDeserializedClass = JsonConvert.DeserializeObject<ResultJson>(Value);
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateBackCharge(daily, "Create").Result;
            }
            return svMain;
        }
        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateBackCharge([FromBody] BackCharge daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svDaily = new ResultJson();
            svDaily = _setData.ApiCreateBackCharge(daily, "Update").Result;
            return svDaily;
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteBackCharge([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            svAction = _delData.ApiDeleteBackChargeSub(actionModel.Id).Result;
            return new JsonResult(svAction);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveBackChargeSub([FromBody] BackChargeSub daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateBackChargeSub(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetBackCharge([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiBackChargeBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;
            viewModel.vBackCharge = rslRequest;
            if (viewModel.vBackCharge != null)
            {
                var mdlInternal = _getData.ApiBackChargeSub(viewModel.vBackCharge.bKPCReqNo);
                var rslInternal = mdlInternal.Result.Value;
                viewModel.vBackChargeSub = rslInternal;
            }
            return new JsonResult(viewModel);

        }
        public List<StockIn> GetStockIn(string code)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiStockIn(code);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<StockOut> GetStockOut(string code)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiStockOut(code);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetStockDetailBy([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiStockDetailBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;
            viewModel.vStockDetail = rslRequest;
            if (viewModel.vStockDetail != null)
            {
                viewModel.vStockIn = GetStockIn(viewModel.vStockDetail.sStockCode);
                viewModel.vStockOut = GetStockOut(viewModel.vStockDetail.sStockCode);
            }
            return new JsonResult(viewModel);

        }
        public List<ReportMaster> GetReportMaster()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiReportMaster();
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetDropDownReport([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            List<DropDownReport> dropDownReport = new List<DropDownReport>();
            if (actionModel.Typ.Contains('_'))
            {
                string[] authorsList = actionModel.Typ.Split("_");
                foreach (string author in authorsList)
                {
                    dropDownReport.Add(new DropDownReport
                    {
                        Code = "",
                        Description = author,
                    });
                }
            }
            else
            {
                var mdlRequest = _getData.ApiDropDownReport(actionModel.Typ, actionModel.Idstr, actionModel.Idstr2);
                dropDownReport = mdlRequest.Result.Value;

            }

            return new JsonResult(dropDownReport);

        }
        [HttpPost]
        public async Task<ActionResult<string>> GetListReport([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ReportMaster reportMaster = new ReportMaster();
            var mdlRequest = _getData.ApiOneReportMaster(actionModel.Id);
            reportMaster = mdlRequest.Result.Value;
            string rQuery = reportMaster.rQuery + " " + actionModel.Id + ",'" + actionModel.Typ + "','" + actionModel.Idstr + "','" + actionModel.Idstr2 + "','" + actionModel.Idstr3 + "'";
            var mdlRequest2 = _getData.ApiListReport(rQuery);
            var result = mdlRequest2.Result;
            return new JsonResult(result);

        }
    }
}
