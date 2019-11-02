using DiscordGateway.DiscordEventPayloads;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DiscordGateway.Json
{
    public class PayloadSerializer : System.Text.Json.Serialization.JsonConverter<BasePayload>
    {
        private void ParseData(ref Utf8JsonReader reader, Constants.OpCode op)
        {
            switch(op)
            {
                case Constants.OpCode.Hello:
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
                if(reader.TokenType == JsonTokenType.PropertyName)
                {
                    switch(reader.GetString())
                    {
                        case Constants.PayloadProperties.OP:
                            reader.Read();
                            result.Op = (Constants.OpCode)reader.GetInt32();
                            break;
                        case Constants.PayloadProperties.DATA:
                            ParseData(ref reader, result.Op);
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
