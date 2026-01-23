using System.Text;
using LabelzoomDotnetSdk;

namespace LabelzoomDotnetSdk.Tests
{
    public class LabelzoomClientTests: IDisposable
    {
        private bool disposedValue;

        private readonly LabelzoomClient _client;

        public LabelzoomClientTests()
        {
            var token = Environment.GetEnvironmentVariable("LABELZOOM_API_TOKEN") ?? throw new ArgumentException("LABELZOOM_API_TOKEN environment variable was not defined");
            var endpoint = Environment.GetEnvironmentVariable("LABELZOOM_ENDPOINT") ?? throw new ArgumentException("LABELZOOM_ENDPOINT environment variable was not defined");

            _client = new LabelzoomClient(token);
            _client.Endpoint = endpoint;
        }

        [Fact]
        public async Task Convert_PdfToZpl_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            var zpl = await _client.PdfToZpl(pdfPath);
            Assert.NotNull(zpl);
            Assert.Contains("^XA", zpl);
            Assert.Contains("^XZ", zpl);
        }

        [Fact]
        public async Task Convert_PdfToZplAsync_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            int labelCount = 0;
            var zplBuffer = new StringBuilder();
            await _client.PdfToZplAsync(pdfPath, async (zpl) => {
                labelCount++;
                zplBuffer.Append(zpl);
            });
            Assert.Contains("^XA", zplBuffer.ToString());
            Assert.Contains("^XZ", zplBuffer.ToString());
            Assert.Equal(12, labelCount);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
