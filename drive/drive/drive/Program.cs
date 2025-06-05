using drive.Components;
using Microsoft.EntityFrameworkCore;

namespace drive
{
    public class Program
    {
        // Static field to store error messages
        public static string errorLog = string.Empty;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DBContext pool
            builder.Services.AddDbContextPool<DriveDbContext>((options) =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddControllers();

            Helper.Initialize(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
            app.MapControllers();

            // Ensure DB is created
            using (var Scope = app.Services.CreateScope())
            {
                var context = Scope.ServiceProvider.GetRequiredService<DriveDbContext>();
                context.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}