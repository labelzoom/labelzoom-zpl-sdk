using System;
using System.IO;
using System.Threading.Tasks;
using LabelzoomDotnetSdk;
using Xunit;

namespace LabelzoomDotnetSdk.Tests
{
    public class FluentApiTests
    {
        [Fact]
        public async Task FluentApi_PdfToZpl_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            using (var client = new LabelzoomClient(""))
            {
                var zpl = await client.Convert()
                    .FromPdf(pdfPath)
                    .ToZpl()
                    .ExecuteAsync();

                Assert.NotNull(zpl);
                Assert.Contains("^XA", zpl);
                Assert.Contains("^XZ", zpl);
            }
        }

        [Fact]
        public async Task FluentApi_PdfToZpl_Streaming_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            using (var client = new LabelzoomClient(""))
            {
                int labelCount = 0;
                string zplBuffer = "";

                await client.Convert()
                    .FromPdf(pdfPath)
                    .ToZpl()
                    .StreamAsync(async (zpl) =>
                    {
                        labelCount++;
                        zplBuffer += zpl;
                        await Task.CompletedTask;
                    });

                Assert.NotNull(zplBuffer);
                Assert.Contains("^XA", zplBuffer);
                Assert.Contains("^XZ", zplBuffer);
                Assert.Equal(12, labelCount);
            }
        }

        [Fact]
        public void FluentApi_FromPdf_WithNullPath_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<ArgumentException>(() => client.Convert().FromPdf((string)null));
            }
        }

        [Fact]
        public void FluentApi_FromPdf_WithEmptyPath_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<ArgumentException>(() => client.Convert().FromPdf(""));
            }
        }

        [Fact]
        public void FluentApi_FromPdf_WithNonExistentFile_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<FileNotFoundException>(() => client.Convert().FromPdf("nonexistent.pdf"));
            }
        }

        [Fact]
        public void FluentApi_FromPdf_WithNullStream_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<ArgumentNullException>(() => client.Convert().FromPdf((Stream)null));
            }
        }

        [Fact]
        public void FluentApi_FromPng_WithNullPath_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<ArgumentException>(() => client.Convert().FromPng((string)null));
            }
        }

        [Fact]
        public void FluentApi_FromPng_WithNonExistentFile_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<FileNotFoundException>(() => client.Convert().FromPng("nonexistent.png"));
            }
        }

        [Fact]
        public void FluentApi_FromZpl_WithNullContent_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<ArgumentException>(() => client.Convert().FromZpl(null));
            }
        }

        [Fact]
        public void FluentApi_FromZpl_WithEmptyContent_ThrowsException()
        {
            using (var client = new LabelzoomClient("test-token"))
            {
                Assert.Throws<ArgumentException>(() => client.Convert().FromZpl(""));
            }
        }

        [Fact]
        public async Task FluentApi_StreamAsync_WithNullCallback_ThrowsException()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            using (var client = new LabelzoomClient(""))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                {
                    await client.Convert()
                        .FromPdf(pdfPath)
                        .ToZpl()
                        .StreamAsync(null);
                });
            }
        }

        [Fact]
        public async Task FluentApi_WithBuilder_PdfToZpl_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            using (var client = new LabelzoomClientBuilder()
                .WithToken("")
                .Build())
            {
                var zpl = await client.Convert()
                    .FromPdf(pdfPath)
                    .ToZpl()
                    .ExecuteAsync();

                Assert.NotNull(zpl);
                Assert.Contains("^XA", zpl);
                Assert.Contains("^XZ", zpl);
            }
        }
    }
}

