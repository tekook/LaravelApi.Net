using System.Collections.Generic;

namespace Tekook.LaravelApi.Contracts
{
    /// <summary>
    /// Contract for an Collection-Response.
    /// </summary>
    /// <typeparam name="T">Type of resource this collection holds.</typeparam>
    public interface ICollectionResponse<T> where T : IResource
    {
        /// <summary>
        /// The data of the collection containing the resources.
        /// </summary>
        IList<T> Data { get; set; }

        /// <summary>
        /// Determinates if there is more Data pending.
        /// </summary>
        bool DataPending { get; }

        /// <summary>
        /// Links provided by laravel-pagination.
        /// </summary>
        ILinksResponse Links { get; set; }

        /// <summary>
        /// Meta provied by laravel-pagination.
        /// </summary>
        IMetaResponse Meta { get; set; }
    }
}