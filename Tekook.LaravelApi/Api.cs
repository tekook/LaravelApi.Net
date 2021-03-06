﻿using Flurl.Http;
using System;
using System.Threading.Tasks;
using Tekook.LaravelApi.Contracts;
using Tekook.LaravelApi.Exceptions;
using Tekook.LaravelApi.Responses;

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
        /// Header to send which each request via the "Accept" parameter.
        /// </summary>
        public string AcceptHeader { get; set; } = "application/json";

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
        /// Header to send with each request via the "Accept-Language" parameter.
        /// </summary>
        public string LanguageHeader { get; set; }

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
            return url.WithHeaders(new { Accept = this.AcceptHeader, User_Agent = this.ClientIdentifier, Accept_Language = this.LanguageHeader });
        }

        #endregion Methods

        #region Api Methods

        /// <summary>
        /// Will be called each time a request was succesfully made to the api.
        /// </summary>
        /// <param name="sender">The api which made the request.</param>
        /// <param name="result">The result returned from the Api.</param>
        public delegate void ResultReceived(Api sender, object result);

        /// <summary>
        /// Will be called each time a request was succesfully made to the api.
        /// </summary>
        public event ResultReceived OnRequestReceived;

        /// <summary>
        /// Wrap around API calls to ensure the <see cref="Api"/> instance is injected into <see cref="IHoldsApi.Api"/>.
        /// </summary>
        /// <typeparam name="T">Any object your function returns.</typeparam>
        /// <param name="func">The function which results in the api response</param>
        /// <returns>async task result of your request.</returns>
        /// <exception cref="ApiException">Generic Exception thrown if the error is not identified.</exception>
        /// <exception cref="ApiTimeoutException">Thrown when the request timed out.</exception>
        /// <exception cref="ApiServerException">Thrown when the laravel api returned an readable error.</exception>
        /// <exception cref="ApiInvalidRequestException">Thrown when laravel returned status-code 422, stating an invalid request.</exception>
        public async Task<T> Wrap<T>(Func<Task<T>> func)
        {
            try
            {
                T result = await func.Invoke();
                if (result is IHoldsApi apiResponse)
                {
                    apiResponse.Api = this;
                }
                this.OnRequestReceived?.Invoke(this, result);
                return result;
            }
            catch (FlurlHttpTimeoutException e)
            {
                throw new ApiTimeoutException(e.Message, e);
            }
            catch (FlurlHttpException e)
            {
                ErrorResponse error = await ErrorResponse.FromException(e);
                if (error != null)
                {
                    if ((int)error.StatusCode == 422)
                    {
                        throw new ApiInvalidRequestException(error);
                    }
                    throw new ApiServerException(error);
                }
                throw new ApiException(e.Message, e);
            }
        }

        #endregion Api Methods
    }
}