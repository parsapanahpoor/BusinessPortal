using Microsoft.AspNetCore.Mvc;

namespace BusinessPortal.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Admin Dashboard

        public IActionResult Index()
        {
            return View();
        }

        #endregion

    }
}
