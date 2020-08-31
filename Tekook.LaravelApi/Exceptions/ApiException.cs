using Flurl.Http;
using System;
using System.Runtime.Serialization;

namespace Tekook.LaravelApi.Exceptions
{
    /// <summary>
    /// Base Exception of all errors representing an "unkown" error.
    /// </summary>
    [Serializable]
    public class ApiException : Exception
    {
        /// <summary>
        /// Url of the request (if available)
        /// </summary>
        public Uri Uri { get; protected set; }

        /// <inheritdoc/>
        public ApiException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public ApiException(string message, Exception inner) : base(message, inner)
        {
            if (inner is FlurlHttpException e)
            {
                this.Uri = e.Call?.FlurlRequest.Url.ToUri();
            }
        }

        /// <inheritdoc/>
        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}