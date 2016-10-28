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
    /// <summary>
    /// Implementation of the Postcode.nl DATA API.
    /// </summary>
    public class PostcodeNL
    {
        /// <summary>
        /// The <see cref="NetworkCredential">Credentials</see> to use.
        /// </summary>
        public NetworkCredential Credentials { get; private set; }

        /// <summary>
        /// The base URI for to use.
        /// </summary>
        public Uri BaseUri { get; private set; }

        /// <summary>
        /// The default URI to use.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PostcodeNL"/> class.
        /// </summary>
        /// <param name="userName">The username (or 'key') to use.</param>
        /// <param name="password">The password (or 'secret') to use.</param>
        public PostcodeNL(string userName, string password)
            : this(new NetworkCredential(userName, password)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostcodeNL"/> class.
        /// </summary>
        /// <param name="userName">The username (or 'key') to use.</param>
        /// <param name="password">The password (or 'secret') to use.</param>
        /// <param name="baseUri">The base URI to use.</param>
        public PostcodeNL(string userName, string password, Uri baseUri)
            : this(new NetworkCredential(userName, password), baseUri) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostcodeNL"/> class using the default base URI.
        /// </summary>
        /// <param name="credentials">The credentials to use.</param>
        public PostcodeNL(NetworkCredential credentials)
            : this(credentials, DEFAULTURI) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostcodeNL"/> class.
        /// </summary>
        /// <param name="credentials">The credentials to use.</param>
        /// <param name="baseUri">The base URI to use.</param>
        public PostcodeNL(NetworkCredential credentials, Uri baseUri)
        {
            this.Credentials = credentials;
            this.BaseUri = baseUri;
        }

        /// <summary>
        /// Executes the given request to postcode.nl and returns the desired type.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="uri">The URI to GET.</param>
        /// <returns>Returns the desired parsed result.</returns>
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

        /// <summary>
        /// Retrieves the specified subscription account.
        /// </summary>
        /// <param name="accountId">Identifier of the subscription account to return.</param>
        /// <returns>Returns the specified subscription account.</returns>
        public Task<Account> GetAccountAsync(long accountId)
        {
            return DoRequest<Account>(BuildUri($"subscription/accounts/{accountId}"));
        }


        /// <summary>
        /// Retrieves the subscription accounts available to the client; optionally filtered by productcode.
        /// </summary>
        /// <param name="productCode">Productcode filter; return only products with this productcode.</param>
        /// <returns>Returns the subscription accounts available to the client.</returns>
        public Task<IEnumerable<Account>> ListAccountsAsync(string productCode = null)
        {
            var nvc = new NameValueCollection();
            if (productCode != null)
                nvc["productCode"] = productCode;

            return DoRequest<IEnumerable<Account>>(BuildUri("subscription/accounts", nvc));
        }


        /// <summary>
        /// Retrieves the specified delivery.
        /// </summary>
        /// <param name="deliveryId">Identifier of the delivery to return.</param>
        /// <returns>Returns the specified delivery.</returns>
        public Task<Delivery> GetDeliveryAsync(string deliveryId)
        {
            if (string.IsNullOrEmpty(deliveryId))
                throw new ArgumentNullException(nameof(deliveryId));

            return DoRequest<Delivery>(BuildUri($"subscription/deliveries/{Uri.EscapeUriString(deliveryId)}"));
        }

        /// <summary>
        /// Retrieves a list of all deliveries based on a <see cref="DeliveriesQuery"/>.
        /// </summary>
        /// <param name="query">The filtering options to use when retrieving the deliveries.</param>
        /// <returns>Returns the deliveries filtered by the given <see cref="DeliveriesQuery"/>.</returns>
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

        /// <summary>
        /// Downloads the specified <see cref="Delivery"/> to a local file as an asynchronous operation using a task object.
        /// </summary>
        /// <param name="delivery">The delivery to download.</param>
        /// <param name="fileName">The name of the file to be placed on the local computer.</param>
        /// <returns>Returns <see cref="Task"/>. The task object representing the asynchronous operation.</returns>
        public Task DownloadDeliveryAsync(Delivery delivery, string fileName)
        {
            return DownloadFileAsync(delivery.DownloadUrl, fileName);
        }

        /// <summary>
        /// Downloads the specified resource to a local file as an asynchronous operation using a task object.
        /// </summary>
        /// <param name="uri">The URI of the resource to download.</param>
        /// <param name="fileName">The name of the file to be placed on the local computer.</param>
        /// <returns>Returns <see cref="Task"/>. The task object representing the asynchronous operation.</returns>
        public Task DownloadFileAsync(Uri uri, string fileName)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadFileTaskAsync(uri, fileName);
            }
        }

        /// <summary>
        /// Builds a uri based on this instance's Base Uri and a given (relative) path with optional querystring arguments.
        /// </summary>
        /// <param name="path">The relative path.</param>
        /// <param name="query">Querystring arguments (key/value pairs).</param>
        /// <returns>Returns a composed ("built") uri.</returns>
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

    /// <summary>
    /// For internal use only; used to deserialize exception details from the 'backend' (e.g. postcode.nl) into a <see cref="PostcodeNLException"/>.
    /// </summary>
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
