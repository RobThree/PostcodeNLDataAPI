using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PostcodeNLDataAPI.Tests
{
    public class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _FakeResponses = new Dictionary<Uri, HttpResponseMessage>();


        public FakeResponseHandler AddResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _FakeResponses.Add(uri, responseMessage);
            return this;
        }

        public FakeResponseHandler AddEmptyResponse(Uri uri, HttpStatusCode httpStatusCode)
        {
            _FakeResponses.Add(uri, new HttpResponseMessage(httpStatusCode));
            return this;
        }


        public FakeResponseHandler AddJsonResponse(Uri uri, string json)
        {
            return AddJsonResponse(uri, json, HttpStatusCode.OK);
        }

        public FakeResponseHandler AddJsonResponse(Uri uri, string json, HttpStatusCode httpStatusCode)
        {
            var responsemsg = new HttpResponseMessage(httpStatusCode);
            responsemsg.Content = new StringContent(json, Encoding.UTF8, "application/json");
            _FakeResponses.Add(uri, responsemsg);
            return this;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (_FakeResponses.ContainsKey(request.RequestUri))
            {
                var response = _FakeResponses[request.RequestUri];
                response.RequestMessage = request;
                return Task.FromResult(response);
            }
            else
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }

        }
    }
}
