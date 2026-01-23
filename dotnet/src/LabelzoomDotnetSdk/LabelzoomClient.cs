using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk
{
    public class LabelzoomClient : IDisposable
    {
        private readonly HttpClient client;
        private bool disposedValue;

        public LabelzoomClient(string token)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public string Endpoint { get; set; } = "https://api.labelzoom.net";

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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LabelzoomClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
