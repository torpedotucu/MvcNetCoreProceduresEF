using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("SqlHospital");
builder.Services.AddTransient<RepositoryEnfermos>();
builder.Services.AddTransient<RepositoryEmpleados>();
builder.Services.AddDbContext<EnfermosContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<RepositoryDoctores>();
builder.Services.AddDbContext<DoctoresContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<RepositoryTrabajadores>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
