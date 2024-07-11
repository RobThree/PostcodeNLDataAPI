using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PostcodeNLDataAPI.Tests;

public class FakeResponseHandler : DelegatingHandler
{
    private readonly Dictionary<Uri, HttpResponseMessage> _fakeresponses = [];


    public FakeResponseHandler AddResponse(Uri uri, HttpResponseMessage responseMessage)
    {
        _fakeresponses.Add(uri, responseMessage);
        return this;
    }

    public FakeResponseHandler AddEmptyResponse(Uri uri, HttpStatusCode httpStatusCode)
    {
        _fakeresponses.Add(uri, new HttpResponseMessage(httpStatusCode));
        return this;
    }


    public FakeResponseHandler AddJsonResponse(Uri uri, string json) => AddJsonResponse(uri, json, HttpStatusCode.OK);

    public FakeResponseHandler AddJsonResponse(Uri uri, string json, HttpStatusCode httpStatusCode)
    {
        var responsemsg = new HttpResponseMessage(httpStatusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        _fakeresponses.Add(uri, responsemsg);
        return this;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        if (_fakeresponses.TryGetValue(request.RequestUri, out var value))
        {
            var response = value;
            response.RequestMessage = request;
            return Task.FromResult(response);
        }
        else
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
        }

    }
}
