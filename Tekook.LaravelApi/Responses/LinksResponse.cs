using Newtonsoft.Json;
using System;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// Default implementation of <see cref="ILinksResponse"/> with the default json properties set statically.
    /// </summary>
    public class LinksResponse : ILinksResponse
    {
        /// <inheritdoc/>
        [JsonProperty("first")]
        public Uri First { get; set; }

        /// <inheritdoc/>
        [JsonProperty("last")]
        public Uri Last { get; set; }

        /// <inheritdoc/>
        [JsonProperty("next")]
        public Uri Next { get; set; }

        /// <inheritdoc/>
        [JsonProperty("prev")]
        public Uri Prev { get; set; }
    }
}