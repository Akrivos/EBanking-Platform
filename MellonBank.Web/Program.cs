using MellonBank.Application;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Infrastructure;
using MellonBank.Infrastructure.Identity;
using MellonBank.Infrastructure.Initialization;
using MellonBank.Infrastructure.Persistence;
using MellonBank.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MellonBank.Web
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            builder.Host.UseSerilog();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddControllersWithViews();
            services.AddApplication();
            services.AddInfrastructure(builder.Configuration);
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddRazorPages();

            var app = builder.Build();

            // Seed staff user and roles
            using (var scope = app.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var config = scopeService.GetRequiredService<IConfiguration>();

                await ApplicationInitializer.InitializeAsync(scopeService, config);
            }

            app.UseSerilogRequestLogging();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            app.Run();
        }
    }
}
