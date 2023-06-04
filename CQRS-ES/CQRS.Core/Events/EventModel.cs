using CQRS.Core.Infrastructure;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CQRS.Core.Events
{
    public class EventModel 
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime TimeStamp { get; set; }
        public Guid AggregateIdentifier { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; }

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Objects)]
        public BaseEvent EventData { get; set; }
    }
}