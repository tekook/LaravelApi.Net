﻿using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// Default implementation of <see cref="ICollectionResponse{T}"/> with the default json properties set statically.
    /// Provides multiple methods for working with the results.
    /// </summary>
    /// <typeparam name="T">Type of the resource this collection containes.</typeparam>
    public class CollectionResponse<T> : IHoldsApi, ICollectionResponse<T> where T : IResource
    {
        #region Properties

        /// <inheritdoc/>
        public Api Api { get; set; }

        /// <inheritdoc/>
        [JsonProperty("data")]
        public IList<T> Data { get; set; }

        /// <inheritdoc/>
        public bool DataPending => this.Links?.Next != null;

        /// <inheritdoc/>
        [JsonProperty("links")]
        public ILinksResponse Links { get; set; } = new LinksResponse();

        /// <inheritdoc/>
        [JsonProperty("meta")]
        public IMetaResponse Meta { get; set; } = new MetaResponse();

        #endregion Properties

        #region Chunk

        /// <summary>
        /// Iterates overall pages of the resource and calls the func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        [Obsolete("Please use Chunk(Action{CollectionResponse{T}}, object) instead.")]
        public async Task Chunk(Api api, Action<CollectionResponse<T>> func, object queryParams = null)
        {
            await ChunkViaApi(api, func, queryParams);
        }

        /// <summary>
        /// Iterates overall pages of the resource and calls the func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        public async Task Chunk(Action<CollectionResponse<T>> func, object queryParams = null)
        {
            await this.ChunkViaApi(this.Api, func, queryParams);
        }

        /// <summary>
        /// Iterates overall pages of the resource and calls the async func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        [Obsolete("Please use Chunk(Func{CollectionResponse{T}, Task}, object) instead.")]
        public async Task Chunk(Api api, Func<CollectionResponse<T>, Task> func, object queryParams = null)
        {
            await this.ChunkViaApi(api, func, queryParams);
        }

        /// <summary>
        /// Iterates overall pages of the resource and calls the async func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        public async Task Chunk(Func<CollectionResponse<T>, Task> func, object queryParams = null)
        {
            await this.ChunkViaApi(this.Api, func, queryParams);
        }

        /// <summary>
        /// Iterates overall pages of the resource and calls the async func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        protected async Task ChunkViaApi(Api api, Func<CollectionResponse<T>, Task> func, object queryParams = null)
        {
            var collection = this;
            await func(collection);
            while (collection.DataPending)
            {
                collection = await collection.GetNext(api, queryParams);
                await func(collection);
            }
        }

        /// <summary>
        /// Iterates overall pages of the resource and calls the async func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        protected async Task ChunkViaApi(Api api, Action<CollectionResponse<T>> func, object queryParams = null)
        {
            var collection = this;
            func(collection);
            while (collection.DataPending)
            {
                collection = await collection.GetNext(api, queryParams);
                func(collection);
            }
        }

        #endregion Chunk

        #region FetchAll

        /// <summary>
        /// Fetches all pending data from the api and integrated it in the current collection.
        /// Use with caution, this could take forever!
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="queryParams">Optional query parameters to use for each call.</param>
        /// <returns>The Collection itself.</returns>
        public async Task<CollectionResponse<T>> FetchAll(Api api, object queryParams = null)
        {
            if (!this.DataPending)
            {
                return this;
            }
            while (this.DataPending)
            {
                var request = await this.GetNext(api, queryParams);
                foreach (T model in request.Data)
                {
                    this.Data.Add(model);
                }
                this.Links = request.Links;
                this.Meta = request.Meta;
            }
            return this;
        }

        /// <summary>
        /// Fetches all pending data from the api and integrated it in the current collection.
        /// Use with caution, this could take forever!
        /// </summary>
        /// <param name="queryParams">Optional query parameters to use for each call.</param>
        /// <returns>The Collection itself.</returns>
        public async Task<CollectionResponse<T>> FetchAll(object queryParams = null)
        {
            return await this.FetchAll(this.Api, queryParams);
        }

        #endregion FetchAll

        #region Methods

        /// <summary>
        /// Gets the next page of the resource.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="queryParams">Optional query parameters to use for each call.</param>
        /// <returns>Null if there are no more pages.</returns>
        protected async Task<CollectionResponse<T>> GetNext(Api api, object queryParams)
        {
            if (!this.DataPending)
            {
                return null;
            }
            return await api.GetRequest(this.Links.Next.AbsoluteUri)
                                .WithOAuthBearerToken(api.AccessToken)
                                .SetQueryParams(queryParams)
                                .GetAsync()
                                .ReceiveJson<CollectionResponse<T>>();
        }

        #endregion Methods
    }
}