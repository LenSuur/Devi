﻿using System.Diagnostics.CodeAnalysis;
using Devi.Data;
using Devi.Data.Repositories;
using Devi.Services;
using Microsoft.EntityFrameworkCore;

namespace Devi;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<DeviContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DeviContext") ??
                                 throw new InvalidOperationException("Connection string 'DeviContext' not found.")));

// Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        builder.Services.AddScoped<IDeviceService, DeviceService>();
        builder.Services.AddScoped<IFileClient, LocalFileClient>();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}