using System;
using System.Net.Http;

namespace PostcodeNLDataAPI;

/// <summary>
/// 
/// </summary>
public class PostcodeNLException : Exception
{
    /// <summary>
    /// Gets the exception identifier to categorize the error.
    /// </summary>
    public string ExceptionId { get; private set; }
    /// <summary>
    /// Gets the identifier used to describe the request the error occured in.
    /// </summary>
    public string RequestId { get; private set; }
    /// <summary>
    /// Gets the original <see cref="HttpRequestMessage"/> (if any) that caused the exception.
    /// </summary>
    public HttpResponseMessage HttpResponseMessage { get; private set; }
    /// <summary>
    /// Gets the original <see cref="Uri"/> (if any) that caused the exception.
    /// </summary>
    public Uri Uri { get; private set; }


    internal PostcodeNLException(ExceptionDetails exceptionDetails, Uri uri, HttpResponseMessage httpResponseMessage)
        : this(exceptionDetails, uri, httpResponseMessage, null) { }

    internal PostcodeNLException(ExceptionDetails exceptionDetails, Uri uri, HttpResponseMessage httpResponseMessage, Exception innerException)
        : this(exceptionDetails.Exception, innerException)
    {
        ExceptionId = exceptionDetails.ExceptionId;
        RequestId = exceptionDetails.RequestId;
        HttpResponseMessage = httpResponseMessage;
        Uri = uri;
    }

    internal PostcodeNLException() : base() { }

    internal PostcodeNLException(string message) : base(message) { }

    internal PostcodeNLException(string message, Exception innerException) : base(message, innerException) { }
}
