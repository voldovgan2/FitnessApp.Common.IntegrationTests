using FitnessApp.Common.Abstractions.Db.Configuration;
using FitnessApp.Common.Abstractions.Db.DbContext;
using FitnessApp.Common.Abstractions.Db.Entities.Generic;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;

namespace FitnessApp.Common.IntegrationTests;
public abstract class MongoDbFixtureBase<TEntity> : IDisposable
    where TEntity : IGenericEntity
{
    private readonly MongoDbRunner _runner;
    public MongoClient Client { get; }

    protected MongoDbFixtureBase(MongoDbSettings mongoDbSettings)
    {
        _runner = MongoDbRunner.Start();
        Client = new MongoClient(_runner.ConnectionString);        
        var dbContext = new DbContext<TEntity>(Client, Options.Create(mongoDbSettings));
        CreateMockData(dbContext).GetAwaiter().GetResult();
    }

    protected abstract Task CreateMockData(DbContext<TEntity> dbContext);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _runner.Dispose();
    }
}
