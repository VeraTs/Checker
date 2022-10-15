using System;
using CheckerServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.InMemory;
using CheckerServer.Hubs;
using CheckerServer.utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// for using azure hosting with sql server service
/*builder.Services.AddDbContext<CheckerDBContext>(options =>
                  options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));*/

builder.Services.AddDbContext<CheckerDBContext>(options =>
                  options.UseSqlServer(builder.Configuration.GetConnectionString("localDB")));

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();
builder.Services.AddSingleton<RestaurantManager>();

builder.Host.ConfigureServices(services => { services.AddHostedService<RestaurantManager>(provider => provider.GetService<RestaurantManager>()); });

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    dbInitializer.Initialize(services);
}

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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<OrdersHub>("/OrdersHub");
app.MapHub<KitchenHub>("/KitchenHub");

app.Run();