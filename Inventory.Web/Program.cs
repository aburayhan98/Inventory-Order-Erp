using Inventory.Business.Interfaces.Persistence.IOrder;
using Inventory.Business.Interfaces.Persistence.IProduct;
using Inventory.Business.Interfaces.Persistence.IServices;
using Inventory.Business.Services;
using Inventory.DataAccess.Connection;
using Inventory.DataAccess.Data.Commands;
using Inventory.DataAccess.Data.Queries;
using Inventory.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Config / DB
builder.Services.AddScoped<DbConfig>();

// Product CQS
builder.Services.AddScoped<IProductCommand, ProductCommand>();
builder.Services.AddScoped<IProductQuery, ProductQuery>();

// Order CQS
builder.Services.AddScoped<IOrderCommand, OrderCommand>();
builder.Services.AddScoped<IOrderQuery, OrderQuery>();

// Business Services
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
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseGlobalExceptionHandling();

app.UseRouting();

app.UseRouting();

// If Identity is not added yet, Authorization alone is okay for now.
// Later add: app.UseAuthentication(); before UseAuthorization().
app.UseAuthorization();

app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();