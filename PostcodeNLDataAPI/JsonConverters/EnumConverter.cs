using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PostcodeNLDataAPI.JsonConverters;

internal class EnumConverter<T> : JsonConverter<T>
     where T : struct
{
    public string DateTimeFormat { get; private set; }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert != typeof(T))
        {
            throw new InvalidOperationException();
        }

        var value = reader.GetString();
        return Enum.TryParse<T>(value, true, out var result)
            ? result
            : throw new ArgumentException($"'{value}' is not a valid value for '{typeof(T)}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStringValue(value.ToString().ToLowerInvariant());
    }
}
