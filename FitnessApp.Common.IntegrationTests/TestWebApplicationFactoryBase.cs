using FitnessApp.Common.ServiceBus.Nats.Services;
using FitnessApp.Common.Vault;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FitnessApp.Common.IntegrationTests;

public class TestWebApplicationFactoryBase<TProgram, TAuthenticationHandler> : WebApplicationFactory<TProgram>
    where TProgram : class
    where TAuthenticationHandler: MockAuthenticationHandlerBase
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication(defaultScheme: MockConstants.Scheme)
                    .AddScheme<AuthenticationSchemeOptions, TAuthenticationHandler>(MockConstants.Scheme, options => { });

                services.RemoveAll<IVaultService>();
                services.AddSingleton<IVaultService, MockVaultService>();

                services.RemoveAll<IServiceBus>();
                services.AddSingleton<IServiceBus, MockServiceBus>();

                services.RemoveAll<IDistributedCache>();
                services.AddSingleton(Options.Create(new MemoryDistributedCacheOptions()));
                services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
            })
            .UseEnvironment("Development");
    }
}
