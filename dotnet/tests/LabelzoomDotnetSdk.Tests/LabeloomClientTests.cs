using LabelzoomDotnetSdk;

namespace LabelzoomDotnetSdkTest
{
    public class LabeloomClientTests
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
    }
}