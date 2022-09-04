using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using e_commerce_web.Extension;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddMemoryCache();
            services.AddSession();
            services.AddScoped<Saveimage>();
            services.AddMemoryCache();
            services.AddControllersWithViews();

            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            services.AddSession(options => 
            { 
                options.Cookie.Name = "e_commerce.Session"; 
            });

            services.AddAuthentication("CookieAuthentication_e_commerce")
                        .AddCookie("CookieAuthentication_e_commerce", config =>
                        {
                            config.Cookie.Name = "e_commerce_UserLoginCookie";
                            config.ExpireTimeSpan = TimeSpan.FromMinutes(30); ;
                            config.LoginPath = "/dang-nhap.html";
                            config.LogoutPath = "/dang-xuat.html";
                            config.AccessDeniedPath = "/not-found.html";
                        });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings,only this changes expiration
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromMinutes(15);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
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
