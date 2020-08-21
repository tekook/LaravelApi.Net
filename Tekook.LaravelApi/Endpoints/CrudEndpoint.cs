using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tekook.LaravelApi.Responses;

namespace Tekook.LaravelApi.Endpoints
{
    /// <summary>
    /// Default implementation of an Crud-supported-Endpoint
    /// </summary>
    /// <typeparam name="T">Type of the resource this Endpoint uses.</typeparam>
    public class CrudEndpoint<T> : Endpoint where T : Contracts.IIndexedResource
    {
        /// <summary>
        /// Creates a new Endpoint bound to the specified api and resource path.
        /// </summary>
        /// <param name="api"><see cref="Api"/> to bind this endpoint to.</param>
        /// <param name="path">Path this Endpoint resides.</param>
        public CrudEndpoint(Api api, string path) : base(api)
        {
            this.Path = path;
        }

        #region Methods

        /// <summary>
        /// Destroys an resource.
        /// </summary>
        /// <param name="resource">The resource to destroy.</param>
        /// <returns>Laravel does not provided data on destroy, thus only the normal <see cref="System.Net.Http.HttpResponseMessage"/> is provided.</returns>
        public async Task<System.Net.Http.HttpResponseMessage> Destroy(T resource)
        {
            return await this.Destroy(resource.PrimaryKeyValue);
        }

        /// <summary>
        /// Destroys an resource by primary key.
        /// </summary>
        /// <param name="pkey">The primary key of the resource.</param>
        /// <returns>Laravel does not provided data on destroy, thus only the normal <see cref="System.Net.Http.HttpResponseMessage"/> is provided.</returns>
        public async Task<System.Net.Http.HttpResponseMessage> Destroy(object pkey)
        {
            return await this.Api.Wrap(async () =>
            {
                return await this.AuthedRequest()
                .AppendPathSegment(pkey)
                .DeleteAsync();
            });
        }

        /// <summary>
        /// Indexes the resource.
        /// </summary>
        /// <param name="queryParams">Query parameters to supply to the request.</param>
        /// <returns>List of the resources.</returns>
        public async Task<IList<T>> Index(object queryParams = null)
        {
            return (await this.IndexResponse(queryParams)).Data;
        }

        /// <summary>
        /// Indexes the resource.
        /// </summary>
        /// <param name="queryParams">Query parameters to supply to the request.</param>
        /// <returns><see cref="CollectionResponse{T}"/> of the resource.</returns>
        public async Task<CollectionResponse<T>> IndexResponse(object queryParams = null)
        {
            return await this.Api.Wrap(async () =>
            {
                return await this.AuthedRequest()
                .SetQueryParams(queryParams)
                .GetAsync()
                .ReceiveJson<CollectionResponse<T>>();
            });
        }

        /// <summary>
        /// Retrieves a specific resource by primary key from the api.
        /// </summary>
        /// <param name="pkey">The primary key of the resource.</param>
        /// <returns></returns>
        public async Task<T> Show(object pkey)
        {
            return (await this.ShowResponse(pkey)).Data;
        }

        /// <summary>
        /// Retrieves a specific resource by an already fetched resource.
        /// </summary>
        /// <param name="resource">Model to "refetch".</param>
        /// <returns></returns>
        public async Task<T> Show(T resource)
        {
            return (await this.ShowResponse(resource)).Data;
        }

        /// <summary>
        /// Retrieves a <see cref="Response{T}"/> for a specific resource by an already fetched resource.
        /// </summary>
        /// <param name="resource">Model to "refetch"</param>
        /// <returns>Response of the resource.</returns>
        public async Task<Response<T>> ShowResponse(T resource)
        {
            return await this.ShowResponse(resource.PrimaryKeyValue);
        }

        /// <summary>
        /// Retrieves a <see cref="Response{T}"/> for a specific resource by an primary key.
        /// </summary>
        /// <param name="pkey">The primary key of the resource.</param>
        /// <returns></returns>
        public async Task<Response<T>> ShowResponse(object pkey)
        {
            return await this.Api.Wrap(async () =>
            {
                return await this.AuthedRequest()
                .AppendPathSegment(pkey)
                .GetAsync()
                .ReceiveJson<Response<T>>();
            });
        }

        /// <summary>
        /// Stores an new Model to the api.
        /// </summary>
        /// <param name="resource">The resource to store.</param>
        /// <returns>The newly stored resource from the api.</returns>
        public async Task<T> Store(T resource)
        {
            return (await this.StoreResponse(resource)).Data;
        }

        /// <summary>
        /// Stores a new Resource to the api.
        /// </summary>
        /// <param name="resource">The resource to store.</param>
        /// <returns>Response of the newly stored resource.</returns>
        public async Task<Response<T>> StoreResponse(T resource)
        {
            return await this.Api.Wrap(async () =>
            {
                return await this.AuthedRequest()
                .PostJsonAsync(resource)
                .ReceiveJson<Response<T>>();
            });
        }

        /// <summary>
        /// Updates a resource in the api to the local state.
        /// </summary>
        /// <param name="resource">The resource to update.</param>
        /// <returns>The updated resource.</returns>
        public async Task<T> Update(T resource)
        {
            return (await this.UpdateResponse(resource)).Data;
        }

        /// <summary>
        /// Updates a resource in the api to the local state.
        /// </summary>
        /// <param name="resource">The resource to update.</param>
        /// <returns>Response of the updated resource.</returns>
        public async Task<Response<T>> UpdateResponse(T resource)
        {
            return await this.Api.Wrap(async () =>
            {
                return await this.AuthedRequest()
                .AppendPathSegment(resource.PrimaryKeyValue)
                .PostJsonAsync(resource)
                .ReceiveJson<Response<T>>();
            });
        }

        #endregion Methods
    }
}