using System;
using System.Net.Http;

namespace PostcodeNLDataAPI
{
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
            : base(exceptionDetails.Exception, innerException)
        {
            this.ExceptionId = exceptionDetails.ExceptionId;
            this.RequestId = exceptionDetails.RequestId;
            this.HttpResponseMessage = httpResponseMessage;
            this.Uri = uri;
        }
    }
}
