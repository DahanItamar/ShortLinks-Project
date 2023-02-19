#pragma warning disable CS8625
#pragma warning disable CS8603
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlProjectV1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LinkDataBase>(option =>
	option.UseSqlServer(builder.Configuration.GetConnectionString("UrlsDataBase")));
builder.Services.AddScoped<LinkDataBase>();


builder.Services.AddCors(options =>
{
	options.AddPolicy(
		"PolicyAccess",
		policy =>
		{
			policy.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
		});
});
builder.Services.AddDefaultIdentity<IdentityUser>()
	.AddDefaultUI()
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<LinkDataBase>();
builder.Services.AddAuthentication()
		.AddGoogle(options =>
		{
			options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
			options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
			options.SaveTokens = true;
		});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("PolicyAccess");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();