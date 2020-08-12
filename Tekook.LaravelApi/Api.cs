using Flurl.Http;
using System;
using System.Threading.Tasks;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi
{
    /// <summary>
    /// Base class for all Apis extend and implement constructor for your own endpoints.
    /// Make sure to set <see cref="BaseUrl"/> and <see cref="ClientIdentifier"/>.
    /// </summary>
    public class Api
    {
        #region Properties and construct

        private string accessToken;

        /// <summary>
        /// Access token to bear as authentification for most requests.
        /// </summary>
        public string AccessToken
        {
            get => accessToken; set
            {
                accessToken = value;
                OnAccessTokenChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Base of the Api and all requests.
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Client Identifier of all Api Calls.
        /// </summary>
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// RAsied when the <see cref="AccessToken"/> of the <see cref="Api"/> changes.
        /// </summary>
        /// <param name="sender">The api which got a new access token.</param>
        /// <param name="accessToken">The new <see cref="AccessToken"/>.</param>
        public delegate void AccessTokenChanged(Api sender, string accessToken);

        /// <summary>
        /// Raised when the <see cref="AccessToken"/> of the <see cref="Api"/> changes.
        /// </summary>
        public event AccessTokenChanged OnAccessTokenChanged;

        /// <summary>
        /// BaseRequest of the Api for all requests.
        /// </summary>
        public IFlurlRequest BaseRequest() => GetRequest(this.BaseUrl ?? throw new ArgumentNullException("BaseUrl", "BaseUrl ist not set, please provide"));

        #endregion Properties and construct

        #region Methods

        /// <summary>
        /// Adds <see cref="AccessToken"/> as OAuth to the Request.
        /// </summary>
        /// <param name="request">Request to add the token to.</param>
        /// <returns>Request to send</returns>
        public IFlurlRequest AddAccessAuthorization(IFlurlRequest request)
        {
            return request.WithOAuthBearerToken(this.AccessToken);
        }

        /// <summary>
        /// Gets a <see cref="IFlurlRequest"/> with default settings.
        /// </summary>
        /// <param name="url">Url to call.</param>
        /// <returns></returns>
        ///
        public IFlurlRequest GetRequest(Uri url)
        {
            return GetRequest(url.ToString());
        }

        /// <summary>
        /// Gets a <see cref="IFlurlRequest"/> with default settings.
        /// </summary>
        /// <param name="url">Url to call.</param>
        /// <returns></returns>
        public IFlurlRequest GetRequest(string url)
        {
            return url.WithHeaders(new { Accept = "application/json", User_Agent = this.ClientIdentifier });
        }

        #endregion Methods

        #region Api Methods

        /// <summary>
        /// Wrap around API calls to ensure the <see cref="Api"/> instance is injected into <see cref="IApiResponse.Api"/>.
        /// </summary>
        /// <typeparam name="T">Any object your function returns.</typeparam>
        /// <param name="func">The function which results in the api response</param>
        /// <returns>async task result of your request.</returns>
        public async Task<T> Wrap<T>(Func<Task<T>> func)
        {
            T result = await func.Invoke();
            if (result is IApiResponse apiResponse)
            {
                apiResponse.Api = this;
            }
            return result;
        }

        #endregion Api Methods
    }
}