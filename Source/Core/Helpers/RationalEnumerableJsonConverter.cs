using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rationals;

namespace NashEquilibriumFinder.Core.Helpers
{
    // TODO: Do i need this?
    // Use factory pattern to this generic type
    public class RationalEnumerableJsonConverter : JsonConverter<List<Rational>>
    {
        public override List<Rational> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            List<Rational> value,
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var number in value)
            {
                writer.WriteStartObject();

                writer.WriteNumber("numerator", (int)number.Numerator);
                writer.WriteNumber("denominator", (int)number.Denominator);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
