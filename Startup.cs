using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using DatePot.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elmah.Io.AspNetCore;
using Serilog;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using DatePot.Db;
using DatePot.Areas.FilmPot.Data;
using DatePot.Areas.FoodPot.Data;
using DatePot.Areas.CoffeePot.Data;
using DatePot.Areas.ActivityPot.Data;
using DatePot.Areas.Identity.Data;
using DatePot.Areas.VinylPot.Data;
using WebPWrecover.Services;

namespace DatePot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1200);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(
                            Configuration.GetConnectionString("Default")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();

            services.AddSingleton<ISqlDb, SqlDb>();
            services.AddSingleton<IFilmData, FilmData>();
            services.AddSingleton<IFoodData, FoodData>();
            services.AddSingleton<ICoffeeData, CoffeeData>();
            services.AddSingleton<IActivityData, ActivityData>();
            services.AddSingleton<IVinylData, VinylData>();
            services.AddSingleton<ISiteData, SiteData>();
            services.AddSingleton<IIdentityData, IdentityData>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddElmahIo(o =>
            {
                o.ApiKey = "98744865cb034c31a1719586a0f73cbe";
                o.LogId = new Guid("95fe989e-1120-4361-835f-8f6d61e91879");
            });
            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.SignIn.RequireConfirmedEmail = false;
            });

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"D:\HostingSpaces\scud97\thedatepot.co.uk\data\keys\"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseElmahIo();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
