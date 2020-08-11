using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// Collection response for multiple items of type T.
    /// </summary>
    /// <typeparam name="T">Type of the resource this collection containes.</typeparam>
    public class CollectionResponse<T> where T : Resources.Resource
    {
        /// <summary>
        /// The data of the collection containing the resources.
        /// </summary>
        [JsonProperty("data")]
        public IList<T> Data { get; set; }

        /// <summary>
        /// Links provided by laravel-pagination.
        /// </summary>
        [JsonProperty("links")]
        public LinksResponse Links { get; set; }

        /// <summary>
        /// Meta provied by laravel-pagination.
        /// </summary>
        [JsonProperty("meta")]
        public MetaResponse Meta { get; set; }

        #region Methods

        /// <summary>
        /// Determinates if there is more Data pending.
        /// </summary>
        public bool DataPending => this.Links?.Next != null;

        /// <summary>
        /// Iterates overall pages of the resource and calls the func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        public async Task Chunk(Api api, Action<CollectionResponse<T>> func, object queryParams = null)
        {
            var collection = this;
            func(collection);
            while (collection.DataPending)
            {
                collection = await collection.GetNext(api, queryParams);
                func(collection);
            }
        }

        /// <summary>
        /// Iterates overall pages of the resource and calls the async func with the corresponding <see cref="CollectionResponse{T}"/>.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to use for calls.</param>
        /// <param name="func">Function to call for each chunk.</param>
        /// <param name="queryParams">Optional query paramters to use for each call.</param>
        /// <returns></returns>
        public async Task Chunk(Api api, Func<CollectionResponse<T>, Task> func, object queryParams = null)
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