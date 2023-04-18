using FOXIC.DataAccesLayer;
using FOXIC.Entities.UserModels;
using FOXIC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FoxicDb>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<LayoutService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
	opt.Password.RequiredUniqueChars = 3;
	opt.Password.RequireNonAlphanumeric = false;
	opt.Password.RequiredLength = 8;
	opt.Password.RequireDigit = true;
	opt.Password.RequireLowercase = true;
	opt.Password.RequireUppercase = true;

	opt.User.RequireUniqueEmail = true;
	opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnm1234567890";

	opt.Lockout.MaxFailedAccessAttempts = 5;
	opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
}).AddDefaultTokenProviders().AddEntityFrameworkStores<FoxicDb>();


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
app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);

	endpoints.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
