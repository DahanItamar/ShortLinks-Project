using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlProjectV1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// SQL Server when a connection string is configured; otherwise an in-memory
// database so the service runs out of the box with zero setup.
var connectionString = builder.Configuration.GetConnectionString("UrlsDataBase");
builder.Services.AddDbContext<LinkDataBase>(options =>
{
	if (string.IsNullOrWhiteSpace(connectionString))
		options.UseInMemoryDatabase("ShortLinks");
	else
		options.UseSqlServer(connectionString);
});

// No email infrastructure in this project, so accounts work without
// email confirmation — register and you're in.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddDefaultUI()
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<LinkDataBase>();

// Google sign-in: real credentials come from user-secrets or environment
// variables (see .env.example) — never from git. Without them the button
// still renders; it just can't complete the round-trip to Google.
builder.Services.AddAuthentication()
	.AddGoogle(options =>
	{
		options.ClientId = builder.Configuration["Authentication:Google:ClientId"]
			?? "not-configured.apps.googleusercontent.com";
		options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
			?? "not-configured";
		options.SaveTokens = true;
	});

var app = builder.Build();

// Create the schema automatically: EnsureCreated in memory, migrations-free bootstrap on SQL Server.
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<LinkDataBase>();
	db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Identity UI (login, register)
app.Run();
