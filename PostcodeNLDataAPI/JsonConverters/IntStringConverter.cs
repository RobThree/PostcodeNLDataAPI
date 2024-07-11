using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PostcodeNLDataAPI.JsonConverters;

internal class IntStringConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => typeToConvert != typeof(int)
            ? throw new InvalidOperationException()
            : int.TryParse(reader.GetString(), out var result) ? result : 0;

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}
