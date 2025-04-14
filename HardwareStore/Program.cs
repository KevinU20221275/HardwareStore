using FluentValidation;
using HardwareStore.Models;
using HardwareStore.Repositories.Categories;
using HardwareStore.Repositories.Employees;
using HardwareStore.Repositories.Products;
using HardwareStore.Repositories.Customers;
using HardwareStore.Repositories.Roles;
using HardwareStore.Repositories.Sales;
using HardwareStore.Repositories.Suppliers;
using HardwareStore.Repositories.Logins;
using HardwareStore.Repositories.Reports;
using HardwareStore.Services.Email;
using HardwareStore.Validations;
using Microsoft.AspNetCore.Authentication.Cookies;
using HardwareStore.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// conection database
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

// validations
builder.Services.AddScoped<IValidator<Employee>, EmployeeValidator>();
builder.Services.AddScoped<IValidator<Sale>, SaleValidator>();
builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();
builder.Services.AddScoped<IValidator<Supplier>, SupplierValidator>();
builder.Services.AddScoped<IValidator<Role>, RoleValidator>();
builder.Services.AddScoped<IValidator<UpdatePassword>, UpdatePasswordValidator>();

// repositories
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

// services
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Login/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Home/AccessDenied";
});

var app = builder.Build();

// no guardar cache
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
