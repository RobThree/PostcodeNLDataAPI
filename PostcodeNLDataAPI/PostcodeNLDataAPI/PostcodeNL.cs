using Newtonsoft.Json;
using PostcodeNLDataAPI.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PostcodeNLDataAPI
{
    public class PostcodeNL
    {
        public NetworkCredential Credentials { get; private set; }
        public Uri BaseUri { get; set; }
        public static readonly Uri DEFAULTURI = new Uri("https://data.postcode.nl/rest/");
        
        internal const string DATETIMEFORMAT = "yyyyMMdd";

        private static readonly JsonSerializerSettings _serializersettings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            DateFormatString = DATETIMEFORMAT,
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
            DateParseHandling = DateParseHandling.DateTime,
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };

        public PostcodeNL(string userName, string password)
            : this(new NetworkCredential(userName, password)) { }

        public PostcodeNL(string userName, string password, Uri baseUri)
            : this(new NetworkCredential(userName, password), baseUri) { }

        public PostcodeNL(NetworkCredential credentials)
            : this(credentials, DEFAULTURI) { }

        public PostcodeNL(NetworkCredential credentials, Uri baseUri)
        {
            this.Credentials = credentials;
            this.BaseUri = baseUri;
        }

        private async Task<T> DoRequest<T>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            using (var client = new HttpClient())
            {
                client.BaseAddress = this.BaseUri;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{this.Credentials.UserName}:{this.Credentials.Password}")));

                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(content, _serializersettings);
                }
                else
                {
                    ExceptionDetails exceptiondetails  = null;
                    // Try deserializing the exception details (if any)
                    try
                    {
                        exceptiondetails = JsonConvert.DeserializeObject<ExceptionDetails>(content, _serializersettings);
                    }
                    catch
                    {
                        // Well, that failed... Create a "generic" exception
                        exceptiondetails = new ExceptionDetails { Exception = "Error executing request" };
                    }
                    throw new PostcodeNLException(exceptiondetails, uri, response);
                }
            }
        }

        public Task<Account> GetAccountAsync(long accountId)
        {
            return DoRequest<Account>(BuildUri($"subscription/accounts/{accountId}"));
        }

        public Task<IEnumerable<Account>> ListAccountsAsync()
        {
            return DoRequest<IEnumerable<Account>>(BuildUri("subscription/accounts"));
        }

        public Task<Delivery> GetDeliveryAsync(string deliveryId)
        {
            if (string.IsNullOrEmpty(deliveryId))
                throw new ArgumentNullException(nameof(deliveryId));

            return DoRequest<Delivery>(BuildUri($"subscription/deliveries/{Uri.EscapeUriString(deliveryId)}"));
        }

        public Task<IEnumerable<Delivery>> ListDeliveriesAsync(DeliveriesQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            // Make sure we have at least one search-argument
            var nvc = query.AsNameValueCollection();
            if (nvc.Count == 0)
                throw new InvalidOperationException("No query arguments provided");

            return DoRequest<IEnumerable<Delivery>>(BuildUri("subscription/deliveries", nvc));
        }

        public Task DownloadDeliveryAsync(Delivery delivery, string fileName)
        {
            return DownloadFileAsync(delivery.DownloadUrl, fileName);
        }

        public Task DownloadFileAsync(Uri uri, string fileName)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadFileTaskAsync(uri, fileName);
            }
        }

        private Uri BuildUri(string path, NameValueCollection query = null)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            Uri requesturi;
            if (!Uri.TryCreate(this.BaseUri, path, out requesturi))
                throw new PostcodeNLException(new ExceptionDetails { Exception = "Invalid URI" }, null, null);

            if (query != null && query.Count > 0)
            {
                var builder = new UriBuilder(requesturi);
                builder.Query = string.Join("&",
                        query.AllKeys.Where(key => !string.IsNullOrWhiteSpace(query[key]))
                            .Select(key => string.Join("&", query.GetValues(key).Select(val => string.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(val))))));
                requesturi = builder.Uri;
            }
            return requesturi;
        }
    }

    internal class ExceptionDetails
    {
        [JsonProperty(PropertyName = "exception")]
        public string Exception { get; set; }
        [JsonProperty(PropertyName = "exceptionId")]
        public string ExceptionId { get; set; }
        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }
    }
}
