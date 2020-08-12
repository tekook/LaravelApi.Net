using Newtonsoft.Json;
using Tekook.LaravelApi.Contracts;
using Tekook.LaravelApi.Resources;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// All responses contain the <see cref="Data"/> attribute.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Data"/></typeparam>
    public class Response<T> : IApiResponse where T : Resource
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