using Newtonsoft.Json;
using System;

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
        [JsonProperty(PropertyName = "accountId")]
        public long Id { get; set; }

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
        /// Gets/sets the starting reference data of the subscription.
        /// </summary>
        [JsonProperty(PropertyName = "periodBegin")]
        public DateTime SubscriptionStart { get; set; }

        /// <summary>
        /// Gets/sets the ending reference data of the subscription.
        /// </summary>
        [JsonProperty(PropertyName = "periodEnd")]
        public DateTime SubscriptionEnd { get; set; }

        /// <summary>
        /// Gets/sets the last created 'complete' delivery, or null if none.
        /// </summary>
        [JsonProperty(PropertyName = "lastDeliveryComplete")]
        public DateTime? LastDeliveryComplete { get; set; }
        
        /// <summary>
        /// Gets/sets the last created 'mutation' delivery, or null if none.
        /// </summary>
        [JsonProperty(PropertyName = "lastDeliveryMutation")]
        public DateTime? LastDeliveryMutation { get; set; }

        /// <summary>
        /// Gets/sets the next scheduled 'complete' delivery, or null if none.
        /// </summary>
        [JsonProperty(PropertyName = "nextDeliveryComplete")]
        public DateTime? NextDeliveryComplete { get; set; }

        /// <summary>
        /// Gets/sets the next scheduled 'mutation' delivery, or null if none.
        /// </summary>
        [JsonProperty(PropertyName = "nextDeliveryMutation")]
        public DateTime? NextDeliveryMutation { get; set; }
    }
}
