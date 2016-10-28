using System;
using System.Net.Http;

namespace PostcodeNLDataAPI
{
    public class PostcodeNLException : Exception
    {
        public string ExceptionId { get; private set; }
        public string RequestId { get; private set; }
        public HttpResponseMessage HttpResponseMessage { get; private set; }
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
