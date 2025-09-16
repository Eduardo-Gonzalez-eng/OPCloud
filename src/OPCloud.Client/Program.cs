using System;
using System.Net.Http;
using System.Threading.Tasks;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OPCloud.Client.Services;

namespace OPCloud
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);


            // HttpClient scoped (ya incluido por defecto en WASM, pero lo dejamos explícito)
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Servicios de aplicación como Scoped
            builder.Services.AddScoped<SensorService>();
            //builder.Services.AddScoped<SignalRService>();

            AddClientServices(builder.Services);

            builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));

            await builder.Build().RunAsync();
        }

        public static void AddClientServices(IServiceCollection services)
        {
            services.AddAntDesign();
        }
    }
}