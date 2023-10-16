using GameStore.Data.DbContexts;
using GameStore.Service.Commons.Helpers;
using GameStore.Web.Configurations;
using GameStore.Web.Middlewares;
using GameStore.Web.Models.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCustomService();
builder.Services.AddWeb(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Add HttpContextHelper
app.InitAccessor();

// Get wwwroot path
EnvironmentHelper.WebRootPath = Path.GetFullPath("wwwroot");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Add custom middleware
app.UseMiddleware<TokenRedirectMiddleware>();


//app.UseStatusCodePages(async context =>
//{
//    if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
//    {
//        context.HttpContext.Response.Redirect("login");
//    }
//});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData.EnsurePopulated(app);

app.Run();
