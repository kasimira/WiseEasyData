using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WiseEasyData.Models;

namespace WiseEasyData.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
       
    }
}
