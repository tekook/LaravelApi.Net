namespace Tekook.LaravelApi.Contracts
{
    /// <summary>
    /// Contracts that the object was made by an <see cref="LaravelApi.Api"/> request and holds it's creator in <see cref="Api"/>.
    /// </summary>
    public interface IHoldsApi
    {
        /// <summary>
        /// The <see cref="LaravelApi.Api"/> which created this object.
        /// </summary>
        Api Api { get; set; }
    }
}