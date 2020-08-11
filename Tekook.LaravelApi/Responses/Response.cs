using Newtonsoft.Json;
using Tekook.LaravelApi.Resources;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// All responses contain the <see cref="Data"/> attribute.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Data"/></typeparam>
    public class Response<T> where T : Resource
    {
        /// <summary>
        /// Data of the response
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}