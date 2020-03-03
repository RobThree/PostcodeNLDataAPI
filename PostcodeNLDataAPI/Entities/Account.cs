using System;
using System.Text.Json.Serialization;

namespace PostcodeNLDataAPI.Entities
{
    /// <summary>
    /// Represents a Postcode.nl account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets/sets the identifier for this specific account.
        /// </summary>
        [JsonPropertyName("accountId")]
        public long Id { get; set; }

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
        /// Gets/sets the starting reference data of the subscription.
        /// </summary>
        [JsonPropertyName("periodBegin")]
        public DateTime SubscriptionStart { get; set; }

        /// <summary>
        /// Gets/sets the ending reference data of the subscription.
        /// </summary>
        [JsonPropertyName("periodEnd")]
        public DateTime SubscriptionEnd { get; set; }

        /// <summary>
        /// Gets/sets the last created 'complete' delivery, or null if none.
        /// </summary>
        [JsonPropertyName("lastDeliveryComplete")]
        public DateTime? LastDeliveryComplete { get; set; }

        /// <summary>
        /// Gets/sets the last created 'mutation' delivery, or null if none.
        /// </summary>
        [JsonPropertyName("lastDeliveryMutation")]
        public DateTime? LastDeliveryMutation { get; set; }

        /// <summary>
        /// Gets/sets the next scheduled 'complete' delivery, or null if none.
        /// </summary>
        [JsonPropertyName("nextDeliveryComplete")]
        public DateTime? NextDeliveryComplete { get; set; }

        /// <summary>
        /// Gets/sets the next scheduled 'mutation' delivery, or null if none.
        /// </summary>
        [JsonPropertyName("nextDeliveryMutation")]
        public DateTime? NextDeliveryMutation { get; set; }
    }
}
