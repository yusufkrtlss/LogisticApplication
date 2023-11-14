using LogisticMVC.DTOs.APISettings;
using LogisticMVC.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<ILanguageService, LanguageManager>();


// appsettings.json dosyasýný yükleyin
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
builder.Services.AddSingleton(apiSettings);

// Localization for Language
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
           .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
           .AddDataAnnotationsLocalization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
                new CultureInfo("tr-TR"), // Türkçe
                new CultureInfo("en-US")  // Ýngilizce
            };

    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Http Client
builder.Services.AddHttpClient();


var app = builder.Build();

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
app.UseRequestLocalization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "Admin/YetkiTanimlari/SirketTanimlari/Index",
        defaults: new { controller = "Company", action = "Index" });

    endpoints.MapControllerRoute(
        name: "users",
        pattern: "Admin/YetkiTanimlari/KullaniciTanimlari/Index",
        defaults: new { controller = "User", action = "Index" });

    endpoints.MapControllerRoute(
        name: "roles",
        pattern: "Admin/YetkiTanimlari/RolTanimlari/Index",
        defaults: new { controller = "Roles", action = "Index" });

    //endpoints.MapControllerRoute(
    //    name: "users",
    //    pattern: "Admin/YetkiTanimlari/KullaniciTanimlari/Index/CreateUser",
    //    defaults: new { controller = "User", action = "CreateUser" });
});
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
