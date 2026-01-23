using LabelzoomDotnetSdk;

namespace LabelzoomDotnetSdk.Tests
{
    public class LabelzoomClientTests
    {
        [Fact]
        public async Task Convert_PdfToZpl_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            using (var client = new LabelzoomClient(""))
            {
                var zpl = await client.PdfToZpl(pdfPath);
                Assert.NotNull(zpl);
                Assert.Contains("^XA", zpl);
                Assert.Contains("^XZ", zpl);
            }
        }

        [Fact]
        public async Task Convert_PdfToZplAsync_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            using (var client = new LabelzoomClient(Environment.GetEnvironmentVariable("LABELZOOM_API_TOKEN") ?? ""))
            {
                client.Endpoint = Environment.GetEnvironmentVariable("LABELZOOM_ENDPOINT");
                int labelCount = 0;
                string zplBuffer = "";
                await client.PdfToZplAsync(pdfPath, async (zpl) => {
                    labelCount++;
                    zplBuffer += zpl;
                });
                Assert.NotNull(zplBuffer);
                Assert.Contains("^XA", zplBuffer);
                Assert.Contains("^XZ", zplBuffer);
                Assert.Equal(12, labelCount);
            }
        }
    }
}
