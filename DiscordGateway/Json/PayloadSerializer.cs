using DiscordGateway.DiscordEventPayloads;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DiscordGateway.Json
{
    public class PayloadSerializer : System.Text.Json.Serialization.JsonConverter<BasePayload>
    {
        private System.Text.StringBuilder _stringBuilder;
        public PayloadSerializer()
        {
            _stringBuilder = new StringBuilder();
        }

        private string GetObjectString(ref Utf8JsonReader reader)
        {
            do
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        _stringBuilder.Append('[');
                        break;
                    case JsonTokenType.StartObject:
                        _stringBuilder.Append('{');
                        break;
                    case JsonTokenType.String:
                        _stringBuilder.Append(reader.GetString());
                        break;
                    case JsonTokenType.Number:
                        _stringBuilder.Append(reader.GetInt32());
                        break;
                    case JsonTokenType.True:
                        _stringBuilder.Append("true");
                        break;
                    case JsonTokenType.False:
                        _stringBuilder.Append("false");
                        break;
                    case JsonTokenType.PropertyName:
                        _stringBuilder.Append('\"');
                        _stringBuilder.Append(reader.GetString());
                        _stringBuilder.Append('\"');
                        _stringBuilder.Append(':');
                        break;
                    case JsonTokenType.EndArray:
                        _stringBuilder.Append(']');
                        break;
                    default:
                        break;
                }
                reader.Read();
            } while (reader.TokenType != JsonTokenType.EndArray || reader.TokenType != JsonTokenType.EndObject);
            // todo: avoid allocations somehow
            return _stringBuilder.ToString();
        }
        private void ParseData(ref Utf8JsonReader reader, ref BasePayload data, Constants.OpCode op)
        {
            switch (op)
            {
                case Constants.OpCode.Hello:
                    break;
                case Constants.OpCode.HeartbeatAck:
                    reader.Read();
                    //data.Event = JsonSerializer.Deserialize<Hello>(GetObjectString(ref reader));
                    break;
                default:
                    break;
            }
        }

        public override BasePayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            BasePayload result = new BasePayload();
            do
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    switch (reader.GetString())
                    {
                        case Constants.PayloadProperties.OP:
                            reader.Read();
                            result.Op = (Constants.OpCode)reader.GetInt32();
                            break;
                        case Constants.PayloadProperties.DATA:
                            ParseData(ref reader, ref result, result.Op);
                            break;
                        default:
                            break;
                    }
                }
            }
            while (reader.Read());
            return result;
        }

        public override void Write(Utf8JsonWriter writer, BasePayload value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
