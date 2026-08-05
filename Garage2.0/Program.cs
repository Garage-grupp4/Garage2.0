using Garage2._0.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString =
                builder.Configuration.GetConnectionString("GarageContext")
                ?? throw new InvalidOperationException("Connection string 'GarageContext' not found.");
            
            var provider = builder.Configuration["DbProvider"] ?? "Sqlite";
 
            builder.Services.AddDbContext<GarageContext>(opt =>
            {
                if (provider == "SqlServer")
                    opt.UseSqlServer(builder.Configuration.GetConnectionString(connectionString));
                else
                    opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
            });
            builder.Services.AddDbContext<GarageContext>(options => options.UseSqlite(connectionString));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Databas
            using (var scope = app.Services.CreateScope()) // öppna scope
            {
                var db = scope.ServiceProvider.GetRequiredService<GarageContext>(); // hämta DbContext
                SeedData.Initialize(db); // gör jobbet
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

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}"
                )
                .WithStaticAssets();

            app.Run();
        }
    }
}
