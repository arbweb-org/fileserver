using drive.web.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace drive.web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add DBContext pool
        builder.Services.AddDbContextPool<DriveDbContext>((options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Add services to the container.
        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        // Load global variables from appsettings.json
        Helper.Admin = builder.Configuration.GetSection("Admin").Value ?? string.Empty;
        Helper.Organization = builder.Configuration.GetSection("Organization").Value ?? string.Empty;
        Helper.Salt = builder.Configuration.GetSection("Salt").Value ?? string.Empty;
        Helper.Key = builder.Configuration.GetSection("Key").Value ?? string.Empty;
        Helper.Smtp = builder.Configuration.GetSection("Smtp").Get<SmtpOptions>() ?? new SmtpOptions();

        var app = builder.Build();

        // Auto-migrate DB
        using (var Scope = app.Services.CreateScope())
        {
            var context = Scope.ServiceProvider.GetRequiredService<DriveDbContext>();
            context.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
