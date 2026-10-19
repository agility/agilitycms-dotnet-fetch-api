using System;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Agility.NET.FetchAPI.Converters
{
    /// <summary>
    /// Lets Newtonsoft.Json read and write a <see cref="JsonElement"/> (or <c>JsonElement?</c>) property, which it
    /// can't do on its own. The GraphQL client deserializes with Newtonsoft; the REST paths use System.Text.Json,
    /// which ignores this attribute.
    /// </summary>
    public sealed class NewtonsoftJsonElementConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(JsonElement) || objectType == typeof(JsonElement?);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            // Keep date-like strings as written. Read numbers as doubles: decimal would fail the whole response on a
            // value above its range (1e30) and turn very small values (1e-40) into 0.
            var dateParseHandling = reader.DateParseHandling;
            var floatParseHandling = reader.FloatParseHandling;
            reader.DateParseHandling = DateParseHandling.None;
            reader.FloatParseHandling = FloatParseHandling.Double;
            JToken token;
            try
            {
                token = JToken.Load(reader);
            }
            finally
            {
                reader.DateParseHandling = dateParseHandling;
                reader.FloatParseHandling = floatParseHandling;
            }

            if (token.Type == JTokenType.Null || token.Type == JTokenType.Undefined)
            {
                return objectType == typeof(JsonElement) ? default(JsonElement) : null;
            }

            using var document = JsonDocument.Parse(token.ToString(Formatting.None));
            return document.RootElement.Clone();
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is JsonElement element && element.ValueKind != JsonValueKind.Undefined)
            {
                JToken.Parse(element.GetRawText()).WriteTo(writer);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
