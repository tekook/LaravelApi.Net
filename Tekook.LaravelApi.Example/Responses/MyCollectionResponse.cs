using System;
using System.Collections.Generic;
using Tekook.LaravelApi.Contracts;
using Tekook.LaravelApi.Responses;

namespace Tekook.LaravelApi.Example.Responses
{
    internal class MyCollectionResponse<T> : ICollectionResponse<T> where T : IResource
    {
        public int Count => this.Data.Count;
        public IList<T> Data { get; set; }

        public bool DataPending => throw new NotImplementedException();

        /// <summary>
        /// Important to provide an instance otherwise json deserialization cannot work.
        /// </summary>
        public ILinksResponse Links { get; set; } = new LinksResponse();

        /// <summary>
        /// Important to provide an instance otherwise json deserialization cannot work.
        /// </summary>
        public IMetaResponse Meta { get; set; } = new MetaResponse();
    }
}