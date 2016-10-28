using Newtonsoft.Json;
using System;

namespace PostcodeNLDataAPI.Entities
{
    public class Account
    {
        [JsonProperty(PropertyName = "accountId")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }
        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "periodBegin")]
        public DateTime SubscriptionStart { get; set; }
        [JsonProperty(PropertyName = "periodEnd")]
        public DateTime SubscriptionEnd { get; set; }

        [JsonProperty(PropertyName = "lastDeliveryComplete")]
        public DateTime? LastDeliveryComplete { get; set; }
        [JsonProperty(PropertyName = "lastDeliverMutation")]
        public DateTime? LastDeliveryMutation { get; set; }

        [JsonProperty(PropertyName = "nextDeliveryComplete")]
        public DateTime? NextDeliveryComplete { get; set; }
        [JsonProperty(PropertyName = "nextDeliverMutation")]
        public DateTime? NextDeliveryMutation { get; set; }
    }
}
