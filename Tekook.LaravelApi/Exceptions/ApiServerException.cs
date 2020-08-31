using System;
using System.Runtime.Serialization;
using Tekook.LaravelApi.Responses;

namespace Tekook.LaravelApi.Exceptions
{
    /// <summary>
    /// Exception represention an server generated error.
    /// It contains an <see cref="ErrorResponse"/> holding information about the error.
    /// Could be 404, 403, 422, 500 etc..
    /// </summary>
    [Serializable]
    public class ApiServerException : ApiException
    {
        /// <summary>
        /// The <see cref="ErrorResponse"/> of the request.
        /// </summary>
        public ErrorResponse Error { get; protected set; }

        /// <summary>
        /// Creates a new instance of <see cref="ApiServerException"/> with the specified <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="error">The specified error which bases the Exception.</param>
        public ApiServerException(ErrorResponse error) : base(error.Message, error.InnerException)
        {
            this.Error = error;
        }

        /// <inheritdoc/>
        protected ApiServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}