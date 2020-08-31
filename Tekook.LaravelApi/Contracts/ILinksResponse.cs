using System;

namespace Tekook.LaravelApi.Contracts
{
    /// <summary>
    /// Contract for the links part of an laravel pagination response.
    /// </summary>
    public interface ILinksResponse
    {
        /// <summary>
        /// Link to the first page of the collection.
        /// </summary>
        Uri First { get; set; }

        /// <summary>
        /// Link to the last page of the collection.
        /// </summary>
        Uri Last { get; set; }

        /// <summary>
        /// Link to the next page of the collection.
        /// </summary>
        Uri Next { get; set; }

        /// <summary>
        /// Link to the previous page of the collection.
        /// </summary>
        Uri Prev { get; set; }
    }
}