using Microsoft.AspNetCore.Mvc;
using SangataWeb.Models;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Text;
using SangataWeb.Class;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Expressions;

namespace SangataWeb.Controllers
{
    public class MasterController : Controller
    {

        private readonly IGetData? _getData;
        private readonly ISetData? _setData;
        private readonly IDelData? _delData;
        public MasterController(IGetData? getData, ISetData? setData, IDelData? delData)
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
                mymodel.User_Login = new Users { UserName = HttpContext.Session.GetString("UserName")};
                mymodel.vForeman = GetForeman();
                mymodel.vStoreMan = GetStoreman();
                mymodel.vUnit = GetUnit();
                return View(mymodel);
            }
            else
            {
                return View("Login", eRr);
            }
        }
        public IActionResult CCL()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = new Users { UserName = HttpContext.Session.GetString("UserName") };
                mymodel.vCCSCode = GetCCSCode();
                mymodel.vCustomer = GetCustomer();
                mymodel.vLocation = GetLocation();
                return View(mymodel);
            }
            else
            {
                return View("Login", eRr);
            }
        }
        public IActionResult Supplier()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = new Users { UserName = HttpContext.Session.GetString("UserName") };
                mymodel.vSupplierList = GetSupplierList();
                return View(mymodel);
            }
            else
            {
                return View("Login", eRr);
            }
        }
        public List<Foreman> GetForeman()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiForeman().Result;
            var query = mdlMain.Value;
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveForeman([FromBody] Foreman daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svForeman = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svForeman = _setData.ApiCreateForeman(daily, typ).Result;
            return new JsonResult(svForeman);
        }
        public List<StoreMan> GetStoreman()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiStoreMan().Result;
            var query = mdlMain.Value;
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveStoreman([FromBody] StoreMan daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svStoreman = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svStoreman = _setData.ApiCreateStoreman(daily, typ).Result;
            return new JsonResult(svStoreman);
        }
        public List<Unit> GetUnit()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiUnit().Result;
            var query = mdlMain.Value;
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveUnit([FromBody] Unit daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svStoreman = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svStoreman = _setData.ApiCreateUnit(daily, typ).Result;
            return new JsonResult(svStoreman);
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveSupplier([FromBody] SupplierList daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svSupplier = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svSupplier = _setData.ApiCreateSupplier(daily, typ).Result;
            return new JsonResult(svSupplier);
        }
        [HttpPost]
        public async Task<ActionResult<string>> DeleteItem([FromBody] ActionModelData actionModel)
        {
            Debug.Assert(_delData != null, nameof(_delData) + " != null");
            ActionResult? svAction = null;
            svAction = _delData.ApiDeleteMaster(actionModel).Result;
            return new JsonResult(svAction);
        }
        public List<SupplierList> GetSupplierList()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlMain = _getData.ApiSupplierList().Result;
            var query = mdlMain.Value;
            return query;
        }
        public List<CCSCode> GetCCSCode()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlRequest = _getData.ApiCCSCode().Result;
            var query = mdlRequest.Value;
            return query;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveCCSCode([FromBody] CCSCode daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svCCSCode = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svCCSCode = _setData.ApiCreateCCSCode(daily, typ).Result;
            return new JsonResult(svCCSCode);
        }
        public List<Customer> GetCustomer()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlCustomer = _getData.ApiCustomer();
            var customer = mdlCustomer.Result.Value.ToList();
            return customer;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveCustomer([FromBody] Customer daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svCustomer = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svCustomer = _setData.ApiCreateCustomer(daily, typ).Result;
            return new JsonResult(svCustomer);
        }
        public List<Location> GetLocation()
        {
            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            var mdlLocation = _getData.ApiLocation();
            var location = mdlLocation.Result.Value.ToList();
            return location;
        }
        [HttpPost]
        public async Task<ActionResult<List<string>>> SaveLocation([FromBody] Location daily)
        {
            Debug.Assert(_setData != null, nameof(_setData) + " != null");
            ActionResult? svLocation = null;
            string typ = ((daily.Id == 0) ? "Create" : "Update");
            svLocation = _setData.ApiCreateLocation(daily, typ).Result;
            return new JsonResult(svLocation);
        }
    }
}
