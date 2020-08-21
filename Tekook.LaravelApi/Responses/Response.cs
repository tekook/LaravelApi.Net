using Newtonsoft.Json;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// All responses contain the <see cref="Data"/> attribute.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Data"/></typeparam>
    public class Response<T> : IHoldsApi where T : IResource
    {
        /// <inheritdoc/>
        public Api Api { get; set; }

        /// <summary>
        /// Data of the response
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}