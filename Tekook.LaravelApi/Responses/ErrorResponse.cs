using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlurlHttpException = Flurl.Http.FlurlHttpException;

namespace Tekook.LaravelApi.Responses
{
    /// <summary>
    /// ErrorResponse containting statically set json properties.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// <see cref="FlurlHttpException"/> which caused the error response.
        /// </summary>
        public FlurlHttpException InnerException { get; set; }

        #region Properties

        /// <summary>
        /// Containts the errors for each property.
        /// Only set on Status 422.
        /// </summary>
        [JsonProperty("errors")]
        public IDictionary<string, IList<string>> Errors { get; set; }

        /// <summary>
        /// FQDN of the exception which was thrown by laravel. (only available in debug mode of laravel)
        /// </summary>
        [JsonProperty("exception")]
        public string Exception { get; set; }

        /// <summary>
        /// File the exception has thrown. (only available in debug mode of laravel)
        /// </summary>
        [JsonProperty("file")]
        public string File { get; set; }

        /// <summary>
        /// Line of code the exception was thrown. (only available in debug mode of laravel)
        /// </summary>
        [JsonProperty("line")]
        public long Line { get; set; }

        /// <summary>
        /// Message returned by the api.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Status of the HttpRequest.
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates an <see cref="ErrorResponse"/> from n <see cref="FlurlHttpException"/>.
        /// </summary>
        /// <param name="exception">The Exception to create our response from.</param>
        /// <returns></returns>
        public async static Task<ErrorResponse> FromException(FlurlHttpException exception)
        {
            try
            {
                ErrorResponse error = await exception.GetResponseJsonAsync<ErrorResponse>();
                if (error != null)
                {
                    error.StatusCode = exception.Call.Response.StatusCode;
                    error.InnerException = exception;
                }
                return error;
            }
            catch
            {
                return null;
            }
        }

        #endregion Methods
    }
}