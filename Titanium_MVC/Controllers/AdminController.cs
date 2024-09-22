using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Domain;
using Application;
using Microsoft.AspNetCore.SignalR;
using CinemaxFinal.SignalRHubs;
namespace Titanium_MVC.Controllers
{
    //[Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IPropertyService<Property> _propertyService;
        IHubContext<NotificationHub> _hubContext;

        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment env, IPropertyService<Property> propertyService,IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _env = env;
            _propertyService = propertyService;
            _hubContext = hubContext;
        }
        [AllowAnonymous]
        public IActionResult Properties()
        {
            return View(_propertyService.GetAll());
        }
        [HttpGet]
        public ViewResult AddPropertyForm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProperty(Property property)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _env.WebRootPath;
                string path = Path.Combine(wwwRootPath, "UploadedImages");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (property.Image != null && property.Image.Length > 0)
                {
                    string imagePath = Path.Combine(path, property.Image.FileName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await property.Image.CopyToAsync(fileStream);
                    }

                    string imagePathInRoot = "/UploadedImages/" + property.Image.FileName;
                    property.Path = imagePathInRoot;

                    _propertyService.Add(property);

                    // Send a notification after adding the property
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New property added: Location: {property.Location}, Type: {property.Type}, Area: {property.Area}");

                    return Json(new { message = "Property added successfully" });
                }

                return Json(new { message = "Error occurred while saving property data" });
            }

            return Json(new { message = "Invalid data received. Property not added" });
        }


        [HttpGet]
        public ViewResult DeletePropertyForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteProperty(int propertyId)
        {
            Property property = new Property { Id = propertyId };
            if (_propertyService.Delete(property))
            {
                return Content("Deleted");
            }
            return Content("Not Deleted");
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SearchProperty()
        {
            string propertyLocation = Request.Form["propertyLocation"];
            return View(_propertyService.searchProperty(propertyLocation));
        }

        public IActionResult UpdatePropertyForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update()
        {
            int propertyId = int.Parse(Request.Form["PropertyId"]);
            Property property = new Property
            {
                Id = propertyId,
                Location = "updated",
                Area = "updated",
                Path = "updated",
                Type = "land",
                UploadDate = DateTime.Now
            };

            if (_propertyService.Update(property))
            {
                return Content("Updated");
            }
            return Content("Not Updated");
        }
    }
}


//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using System.Security.Cryptography.Xml;
//using Titanium_MVC.Models;
//namespace Titanium_MVC.Controllers
//{
//    //[Authorize(Policy = "RequireAdminRole")]
//    public class AdminController : Controller
//    {
//        private readonly ILogger<AdminController> _logger;
//        private readonly IWebHostEnvironment _env;
//        private readonly InterfaceProperty _propertyRepository;
//        // private readonly GenericRepository<Property> _genericRepositoryProperty;
//        private readonly InterfaceGeneric<Property> _genericRepositoryProperty;

//        public string connectionString;
//        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment env, InterfaceProperty propertyInterface,
//            InterfaceGeneric<Property> genericPropertyRepository)
//        {
//            connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Titanium;Integrated Security=True;";

//            //_genericRepositoryProperty = new GenericRepository<Property>();
//            _logger = logger;
//            _env = env;
//            _propertyRepository = propertyInterface;
//            _genericRepositoryProperty = genericPropertyRepository;
//        }
//        [AllowAnonymous]
//        public IActionResult Properties()
//        {

//            return View(_genericRepositoryProperty.GetAll());

//        }
//        [HttpGet]
//        public ViewResult AddPropertyForm()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult AddProperty(Property property)
//        {

//            if (ModelState.IsValid)
//            {
//                string wwwRootPath = _env.WebRootPath;
//                string path = Path.Combine(wwwRootPath, "UplaodedImages");
//                if (!Directory.Exists(path))
//                {
//                    Directory.CreateDirectory(path);
//                }
//                if (property.Image.Length > 0)
//                {
//                    string imagePath = Path.Combine(path, property.Image.FileName);
//                    using (var FileStream = new FileStream(imagePath, FileMode.Create))
//                    {
//                        property.Image.CopyTo(FileStream);
//                    }
//                    string imagePathInRoot = "/UploadedImages/" + property.Image.FileName;

