using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Modal.Services;
using MealOrdering.Client;
using MealOrdering.Client.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace MealOrdering.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<IModalService, ModalService>(); // Blazored Modal kullanýyorsanýz bu gerekli olabilir
            builder.Services.AddScoped<ModalManager>(); // ModalManager servisini ekleyin
            await builder.Build().RunAsync();
        }
    }
}