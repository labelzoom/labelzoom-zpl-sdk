using System;
using System.Net.Http;
using System.Net.Http.Headers;
using LabelzoomDotnetSdk.Conversion;

namespace LabelzoomDotnetSdk
{
    public class LabelzoomClient : IDisposable
    {
        private readonly HttpClient client;
        private readonly bool shouldDisposeHttpClient;
        private bool disposedValue;

        /// <summary>
        /// Internal constructor used by LabelzoomClientBuilder.
        /// </summary>
        /// <param name="options">Configuration options for the client.</param>
        internal LabelzoomClient(LabelzoomClientOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.HttpClient != null)
            {
                client = options.HttpClient;
                shouldDisposeHttpClient = options.DisposeHttpClient;
            }
            else
            {
                client = new HttpClient();
                shouldDisposeHttpClient = true;
            }

            if (options.Timeout.HasValue)
            {
                client.Timeout = options.Timeout.Value;
            }

            client.BaseAddress = new Uri(options.Endpoint);

            if (!string.IsNullOrWhiteSpace(options.Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
            }
        }

        /// <summary>
        /// Gets the HttpClient used for API requests. Internal use only.
        /// </summary>
        internal HttpClient GetHttpClient() => client;

        /// <summary>
        /// Creates a fluent conversion request builder.
        /// </summary>
        /// <returns>A conversion request builder for specifying source and target formats.</returns>
        /// <example>
        /// <code>
        /// var zpl = await client.Convert().FromPdf("document.pdf").ToZpl().ExecuteAsync();
        /// </code>
        /// </example>
        public ConversionRequestBuilder Convert()
        {
            return new ConversionRequestBuilder(this);
        }

        /// <summary>
        /// Disposes the LabelzoomClient and releases resources.
        /// </summary>
        /// <param name="disposing">True if disposing managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (shouldDisposeHttpClient && client != null)
                    {
                        client.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes the LabelzoomClient and releases resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
