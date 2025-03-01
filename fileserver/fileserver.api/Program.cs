using Microsoft.EntityFrameworkCore;

namespace fileserver.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            // Add DBContext pool
            builder.Services.AddDbContextPool<FilesDbContext>((options) =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Get Password and Salt from appsettings.json
            Helper.Password = builder.Configuration.GetSection("Password").Value;
            Helper.Salt = builder.Configuration.GetSection("Salt").Value;

            // Get MailServer, MailPort, MailUser, MailPassword from appsettings.json
            Helper.MailServer = builder.Configuration.GetSection("MailServer").Value;
            Helper.MailPort = int.Parse(builder.Configuration.GetSection("MailPort").Value);
            Helper.MailUser = builder.Configuration.GetSection("MailUser").Value;
            Helper.MailPassword = builder.Configuration.GetSection("MailPassword").Value;

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapControllers();

            // Auto-migrate DB
            using (var Scope = app.Services.CreateScope())
            {
                var context = Scope.ServiceProvider.GetRequiredService<FilesDbContext>();
                context.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}