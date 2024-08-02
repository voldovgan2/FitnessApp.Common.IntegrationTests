using FitnessApp.Common.ServiceBus.Nats.Services;
using FitnessApp.Common.Vault;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using System.Net.Http.Headers;
using FitnessApp.Common.Abstractions.Db.Entities.Generic;

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
            })
            .UseEnvironment("Development");
    }

    public HttpClient CreateHttpClient()
    {
        var httpClient = CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: MockConstants.Scheme);
        return httpClient;
    }
}

public class TestAbstractWebApplicationFactoryBase<
    TProgram,
    TAuthenticationHandler,
    TEntity
    >(MongoDbFixtureBase<TEntity> fixture) :
    TestWebApplicationFactoryBase<TProgram, TAuthenticationHandler>
    where TProgram : class
    where TAuthenticationHandler : MockAuthenticationHandlerBase
    where TEntity : IGenericEntity
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureTestServices(services =>
            {
                services.RemoveAll<IMongoClient>();
                services.AddSingleton<IMongoClient>((_) => fixture.Client);
            });
    }
}
