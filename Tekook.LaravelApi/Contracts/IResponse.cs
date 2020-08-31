namespace Tekook.LaravelApi.Contracts
{
    /// <summary>
    /// Contract for a simple laravel-response providing a single resource.
    /// </summary>
    /// <typeparam name="T">The resource this response holds.</typeparam>
    public interface IResponse<T> where T : IResource
    {
        /// <summary>
        /// Data of the response
        /// </summary>
        T Data { get; set; }
    }
}