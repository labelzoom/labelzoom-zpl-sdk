using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LabelzoomDotnetSdk.Conversion;

namespace LabelzoomDotnetSdk
{
    public class LabelzoomClient : IDisposable
    {
        private readonly HttpClient client;
        private readonly bool shouldDisposeHttpClient;
        private bool disposedValue;

        /// <summary>
        /// Creates a new instance of LabelzoomClient with the specified authentication token.
        /// </summary>
        /// <param name="token">The authentication token for the LabelZoom API.</param>
        public LabelzoomClient(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            }

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            shouldDisposeHttpClient = true;
        }

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

            Endpoint = options.Endpoint;

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

            if (!string.IsNullOrWhiteSpace(options.Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
            }
        }

        /// <summary>
        /// Gets or sets the API endpoint URL.
        /// </summary>
        public string Endpoint { get; set; } = "https://api.labelzoom.net";

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
        /// Converts a PDF document to a single ZPL string. Best used for smaller documents with fewer pages.
        /// </summary>
        /// <param name="pdfPath">Path to the PDF file.</param>
        /// <param name="ct">Optional cancellation token.</param>
        /// <returns>The complete ZPL string.</returns>
        public async Task<string> PdfToZpl(string pdfPath, CancellationToken ct = default)
        {
            using (var fileStream = File.OpenRead(pdfPath))
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{Endpoint}/api/v2/convert/pdf/to/zpl"))
            using (var content = new StreamContent(fileStream))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                request.Content = content;
                using (var response = await client.SendAsync(request, ct))
                {
                    response.EnsureSuccessStatusCode();
                    ct.ThrowIfCancellationRequested();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        /// <summary>
        /// Converts a PDF document to ZPL and streams the response one label at a time. Best used for larger documents with many pages.
        /// Each label returned will invoke the callback function <c>onLabelAsync</c>.
        /// </summary>
        /// <remarks>
        /// In future versions, we will use <c>IAsyncEnumerable</c> rather than a callback function.
        /// </remarks>
        /// <param name="pdfPath">Path to the PDF file.</param>
        /// <param name="onLabelAsync">Callback function called once per label.</param>
        /// <param name="ct">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when onLabelAsync is null.</exception>
        public async Task PdfToZplAsync(string pdfPath, Func<string, Task> onLabelAsync, CancellationToken ct = default)
        {
            if (onLabelAsync == null) throw new ArgumentNullException(nameof(onLabelAsync));

            using (var fileStream = File.OpenRead(pdfPath))
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{Endpoint}/api/v2.5/convert/pdf/to/zpl"))
            using (var content = new StreamContent(fileStream))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                request.Content = content;
                using (var response = await client.SendAsync(request, ct))
                {
                    response.EnsureSuccessStatusCode();
                    ct.ThrowIfCancellationRequested();

                    var stream = await response.Content.ReadAsStreamAsync();

                    using (var reader = new StreamReader(stream, Encoding.ASCII, detectEncodingFromByteOrderMarks: false, bufferSize: 16 * 1024, leaveOpen: true))
                    {
                        while (true)
                        {
                            ct.ThrowIfCancellationRequested();

                            var line = await reader.ReadLineAsync().ConfigureAwait(false);
                            if (line == null) break; // EOF

                            if (line.Length == 0) continue; // ignore empty lines

                            await onLabelAsync(line).ConfigureAwait(false);
                        }
                    }
                }
            }
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
