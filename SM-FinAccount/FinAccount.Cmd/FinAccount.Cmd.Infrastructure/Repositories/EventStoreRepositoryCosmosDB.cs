using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Stores;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepositoryCosmosDB : IEventStoreRepository
    {
        private CosmosClient _client = null!;
        private Database _database = null!;
        private Container _container = null!;

        public EventStoreRepositoryCosmosDB(IOptions<CosmosDBConfig> config)
        {
            _client = new CosmosClient(config.Value.ConnectionString, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway,
                SerializerOptions = new CosmosSerializationOptions()
                {
                    
                }
            });

            _database = Task.Run(async () => await _client.CreateDatabaseIfNotExistsAsync(config.Value.Database)).Result;


            _container = Task.Run(async () => await _database.CreateContainerIfNotExistsAsync(config.Value.Collection, config.Value.PartitionKey)).Result;
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            var queryable = _container.GetItemLinqQueryable<EventModel>();

            var matches = queryable.Where(x => x.AggregateIdentifier.ToString() == aggregateId.ToString());

           using FeedIterator<EventModel> linqFeed = matches.ToFeedIterator();

            while (linqFeed.HasMoreResults)
            {
                FeedResponse<EventModel> response = await linqFeed.ReadNextAsync();
                return response.ToList();
            }

            return new List<EventModel>();
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _container.CreateItemAsync(@event, new PartitionKey(@event.AggregateIdentifier.ToString())).ConfigureAwait(false);
        }
    }
}