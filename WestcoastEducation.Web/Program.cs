using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add database support from my own api
builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the database 
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
