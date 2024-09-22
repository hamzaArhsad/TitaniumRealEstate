using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Titanium_MVC.Data;
using Titanium_MVC.Models;
using Application;
using Infrastructure;
using Domain;
using Titanium_MVC.Services;
using CinemaxFinal.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddDefaultIdentity<MyAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
// registering services
builder.Services.AddScoped<InterfaceProperty, PropertyRepository>();
builder.Services.AddScoped(typeof(InterfaceGeneric<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPropertyService<Domain.Property>, PropertyService>();
builder.Services.AddScoped<IContactService<Domain.Contact>, ContactService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
    policy.RequireClaim(ClaimTypes.Role, "Admin"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<NotificationHub>("/notificationHub");
app.MapRazorPages();
app.Run();
