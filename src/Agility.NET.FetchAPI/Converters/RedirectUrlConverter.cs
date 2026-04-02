using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Agility.NET.FetchAPI.Models.API;

namespace Agility.NET.FetchAPI.Converters
{
    public class RedirectUrlConverter : JsonConverter<RedirectUrl>
    {
        public override RedirectUrl Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var url = reader.GetString();
                if (string.IsNullOrEmpty(url))
                {
                    return null;
                }
                return new RedirectUrl { Url = url };
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var redirectUrl = new RedirectUrl();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return redirectUrl;
                    }

                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var propertyName = reader.GetString();
                        reader.Read();

                        if (string.Equals(propertyName, "url", StringComparison.OrdinalIgnoreCase))
                        {
                            redirectUrl.Url = reader.GetString();
                        }
                        else if (string.Equals(propertyName, "target", StringComparison.OrdinalIgnoreCase))
                        {
                            redirectUrl.Target = reader.GetString();
                        }
                    }
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, RedirectUrl value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("url", value.Url);
            writer.WriteString("target", value.Target);
            writer.WriteEndObject();
        }
    }
}
