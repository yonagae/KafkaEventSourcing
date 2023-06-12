using System.Text.Json;
using System.Text.Json.Serialization;
using CQRS.Core.Events;
using Post.Common.Events;

namespace Post.Query.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type type)
        {
            return type.IsAssignableFrom(typeof(BaseEvent));
        }

        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
            {
                throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
            }

            if (!doc.RootElement.TryGetProperty("Type", out var type))
            {
                throw new JsonException("Could not detect the Type discriminator property!");
            }

            var typeDiscriminator = type.GetString();
            var json = doc.RootElement.GetRawText();

            return typeDiscriminator switch
            {
                 nameof(NewFinAccountEvent) => JsonSerializer.Deserialize<NewFinAccountEvent>(json, options),
                nameof(DebitFinAccountEvent) => JsonSerializer.Deserialize<DebitFinAccountEvent>(json, options),
                nameof(CreditFinAccountEvent) => JsonSerializer.Deserialize<CreditFinAccountEvent>(json, options),
                nameof(AddTransactionTypeEvent) => JsonSerializer.Deserialize<AddTransactionTypeEvent>(json, options),

                _ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}