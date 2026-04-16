using System;
using System.Net.Http;

namespace LabelzoomDotnetSdk
{
    /// <summary>
    /// Configuration options for the LabelzoomClient.
    /// </summary>
    public class LabelzoomClientOptions
    {
        /// <summary>
        /// Gets or sets the authentication token for the LabelZoom API.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the API endpoint URL.
        /// </summary>
        public string Endpoint { get; set; } = "https://api.labelzoom.net";

        /// <summary>
        /// Gets or sets the HTTP request timeout.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets or sets a custom HttpClient instance. If null, a new HttpClient will be created.
        /// </summary>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets whether the client should dispose the HttpClient when disposed.
        /// Set to false if you're providing your own HttpClient that you want to manage separately.
        /// </summary>
        public bool DisposeHttpClient { get; set; } = true;

        /// <summary>
        /// Creates a new instance of LabelzoomClientOptions with default values.
        /// </summary>
        public LabelzoomClientOptions()
        {
        }

        /// <summary>
        /// Creates a new instance of LabelzoomClientOptions with the specified token.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        public LabelzoomClientOptions(string token)
        {
            Token = token;
        }

        /// <summary>
        /// Validates the options and throws an exception if any required values are missing or invalid.
        /// </summary>
        internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                throw new InvalidOperationException("Token is required. Use WithToken() to set the authentication token.");
            }

            if (string.IsNullOrWhiteSpace(Endpoint))
            {
                throw new InvalidOperationException("Endpoint cannot be null or empty.");
            }
        }
    }
}

