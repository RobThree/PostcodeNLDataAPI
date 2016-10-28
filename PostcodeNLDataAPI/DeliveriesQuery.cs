using System;
using System.Collections.Specialized;

namespace PostcodeNLDataAPI
{
    public class DeliveriesQuery
    {
        public long? AccountId { get; set; }
        public DeliveryType? DeliveryType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateTime? After { get; set; }

        internal NameValueCollection AsNameValueCollection()
        {
            var nvc = new NameValueCollection();
            if (this.AccountId.HasValue)
                nvc["accountId"] = this.AccountId.Value.ToString();
            if (this.DeliveryType.HasValue)
                nvc["deliveryType"] = this.DeliveryType.Value.ToString().ToLowerInvariant();
            if (this.From.HasValue)
                nvc["from"] = this.From.Value.ToString(PostcodeNL.DATETIMEFORMAT);
            if (this.To.HasValue)
                nvc["to"] = this.To.Value.ToString(PostcodeNL.DATETIMEFORMAT);
            if (this.After.HasValue)
                nvc["after"] = this.After.Value.ToString(PostcodeNL.DATETIMEFORMAT);
            return nvc;
        }
    }
}
