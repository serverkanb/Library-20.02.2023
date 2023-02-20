using Business.Services;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);





#region Localization

List<CultureInfo> cultures = new List<CultureInfo>()
{
	new CultureInfo("en-US") // eðer uygulama Türkçe olacaksa CultureInfo constructor'ýnýn parametresini ("tr-TR") yapmak yeterlidir.
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
	options.SupportedCultures = cultures;
	options.SupportedUICultures = cultures;
});
#endregion


// Add services to the container.
builder.Services.AddControllersWithViews();


#region Authentication
builder.Services
	.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	// projeye Cookie authentication default'lar?n? kullanarak kimlik do?rulama ekliyoruz

	.AddCookie(config =>
	// olu?turulacak cookie'yi config action delegesi üzerinden konfigüre ediyoruz, action delegeleri func delegeleri gibi bir sonuç dönmez,
	// üzerlerinden burada oldu?u gibi genelde konfigürasyon i?lemleri yap?l?r

	{
		config.LoginPath = "/Account/Users/Login";
		// e?er sisteme giri? yap?lmadan bir i?lem yap?lmaya çal???l?rsa kullan?c?y? Account area -> Users controller -> Login action'?na yönlendir

		config.AccessDeniedPath = "/Account/Users/AccessDenied";
		//// e?er sisteme giri? yap?ld?ktan sonra yetki d??? bir i?lem yap?lmaya çal???l?rsa kullan?c?y? Account area -> Users controller -> AccessDenied
		// action'?na yönlendir

		config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
		// sisteme giri? yap?ld?ktan sonra olu?an cookie 30 dakika boyunca kullan?labilsin

		config.SlidingExpiration = true;
		// SlidingExpiration true yap?larak kullan?c? sistemde her i?lem yapt???nda cookie kullan?m süresi yukar?da belirtilen 30 dakika uzat?l?r,
		// e?er false atan?rsa kullan?c?n?n cookie ömrü yukar?da belirtilen 30 dakika sonra sona erer ve yeniden giri? yapmak zorunda kal?r
	});
#endregion

#region AppSettings
//var section = builder.Configuration.GetSection("AppSettings");
var section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings());
#endregion


#region IoC Container (Inversion of Control)
// Autofac, Ninject
var connectionString = builder.Configuration.GetConnectionString("LibraryDb");
builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlServer(connectionString));

// builder.Services.AddTransient<ProductRepoBase, ProductRepo>();   her enjeksiyonda yeni obje olu?turur
// builder.Services.AddSingleton<ProductRepoBase, ProductRepo>();   statik obje kullanman? sa?lar
builder.Services.AddScoped<BookRepoBase, BookRepo>(); // önemli
builder.Services.AddScoped<CategoryRepoBase, CategoryRepo>();
builder.Services.AddScoped<WriterRepoBase, WriterRepo>();
builder.Services.AddScoped<CountryRepoBase, CountryRepo>();
builder.Services.AddScoped<CityRepoBase, CityRepo>();
builder.Services.AddScoped<UserRepoBase, UserRepo>();



builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IWriterService, WriterService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
#endregion

var app = builder.Build();

#region Localization
app.UseRequestLocalization(new RequestLocalizationOptions()
{
	DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
	SupportedCultures = cultures,
	SupportedUICultures = cultures,
});
#endregion

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

#region Authentication
app.UseAuthentication(); // sen kimsin?
#endregion

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
});


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
