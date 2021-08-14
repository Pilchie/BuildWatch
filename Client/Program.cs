using BuildWatch.Client;
using BuildWatch.Shared;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services
    .AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        options.ProviderOptions.DefaultAccessTokenScopes.Add("api://177e5287-d277-4d65-8e7b-77135c831bed/API.Access");
    });

builder.Services
    .AddGrpcClient<WeatherForecasts.WeatherForecastsClient>((services, options) =>
    {
        options.Address = new Uri(ServiceProviderServiceExtensions.GetRequiredService<NavigationManager>(services).BaseUri);
    })
    .ConfigurePrimaryHttpMessageHandler(services =>
     {
         return new GrpcWebHandler(new HttpClientHandler());
     }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

await builder.Build().RunAsync();
