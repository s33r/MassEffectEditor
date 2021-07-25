using Aaron.MassEffectEditor.WebUI.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(
                sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

            builder.Services.AddScoped<DialogService>();

            builder.Services.AddSingleton<MassEffectData>();

            await builder.Build().RunAsync();
        }
    }
}