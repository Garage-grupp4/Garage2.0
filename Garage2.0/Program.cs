using Garage2._0.Data;
using Garage2._0.Models;
using Garage2._0.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Garage2._0
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString =
                builder.Configuration.GetConnectionString("GarageContext")
                ?? throw new InvalidOperationException("Connection string 'GarageContext' not found.");

            var provider = builder.Configuration["DbProvider"] ?? "Sqlite";

            builder.Services.AddDbContext<GarageContext>(opt =>
            {
                if (provider == "SqlServer")
                    opt.UseSqlServer(connectionString);
                else
                    opt.UseSqlite(connectionString);
            });


            // builder.Services.AddDbContext<GarageContext>(options => options.UseSqlite(connectionString));

            builder.Services.Configure<PrakingPricingOptions>(builder.Configuration.GetSection("PrakingPricing"));

            // implementera Auth
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // TODO(#13): Höj lösenordskrav innan produktion (US01 acceptanskriterier)
                    options.SignIn.RequireConfirmedAccount = false; // dev-läge, ingen mmailbekräftelse
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
              .AddEntityFrameworkStores<GarageContext>()
               .AddDefaultTokenProviders();

            // Cookie paths
            builder.Services.ConfigureApplicationCookie(options =>
          {
              options.LoginPath = "/Identity/Account/Login";
              options.LogoutPath = "/Identity/Account/Logout";
              options.AccessDeniedPath = "/Identity/Account/AccessDenied";
          });

            // Mvc + Razor + Email
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddTransient<IEmailSender, GarageEmailSender>();

            var app = builder.Build();

            // Databas
            using (var scope = app.Services.CreateScope()) // öppna scope
            {
                var db = scope.ServiceProvider.GetRequiredService<GarageContext>(); // hämta DbContext
                await SeedData.Initialize(db, scope.ServiceProvider); // gör jobbet
            } // scope stängs, db disposas

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}"
                )
                .WithStaticAssets();

            app.MapRazorPages(); // för att scafooldin ska funka

            app.Run();
        }
    }
}
