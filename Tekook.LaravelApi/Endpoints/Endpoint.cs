using Flurl.Http;

namespace Tekook.LaravelApi.Endpoints
{
    /// <summary>
    /// Base class for all endpoints.
    /// </summary>
    public abstract class Endpoint
    {
        /// <summary>
        /// The <see cref="LaravelApi.Api"/> this endpoint belongs to.
        /// </summary>
        public Api Api { get; set; }

        /// <summary>
        /// The resource path of this endpoint. (Can be empty/null)
        /// </summary>
        protected string Path { get; set; }

        /// <summary>
        /// Creates a new Endpoint bound to the specified api.
        /// </summary>
        /// <param name="api"><see cref="LaravelApi.Api"/> to bind this endpoint to.</param>
        public Endpoint(Api api) => this.Api = api;

        /// <summary>
        /// Provides an authorized request for this endpoint (and its <see cref="Path"/>).
        /// </summary>
        /// <returns>The (already) authorized request.</returns>
        protected IFlurlRequest AuthedRequest() => this.Api.AddAccessAuthorization(this.Request());

        /// <summary>
        /// Provides an request for this endpoint (and its <see cref="Path"/>).
        /// </summary>
        /// <returns></returns>
        protected virtual IFlurlRequest Request() => this.Api.BaseRequest().AppendPathSegment(this.Path);
    }
}