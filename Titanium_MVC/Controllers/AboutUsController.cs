using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Titanium_MVC.Models;
namespace Titanium_MVC.Controllers
{
    public class AboutUsController : Controller
    {
        public ViewResult About()
        {
            return View();
        }
    }
       
}
