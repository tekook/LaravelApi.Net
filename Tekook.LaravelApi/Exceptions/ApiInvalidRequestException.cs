using System;
using System.Runtime.Serialization;
using Tekook.LaravelApi.Responses;

namespace Tekook.LaravelApi.Exceptions
{
    /// <summary>
    /// Exception representing an invalid request (422), containing <see cref="ErrorResponse.Errors"/>.
    /// </summary>
    [Serializable]
    public class ApiInvalidRequestException : ApiServerException
    {
        /// <inheritdoc/>
        public ApiInvalidRequestException(ErrorResponse error) : base(error)
        {
        }

        /// <inheritdoc/>
        protected ApiInvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}