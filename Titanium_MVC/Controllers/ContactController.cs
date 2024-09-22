using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Application;
using Domain;

namespace Titanium_MVC.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService<Contact> contactService;

        public ContactController(IContactService<Contact> _contactService)
        {
            contactService = _contactService;
        }
        public ViewResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddContact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contactService.Add(contact);
                return Content("Thanks for contacting Us");
            }
            return Content("Invalid Data");
        }
    }

}
