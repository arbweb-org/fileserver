using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace drive.Client
{
    internal class Program
    {
        public static Uri BaseAddress;

        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

            await builder.Build().RunAsync();
        }
    }
}