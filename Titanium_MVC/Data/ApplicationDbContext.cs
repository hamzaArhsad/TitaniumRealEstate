using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Titanium_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Domain;
namespace Titanium_MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
