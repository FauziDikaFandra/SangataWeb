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

namespace SangataWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IGetData? _getData;
        public HomeController(IGetData? getData)
        {
            _getData = getData;
        }

        public IActionResult Index()
        {
            var eRr = new ErrorViewModel() { RequestId = "" };
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                mymodel.User_Login = new Users { UserName = HttpContext.Session.GetString("UserName")};
                return View(mymodel);
            }
            else
            {
                return View("Login", eRr);
            }
        }

        public IActionResult Maintenance()
        {
            var eRr = new ErrorViewModel() { RequestId = "Maintenance!!" };
            return View(eRr);
        }

        [HttpPost]
        public ActionResult getLogin(Users data)
        {


            Debug.Assert(_getData != null, nameof(_getData) + " != null");
            Task<ActionResult<Users>> mdlLogin;
            mdlLogin = _getData.ApiLogin()!;

            string query = "";
            try
            {
                query = mdlLogin.Result.Value.UserName.ToString();
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("UserName", ex.Message.ToString());
                return RedirectToAction("Error", "Home");
            }
            finally
            {

            }
            if (query.Any())
            {
                HttpContext.Session.SetString("UserName", ((!string.IsNullOrEmpty(data.UserName)) ? data.UserName : ""));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var eRr = new ErrorViewModel() { RequestId = "Invalid Password!!" };
                return View("Login", eRr);
            }

        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Error()
        {
            ErrorViewModel eRr = new ErrorViewModel();
            ViewModel mymodel = new ViewModel();
            var ssUserName = ((!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))) ? HttpContext.Session.GetString("UserName") as string : "");
            if (ssUserName != "")
            {
                eRr = new ErrorViewModel { RequestId ="0",ErrorMessage = ssUserName };
                return View(eRr);
            }
            else
            {
                return View("Login", eRr);
            }
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
