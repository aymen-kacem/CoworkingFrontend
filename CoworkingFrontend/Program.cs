using CoworkingBlazor;
using CoworkingBlazor.Services;
using CoworkingFrontend;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with your backend API URL (use HTTPS to avoid preflight redirect)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7141/") // Update to your API HTTPS URL
});

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAbonnementService, AbonnementService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IEspaceService, EspaceService>();
builder.Services.AddScoped<IRessourceService, RessourceService>();

await builder.Build().RunAsync();