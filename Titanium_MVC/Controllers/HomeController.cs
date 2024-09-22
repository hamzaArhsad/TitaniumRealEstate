using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Domain;
using Application;
namespace Titanium_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService<Property> _propertyService;

        public HomeController(IPropertyService<Property> propertyService)
        {
             _propertyService = propertyService;
        }

        //[Authorize]
        public ViewResult Index()
        {
            // Set the cookie options
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };
            string data = string.Empty;

            // Get the current user's ID and name
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userName = HttpContext.User.Identity.Name;

            // Greet the user based on whether it's their first visit
            if (HttpContext.Request.Cookies.ContainsKey("first_request"))
            {
                data = "WELCOME BACK, " + userName;
            }
            else
            {
                data = "WELCOME " + userName;
                HttpContext.Response.Cookies.Append("first_request", DateTime.Now.ToString(), cookieOptions);
            }

            // Set the view data with the greeting message
            ViewData["Greeting"] = data;

            // Return the view with the properties retrieved from the service
            return View(_propertyService.GetAll());
        }

        public ViewResult About()
        {
            return View();
        }

        public ViewResult Contact()
        {
            return View();
        }

        public ViewResult Team()
        {
            return View();
        }

        public ViewResult Project()
        {
            return View();
        }
    }
}
