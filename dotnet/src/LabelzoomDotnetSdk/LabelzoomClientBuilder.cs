using System;
using System.Net.Http;

namespace LabelzoomDotnetSdk
{
    /// <summary>
    /// Builder for creating configured instances of LabelzoomClient.
    /// </summary>
    public class LabelzoomClientBuilder
    {
        private readonly LabelzoomClientOptions options;

        /// <summary>
        /// Creates a new instance of LabelzoomClientBuilder.
        /// </summary>
        public LabelzoomClientBuilder()
        {
            options = new LabelzoomClientOptions();
        }

        /// <summary>
        /// Sets the authentication token for the LabelZoom API.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public LabelzoomClientBuilder WithToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            }

            options.Token = token;
            return this;
        }

        /// <summary>
        /// Sets a custom API endpoint URL.
        /// </summary>
        /// <param name="endpoint">The API endpoint URL.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public LabelzoomClientBuilder WithEndpoint(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));
            }

            options.Endpoint = endpoint;
            return this;
        }

        /// <summary>
        /// Sets the HTTP request timeout.
        /// </summary>
        /// <param name="timeout">The timeout duration.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public LabelzoomClientBuilder WithTimeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
            {
                throw new ArgumentException("Timeout must be greater than zero.", nameof(timeout));
            }

            options.Timeout = timeout;
            return this;
        }

        /// <summary>
        /// Provides a custom HttpClient instance to use for API requests.
        /// When using this option, you are responsible for managing the HttpClient lifecycle.
        /// The LabelzoomClient will not dispose the HttpClient unless you call WithDisposeHttpClient(true).
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to use.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public LabelzoomClientBuilder WithHttpClient(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            options.HttpClient = httpClient;
            options.DisposeHttpClient = false; // By default, don't dispose user-provided HttpClient
            return this;
        }

        /// <summary>
        /// Sets whether the LabelzoomClient should dispose the HttpClient when disposed.
        /// Only relevant when using WithHttpClient().
        /// </summary>
        /// <param name="dispose">True to dispose the HttpClient, false to leave it managed externally.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public LabelzoomClientBuilder WithDisposeHttpClient(bool dispose)
        {
            options.DisposeHttpClient = dispose;
            return this;
        }

        /// <summary>
        /// Builds and returns a configured LabelzoomClient instance.
        /// </summary>
        /// <returns>A new LabelzoomClient instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required configuration is missing.</exception>
        public LabelzoomClient Build()
        {
            options.Validate();
            return new LabelzoomClient(options);
        }
    }
}

