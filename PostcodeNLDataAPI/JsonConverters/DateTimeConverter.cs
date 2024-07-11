using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PostcodeNLDataAPI.JsonConverters;

internal class DateTimeConverter(string dateTimeFormat) : JsonConverter<DateTime>
{
    public string DateTimeFormat { get; private set; } = dateTimeFormat ?? throw new ArgumentNullException(nameof(dateTimeFormat));

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => typeToConvert != typeof(DateTime)
            ? throw new InvalidOperationException()
            : DateTime.ParseExact(reader.GetString(), DateTimeFormat, CultureInfo.InvariantCulture);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
    }
}
