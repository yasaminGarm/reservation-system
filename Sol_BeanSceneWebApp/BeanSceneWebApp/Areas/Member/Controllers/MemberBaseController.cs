using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BeanSceneWebApp.Areas.Member.Controllers
{
    [Area("Member"), Authorize(Roles = "Member")]
    public class MemberBaseController : Controller
    {
       
       
    }
}
