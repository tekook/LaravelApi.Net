using Newtonsoft.Json;
using System;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// Default implementation of <see cref="IMetaResponse"/> with default json parameters set statically.
    /// </summary>
    public class MetaResponse : IMetaResponse
    {
        /// <inheritdoc/>
        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }

        /// <inheritdoc/>
        [JsonProperty("from")]
        public long? From { get; set; }

        /// <inheritdoc/>
        [JsonProperty("last_page")]
        public long LastPage { get; set; }

        /// <inheritdoc/>
        [JsonProperty("path")]
        public Uri Path { get; set; }

        /// <inheritdoc/>
        [JsonProperty("per_page")]
        public long PerPage { get; set; }

        /// <inheritdoc/>
        [JsonProperty("to")]
        public long? To { get; set; }

        /// <inheritdoc/>
        [JsonProperty("total")]
        public long Total { get; set; }
    }
}