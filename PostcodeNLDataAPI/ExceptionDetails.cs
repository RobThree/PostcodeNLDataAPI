using System.Text.Json.Serialization;

namespace PostcodeNLDataAPI;

/// <summary>
/// For internal use only; used to deserialize exception details from the 'backend' (e.g. postcode.nl) into a <see cref="PostcodeNLException"/>.
/// </summary>
internal class ExceptionDetails
{
    [JsonPropertyName("exception")]
    public string Exception { get; set; }
    [JsonPropertyName("exceptionId")]
    public string ExceptionId { get; set; }
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }
}
