using Microsoft.AspNetCore.Mvc;
using Domain;
using Application;

namespace Titanium_MVC.Controllers
{
    public class LiveSearchController : Controller
    {
        private readonly IPropertyService<Property> _propertyService;

        public LiveSearchController(IPropertyService<Property> propertyService)
        {
            _propertyService = propertyService;
        }

        public IActionResult LiveSearch()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchProperty(string inputData)
        {
            var results = _propertyService.searchProperty(inputData);
            return PartialView("_HotOffers", results);
        }
    }
}
