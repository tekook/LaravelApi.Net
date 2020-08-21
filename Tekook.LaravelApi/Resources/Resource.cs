using System;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi.Resources
{
    /// <summary>
    /// Base class for all resources.
    /// </summary>
    [Obsolete(nameof(Resource) + " is obsolete, please use " + nameof(IResource) + " or " + nameof(IIndexedResource) + " instead.")]
    public abstract class Resource : IIndexedResource
    {
        /// <inheritdoc/>
        public object PrimaryKeyValue => this.GetPrimaryKeyValue();

        /// <summary>
        /// Method to get the value of the primary key of the resource.
        /// </summary>
        /// <returns>The value of the primary key of the resource.</returns>
        [Obsolete(nameof(GetPrimaryKeyValue) + " is obsolete. Implement " + nameof(IIndexedResource) + " instead")]
        public abstract object GetPrimaryKeyValue();
    }
}