//                    property.Path = imagePathInRoot;
//                    _genericRepositoryProperty.Add(property);
//                    return Json(new { message = "Product added successfully" });
//                }
//                return Json(new { message = "Error occurred while saving product data" });

//            }
//            return Json(new { message = "Invalid data received Poperty Not Added" });


//        }
//        [HttpGet]
//        public ViewResult DeletePropertyForm()
//        {
//            return View();
//        }

//        [HttpPost]
//        //public IActionResult DeleteProperty()
//        //{
//        //    int propertyId = int.Parse(Request.Form["PropertyId"]);
//        //    _logger.LogDebug($"Attempting to delete property with ID: {propertyId}");
//        //    Debug.WriteLine($"Attempting to delete property with ID: {propertyId}");
//        //    Trace.WriteLine($"Attempting to delete property with ID: {propertyId}");

//        //    Property property = new Property { Id = propertyId };

//        //    if (_genericRepositoryProperty.Delete(property))
//        //    {
//        //        _logger.LogInformation($"Successfully deleted property with ID: {propertyId}");
//        //        Debug.WriteLine($"Successfully deleted property with ID: {propertyId}");
//        //        Trace.WriteLine($"Successfully deleted property with ID: {propertyId}");
//        //        return Content("Deleted");
//        //    }
//        //    else
//        //    {
//        //        _logger.LogWarning($"Failed to delete property with ID: {propertyId}");
//        //        Debug.WriteLine($"Failed to delete property with ID: {propertyId}");
//        //        Trace.WriteLine($"Failed to delete property with ID: {propertyId}");
//        //        return Content("Failed to delete property");
//        //    }
//        //}
//        [HttpPost]
//        public IActionResult DeleteProperty(int propertyId)
//        {
//            var id = propertyId;
//            //int propertyId = int.Parse(Request.Form["PropertyId"]);
//            Property property = new Property();
//            property.Id = propertyId;
//            if (_genericRepositoryProperty.Delete(property))
//            {
//                return Content("Deleted");
//            }
//            return Content("Not Deleted");
//        }
//        [AllowAnonymous]
//        [HttpPost]
//        //public IActionResult SearchProperty()
//        //{
//        //    string propertyLocation = Request.Form["propertyLocation"];
//        //    _logger.LogDebug($"Searching for properties in location: {propertyLocation}");
//        //    Debug.WriteLine($"Searching for properties in location: {propertyLocation}");
//        //    Trace.WriteLine($"Searching for properties in location: {propertyLocation}");

//        //    var properties = _propertyRepository.searchProperty(propertyLocation);

//        //    if (properties == null || !properties.Any())
//        //    {
//        //        _logger.LogWarning($"No properties found in location: {propertyLocation}");
//        //        Debug.WriteLine($"No properties found in location: {propertyLocation}");
//        //        Trace.WriteLine($"No properties found in location: {propertyLocation}");
//        //    }
//        //    else
//        //    {
//        //        _logger.LogInformation($"Found {properties.Count()} properties in location: {propertyLocation}");
//        //        Debug.WriteLine($"Found {properties.Count()} properties in location: {propertyLocation}");
//        //        Trace.WriteLine($"Found {properties.Count()} properties in location: {propertyLocation}");
//        //    }

//        //    return View(properties);
//        //}
//        public IActionResult SearchProperty()
//        {
//            string propertyLocation = Request.Form["propertyLocation"];
//            return View(_propertyRepository.searchProperty(propertyLocation));
//        }
//        public IActionResult UpdatePropertyForm()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult Update()
//        {
//            int propertyId = int.Parse(Request.Form["PropertyId"]);
//            Property property = new Property();
//            property.Id = propertyId;
//            property.Location = "updated";
//            property.Area = "updated";
//            property.Path = "updated";
//            property.Type = "land";
//            property.UploadDate = DateTime.Now;
//            if (_genericRepositoryProperty.Update(property))
//            {
//                return Content("Updated");
//            }
//            return Content("Not Updated");
//        }
//    }
//}

