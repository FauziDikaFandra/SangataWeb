using Microsoft.AspNetCore.Mvc;
using SangataWeb.Class;
using SangataWeb.Models;
using System.Diagnostics;

namespace SangataWeb.Controllers
{
    public class MinorSystemController : Controller
    {
        private readonly IGetData? _getData;
        private readonly ISetData? _setData;
        private readonly IDelData? _delData;

        public MinorSystemController(IGetData? getData, ISetData? setData, IDelData? delData)
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
                mymodel.vProject = GetProject();
                mymodel.vTechnician = GetTechnician();
                mymodel.vClientRef = GetClientRef();
                mymodel.vLocation = GetLocation();
                mymodel.vCustomer = GetCustomer();
                mymodel.vCompany = GetCustomer();
                mymodel.vOrderType = GetOrderType();
                mymodel.vCompleteBy = GetCompleteBy();
                mymodel.vOneRepair = GetOneRepair();
                mymodel.vMainFind = GetMainFindLoad();
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
                mymodel.vRepairRequest = GetRepairRequest();
                if (mymodel.vRepairRequest != null)
                {
                    mymodel.vRepairRequestLabour = GetRepairRequestLabour(mymodel.vRepairRequest.drRefNoID);
                    mymodel.vRepairRequestMaterial = GetRepairRequestMaterial(mymodel.vRepairRequest.drRefNoID);
                }
                mymodel.vStore = GetStore();
                mymodel.vProject = GetProject();
                mymodel.vTechnician = GetTechnician();
                mymodel.vTechnicianRate = GetTechnicianRate();
                mymodel.vRepairFind = GetRepairFindLoad();

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
        public List<Project> GetProject()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlProject = _getData.ApiProject();
            var query = mdlProject.Result.Value.ToList();
            return query;
        }
        public List<Technician> GetTechnician()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlTechnicianDD = _getData.ApiTechnician();
            var query = mdlTechnicianDD.Result.Value.ToList();
            return query;
        }
        public List<ClientRef> GetClientRef()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlClientRef = _getData.ApiClientRef();
            var query = mdlClientRef.Result.Value.ToList();
            return query;
        }
        public List<Location> GetLocation()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlLocation = _getData.ApiLocation();
            var location = mdlLocation.Result.Value.ToList();
            return location;
        }
        public List<Customer> GetCustomer()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlCustomer = _getData.ApiCustomer();
            var customer = mdlCustomer.Result.Value.ToList();
            return customer;
        }
        public List<OrderType> GetOrderType()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlOrderType = _getData.ApiOrderType();
            var order = mdlOrderType.Result.Value.ToList();
            return order;
        }
        public List<CompleteBy> GetCompleteBy()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlCompleteBy = _getData.ApiCompleteBy();
            var complete = mdlCompleteBy.Result.Value.ToList();
            return complete;
        }

        public Repair GetOneRepair()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRepair = _getData.ApiOneRepair();
            var query = mdlRepair.Result.Value;
            return query;
        }

        [HttpPost]
        public async Task<ActionResult<ResultJson>> GetExistingMain([FromBody] Repair main)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svMain = new ResultJson();
            svMain = _getData.ApiMainChecking(main).Result;
            if (!svMain.success)
            {
                svMain = _setData.ApiCreateMain(main, "Create").Result;
            }
            return svMain;
        }

        [HttpPost]
        public async Task<ActionResult<ResultJson>> UpdateMain([FromBody] Repair main)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ResultJson svMain = new ResultJson();
            svMain = _setData.ApiCreateMain(main, "Update").Result;
            return svMain;
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> GetMainAll([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlMain = _getData.ApiMainBy(actionModel);
            var rslMain = mdlMain.Result.Value;
            viewModel.vOneRepair = rslMain;
            return new JsonResult(viewModel);

        }

        public List<Repair> GetMainFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiMainFind().Result;
            var query = mdlMain.Value;
            return query;
        }

        public Repair GetRepairRequest()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRepairRequest = _getData.ApiRepairRequest();
            var query = mdlRepairRequest.Result.Value;
            return query;
        }
        public List<DailyRequestLabour> GetRepairRequestLabour(string drRefNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDailyRequestLabour(drRefNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }

        public List<DailyRequestMaterial> GetRepairRequestMaterial(string drRefNo)
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlDaily = _getData.ApiDailyRequestMaterial(drRefNo);
            var query = mdlDaily.Result.Value.ToList();
            return query;
        }
        public List<Store> GetStore()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlStore = _getData.ApiStore();
            var query = mdlStore.Result.Value.ToList();
            return query;
        }
        public List<Technician> GetTechnicianRate()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlTechnician = _getData.ApiTechnicianRate();
            var query = mdlTechnician.Result.Value.ToList();
            return query;
        }
        public List<Repair> GetRepairFindLoad()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiRepairFind().Result;
            var query = mdlRequest.Value;
            //var query = new List<DailyRequest>();
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> GetRequestAll([FromBody] ActionModelData actionModel)
        {
            actionModel.Typ = actionModel.Id == 0 ? "end" : actionModel.Typ;
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            ViewModel viewModel = new ViewModel();
            var mdlRequest = _getData.ApiMainBy(actionModel);
            var rslRequest = mdlRequest.Result.Value;

            List<DailyRequestLabour> rslLabour = new List<DailyRequestLabour>();
            List<DailyRequestMaterial> rslMaterial = new List<DailyRequestMaterial>();

            if (rslRequest != null)
            {
                var mdlLabour = _getData.ApiDailyRequestLabour(rslRequest.rJobOrderGroup);
                rslLabour = mdlLabour.Result.Value;

                var mdlMaterial = _getData.ApiDailyRequestMaterial(rslRequest.rJobOrderGroup);
                rslMaterial = mdlMaterial.Result.Value;

            }

            viewModel.vRepairRequestLabour = rslLabour;
            viewModel.vRepairRequestMaterial = rslMaterial;
            viewModel.vRepairRequest = rslRequest;

            return new JsonResult(viewModel);

        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveLabour([FromBody] DailyRequestLabour daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svLabour = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svLabour = _setData.ApiCreateLabourMinor(daily, typ).Result;
            return new JsonResult(svLabour);
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveMaterial([FromBody] DailyRequestMaterial daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svMaterial = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svMaterial = _setData.ApiCreateMaterialMinor(daily, typ).Result;
            return new JsonResult(svMaterial);
        }
    }
}
