using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for ZPL target conversions with execution methods.
    /// </summary>
    public class ZplTargetBuilder
    {
        private readonly LabelzoomClient client;
        private readonly string sourcePath;
        private readonly Stream sourceStream;
        private readonly string contentType;

        internal ZplTargetBuilder(LabelzoomClient client, string sourcePath, string contentType)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.sourcePath = sourcePath ?? throw new ArgumentNullException(nameof(sourcePath));
            this.contentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }

        internal ZplTargetBuilder(LabelzoomClient client, Stream sourceStream, string contentType)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            this.contentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }

        /// <summary>
        /// Executes the conversion and returns the complete ZPL as a single string.
        /// Best used for smaller documents with fewer pages.
        /// </summary>
        /// <param name="ct">Optional cancellation token.</param>
        /// <returns>The complete ZPL string.</returns>
        public async Task<string> ExecuteAsync(CancellationToken ct = default)
        {
            Stream streamToUse = sourceStream;
            bool shouldDisposeStream = false;

            if (streamToUse == null)
            {
                streamToUse = File.OpenRead(sourcePath);
                shouldDisposeStream = true;
            }

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, $"{client.Endpoint}/api/v2/convert/{GetSourceFormat()}/to/zpl"))
                using (var content = new StreamContent(streamToUse))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    request.Content = content;
                    using (var response = await client.GetHttpClient().SendAsync(request, ct))
                    {
                        response.EnsureSuccessStatusCode();
                        ct.ThrowIfCancellationRequested();
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            finally
            {
                if (shouldDisposeStream && streamToUse != null)
                {
                    streamToUse.Dispose();
                }
            }
        }

        /// <summary>
        /// Executes the conversion and streams the response one label at a time.
        /// Best used for larger documents with many pages.
        /// Each label returned will invoke the callback function.
        /// </summary>
        /// <param name="onLabelAsync">Callback function called once per label.</param>
        /// <param name="ct">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StreamAsync(Func<string, Task> onLabelAsync, CancellationToken ct = default)
        {
            if (onLabelAsync == null)
            {
                throw new ArgumentNullException(nameof(onLabelAsync));
            }

            Stream streamToUse = sourceStream;
            bool shouldDisposeStream = false;

            if (streamToUse == null)
            {
                streamToUse = File.OpenRead(sourcePath);
                shouldDisposeStream = true;
            }

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, $"{client.Endpoint}/api/v2.5/convert/{GetSourceFormat()}/to/zpl"))
                using (var content = new StreamContent(streamToUse))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    request.Content = content;
                    using (var response = await client.GetHttpClient().SendAsync(request, ct))
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
            finally
            {
                if (shouldDisposeStream && streamToUse != null)
                {
                    streamToUse.Dispose();
                }
            }
        }

        private string GetSourceFormat()
        {
            if (contentType == "application/pdf")
                return "pdf";
            if (contentType == "image/png")
                return "png";
            
            throw new InvalidOperationException($"Unsupported content type: {contentType}");
        }
    }
}

