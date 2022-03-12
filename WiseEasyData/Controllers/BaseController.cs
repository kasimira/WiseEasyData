using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WiseEasyData.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
       
    }
}
