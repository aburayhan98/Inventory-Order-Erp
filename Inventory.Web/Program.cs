using Inventory.Business.Interfaces.Persistence.IOrder;
using Inventory.Business.Interfaces.Persistence.IProduct;
using Inventory.Business.Interfaces.Persistence.IServices;
using Inventory.Business.Services;
using Inventory.DataAccess.Connection;
using Inventory.DataAccess.Data.Commands;
using Inventory.DataAccess.Data.Queries;
using Inventory.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Identity DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseSqlServer(
				builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services
		.AddDefaultIdentity<IdentityUser>(options =>
		{
			options.SignIn.RequireConfirmedAccount = false;
			options.Password.RequireDigit = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequiredLength = 6;
		})
		.AddEntityFrameworkStores<ApplicationDbContext>();

// Require authenticated user globally
builder.Services.AddAuthorization(options =>
{
	options.FallbackPolicy = new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.Build();
});

// Dapper config
builder.Services.AddScoped<DbConfig>();

// Product CQS
builder.Services.AddScoped<IProductCommand, ProductCommand>();
builder.Services.AddScoped<IProductQuery, ProductQuery>();

// Order CQS
builder.Services.AddScoped<IOrderCommand, OrderCommand>();
builder.Services.AddScoped<IOrderQuery, OrderQuery>();

// Business services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

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
		pattern: "{controller=Product}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();