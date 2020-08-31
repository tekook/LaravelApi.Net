using Newtonsoft.Json;

namespace Tekook.LaravelApi.Contracts
{
    /// <summary>
    /// Describes a resource which is indexed via an primary key (e.g id) via <see cref="PrimaryKeyValue"/>.
    /// </summary>
    public interface IIndexedResource : IResource
    {
        /// <summary>
        /// The Value of the primary key for this model (e.g. the id)
        /// </summary>
        [JsonIgnore]
        object PrimaryKeyValue { get; }
    }
}