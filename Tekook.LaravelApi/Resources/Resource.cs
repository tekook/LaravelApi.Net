using Tekook.LaravelApi.Resources.Validation;

namespace Tekook.LaravelApi.Resources
{
    /// <summary>
    /// Base class for all resources.
    /// </summary>
    public abstract class Resource : PropertyChangingModel
    {
        /// <summary>
        /// Method to get the value of the primary key of the resource.
        /// </summary>
        /// <returns>The value of the primary key of the resource.</returns>
        public abstract object GetPrimaryKeyValue();
    }
}