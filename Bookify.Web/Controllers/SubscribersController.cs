using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles =AppRoles.Reception)]
    public class SubscribersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Create()
        //{


        //}



    }
}
