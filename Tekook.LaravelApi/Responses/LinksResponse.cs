using Newtonsoft.Json;
using System;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// Links response provided by laravel pagination.
    /// </summary>
    public class LinksResponse
    {
        /// <summary>
        /// Link to the first page of the collection.
        /// </summary>
        [JsonProperty("first")]
        public Uri First { get; set; }

        /// <summary>
        /// Link to the last page of the collection.
        /// </summary>
        [JsonProperty("last")]
        public Uri Last { get; set; }

        /// <summary>
        /// Link to the next page of the collection.
        /// </summary>
        [JsonProperty("next")]
        public Uri Next { get; set; }

        /// <summary>
        /// Link to the previous page of the collection.
        /// </summary>
        [JsonProperty("prev")]
        public Uri Prev { get; set; }
    }
}