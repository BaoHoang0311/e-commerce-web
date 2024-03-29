﻿using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using e_commerce_web.Data;
using e_commerce_web.Data.Services;
using e_commerce_web.Extension;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web
{
    public class Startup
    {
        private string _connection = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new SqlConnectionStringBuilder(
                    Configuration.GetConnectionString("DefaulConnectionString"));
            builder.Password = Configuration["DbPassword"];
            _connection = builder.ConnectionString;

            // connectionString
            services.AddDbContext<dbMarketsContext>(options =>
                options.UseSqlServer(_connection));

            services.AddScoped<ProductServices>();
            services.AddScoped<Saveimage>();
            services.AddScoped<ShoppingCartService>();
            
            services.AddMemoryCache();
            
            services.AddControllersWithViews();

            // add noti
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin", "NV"));
            });
            // *1
            services.AddSession();
            services.AddAuthentication("abc")
                        .AddCookie("abc", config =>
                        {
                            config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                            config.LoginPath = "/dang-nhap.html";
                            config.LogoutPath = "/dang-xuat.html";
                            config.AccessDeniedPath = "/NotFound.html";
                        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Quicksort + MergeSort

            app.UseRouting();

            app.UseSession();

            app.UseNotyf();
            app.UseAuthentication(); // xác thực

            app.UseAuthorization(); // quyền

            app.UseEndpoints(endpoints =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
