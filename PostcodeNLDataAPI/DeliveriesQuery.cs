using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PostcodeNLDataAPI
{
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
            if (this.AccountId.HasValue)
                d.Add("accountId", this.AccountId.Value.ToString());
            if (this.DeliveryType.HasValue)
                d.Add("deliveryType", this.DeliveryType.Value.ToString().ToLowerInvariant());
            if (this.From.HasValue)
                d.Add("from", this.From.Value.ToString(PostcodeNL.DATETIMEFORMAT));
            if (this.To.HasValue)
                d.Add("to", this.To.Value.ToString(PostcodeNL.DATETIMEFORMAT));
            if (this.After.HasValue)
                d.Add("after", this.After.Value.ToString(PostcodeNL.DATETIMEFORMAT));
            return d;
        }
    }
}
