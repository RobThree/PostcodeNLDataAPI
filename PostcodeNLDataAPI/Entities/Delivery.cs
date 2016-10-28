using Newtonsoft.Json;
using System;

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
        [JsonProperty(PropertyName = "deliveryId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets/sets the identifier for the account this delivery belongs to.
        /// </summary>
        [JsonProperty(PropertyName = "accountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// <see cref="PostcodeNLDataAPI.DeliveryType"/>.
        /// </summary>
        [JsonProperty(PropertyName = "deliveryType")]
        public DeliveryType DeliveryType { get; set; }

        /// <summary>
        /// Gets/sets a unique string defining the product.
        /// </summary>
        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets/sets a short description of the product.
        /// </summary>
        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets/sets the source reference data, or null if a complete delivery.
        /// </summary>
        [JsonProperty(PropertyName = "deliverySource")]
        public DateTime? DeliverySource { get; set; }

        /// <summary>
        /// Gets/sets the target reference date.
        /// </summary>
        [JsonProperty(PropertyName = "deliveryTarget")]
        public DateTime DeliveryTarget { get; set; }

        /// <summary>
        /// Gets/sets the unique url from which the delivery can be downloaded.
        /// </summary>
        [JsonProperty(PropertyName = "downloadUrl")]
        public Uri DownloadUrl { get; set; }

        /// <summary>
        /// Gets/sets the number of times this delivery has been downloaded.
        /// </summary>
        [JsonProperty(PropertyName = "downloads")]
        public int DownloadCount { get; set; }
    }
}
