using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using PostcodeNLDataAPI.JsonConverters;

namespace PostcodeNLDataAPI;

/// <summary>
/// Represents the arguments that can be used when querying for deliveries.
/// </summary>
public class DeliveriesQuery
{
    /// <summary>
    /// Account identifier.
    /// </summary>
    public long? AccountId { get; set; }
    /// <summary>
    /// <see cref="PostcodeNLDataAPI.DeliveryType"/>.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<DeliveryType>))]
    public DeliveryType? DeliveryType { get; set; }
    /// <summary>
    /// Deliveries with target date greater than, or equal to, this date.
    /// </summary>
    public DateTime? From { get; set; }
    /// <summary>
    /// Deliveries with target date less than, or equal to, this date.
    /// </summary>
    public DateTime? To { get; set; }
    /// <summary>
    /// Deliveries with target date greater than this date.
    /// </summary>
    public DateTime? After { get; set; }

    internal IDictionary<string, string> AsKeyValueCollection()
    {
        var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (AccountId.HasValue)
        {
            d.Add("accountId", AccountId.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (DeliveryType.HasValue)
        {
            d.Add("deliveryType", DeliveryType.Value.ToString().ToLowerInvariant());
        }

        if (From.HasValue)
        {
            d.Add("from", From.Value.ToString(PostcodeNL.DATETIMEFORMAT, CultureInfo.InvariantCulture));
        }

        if (To.HasValue)
        {
            d.Add("to", To.Value.ToString(PostcodeNL.DATETIMEFORMAT, CultureInfo.InvariantCulture));
        }

        if (After.HasValue)
        {
            d.Add("after", After.Value.ToString(PostcodeNL.DATETIMEFORMAT, CultureInfo.InvariantCulture));
        }

        return d;
    }
}
