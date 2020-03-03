using System;
using System.Text.Json.Serialization;

namespace PostcodeNLDataAPI.Entities
{
    /// <summary>
    /// Represents a Postcode.nl delivery.
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// Gets/sets the identifier for this specific delivery.
        /// </summary>
        [JsonPropertyName("deliveryId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets/sets the identifier for the account this delivery belongs to.
        /// </summary>
        [JsonPropertyName("accountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// <see cref="PostcodeNLDataAPI.DeliveryType"/>.
        /// </summary>
        [JsonPropertyName("deliveryType")]
        public DeliveryType DeliveryType { get; set; }

        /// <summary>
        /// Gets/sets a unique string defining the product.
        /// </summary>
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets/sets a short description of the product.
        /// </summary>
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets/sets the source reference data, or null if a complete delivery.
        /// </summary>
        [JsonPropertyName("deliverySource")]
        public DateTime? DeliverySource { get; set; }

        /// <summary>
        /// Gets/sets the target reference date.
        /// </summary>
        [JsonPropertyName("deliveryTarget")]
        public DateTime DeliveryTarget { get; set; }

        /// <summary>
        /// Gets/sets the unique url from which the delivery can be downloaded.
        /// </summary>
        [JsonPropertyName("downloadUrl")]
        public Uri DownloadUrl { get; set; }

        /// <summary>
        /// Gets/sets the number of times this delivery has been downloaded.
        /// </summary>
        [JsonPropertyName("downloads")]
        public int DownloadCount { get; set; }
    }
}
