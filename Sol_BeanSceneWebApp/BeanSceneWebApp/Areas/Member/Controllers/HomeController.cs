using Microsoft.AspNetCore.Mvc;

namespace BeanSceneWebApp.Areas.Member.Controllers
{
    public class HomeController : MemberBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReservationConfirmation()
        {

            return View("ReservationConfirmation");
        }
    }
}
