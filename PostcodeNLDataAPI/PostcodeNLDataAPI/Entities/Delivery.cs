using Newtonsoft.Json;
using System;

namespace PostcodeNLDataAPI.Entities
{
    public class Delivery
    {
        [JsonProperty(PropertyName = "deliveryId")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "accountId")]
        public long AccountId { get; set; }
        [JsonProperty(PropertyName = "deliveryType")]
        public DeliveryType DeliveryType { get; set; }

        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }
        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }
        [JsonProperty(PropertyName = "deliverySource")]
        public DateTime? DeliverySource { get; set; }
        [JsonProperty(PropertyName = "deliveryTarget")]
        public DateTime DeliveryTarget { get; set; }
        [JsonProperty(PropertyName = "downloadUrl")]
        public Uri DownloadUrl { get; set; }
        [JsonProperty(PropertyName = "downloads")]
        public int DownloadCount { get; set; }
    }
}
