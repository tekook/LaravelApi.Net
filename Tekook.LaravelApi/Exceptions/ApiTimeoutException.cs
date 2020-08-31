using System;
using System.Runtime.Serialization;

namespace Tekook.LaravelApi.Exceptions
{
    /// <summary>
    /// Exception representating a timeout while processing a request.
    /// </summary>
    [Serializable]
    public class ApiTimeoutException : ApiException
    {
        /// <inheritdoc/>
        public ApiTimeoutException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public ApiTimeoutException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <inheritdoc/>
        protected ApiTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}