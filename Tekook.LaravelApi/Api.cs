using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace Tekook.LaravelApi
{
    /// <summary>
    /// Base class for all Apis extend and implement constructor for your own endpoints.
    /// Set <see cref="BaseUrl"/> and <see cref="ClientIdentifier"/> in constructor or elsewhere.
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
        public IFlurlRequest BaseRequest() => GetRequest(this.BaseUrl);

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
        /// Tries to invoke a function which should contain an api call.
        /// If the response is Unauthorized, we will try to get a new AccessToken and retry the function.
        /// </summary>
        /// <typeparam name="T">Any Object your <see cref="Func{T, TResult}"/> returns.</typeparam>
        /// <param name="func">The function which should be invoked.</param>
        /// <returns>async task result of your request.</returns>
        internal async Task<T> WrapCall<T>(Func<Task<T>> func)
        {
            try
            {
                return await func.Invoke();
            }
            catch (FlurlHttpException)
            {
                throw;
            }
        }

        #endregion Api Methods
    }
}