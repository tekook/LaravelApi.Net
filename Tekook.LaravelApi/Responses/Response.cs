using Newtonsoft.Json;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// Default implementation of <see cref="IResponse{T}"/> with the default json properties set statically.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Data"/></typeparam>
    public class Response<T> : IHoldsApi, IResponse<T> where T : IResource
    {
        /// <inheritdoc/>
        public Api Api { get; set; }

        /// <inheritdoc/>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}