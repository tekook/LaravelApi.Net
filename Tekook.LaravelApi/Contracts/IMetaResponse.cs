using System;

namespace Tekook.LaravelApi.Contracts
{
    /// <summary>
    /// Contract for the meta part of a laravel pagination response.
    /// </summary>
    public interface IMetaResponse
    {
        /// <summary>
        /// Current page we are on.
        /// </summary>
        long CurrentPage { get; set; }

        /// <summary>
        /// Defines the number of items this collection is starting from.
        /// </summary>
        long? From { get; set; }

        /// <summary>
        /// Last page for this collection.
        /// </summary>
        long LastPage { get; set; }

        /// <summary>
        /// Path of the request.
        /// </summary>
        Uri Path { get; set; }

        /// <summary>
        /// How many items per page.
        /// </summary>
        long PerPage { get; set; }

        /// <summary>
        /// Defines the number of items this collection goes to.
        /// </summary>
        long? To { get; set; }

        /// <summary>
        /// Total number of items in this collection.
        /// </summary>
        long Total { get; set; }
    }
}