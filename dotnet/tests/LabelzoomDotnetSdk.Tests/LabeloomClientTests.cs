using System.Text;
using LabelzoomDotnetSdk;

namespace LabelzoomDotnetSdk.Tests
{
    public class LabelzoomClientTests: TestBase
    {
        [Fact]
        public async Task Convert_PdfToZpl_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            var zpl = await _client.Convert()
                .FromPdf(pdfPath)
                .ToZpl()
                .ExecuteAsync();
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
            await _client.Convert()
                .FromPdf(pdfPath)
                .ToZpl()
                .StreamAsync(async (zpl) => {
                    labelCount++;
                    zplBuffer.Append(zpl);
                });
            Assert.Contains("^XA", zplBuffer.ToString());
            Assert.Contains("^XZ", zplBuffer.ToString());
            Assert.Equal(12, labelCount);
        }
    }
}
