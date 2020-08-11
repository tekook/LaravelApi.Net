using Newtonsoft.Json;
using System;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// Meta response provided by laravel pagination.
    /// </summary>
    public class MetaResponse
    {
        /// <summary>
        /// Current page we are on.
        /// </summary>
        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }

        /// <summary>
        /// Defines the number of items this collection is starting from.
        /// </summary>
        [JsonProperty("from")]
        public long? From { get; set; }

        /// <summary>
        /// Last page for this collection.
        /// </summary>
        [JsonProperty("last_page")]
        public long LastPage { get; set; }

        /// <summary>
        /// Path of the request.
        /// </summary>
        [JsonProperty("path")]
        public Uri Path { get; set; }

        /// <summary>
        /// How many items per page.
        /// </summary>
        [JsonProperty("per_page")]
        public long PerPage { get; set; }

        /// <summary>
        /// Defines the number of items this collection goes to.
        /// </summary>
        [JsonProperty("to")]
        public long? To { get; set; }

        /// <summary>
        /// Total number of items in this collection.
        /// </summary>
        [JsonProperty("total")]
        public long Total { get; set; }
    }
}