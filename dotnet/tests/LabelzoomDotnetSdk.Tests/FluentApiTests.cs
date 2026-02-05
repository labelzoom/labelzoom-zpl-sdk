using System;
using System.IO;
using System.Threading.Tasks;
using LabelzoomDotnetSdk;
using Xunit;

namespace LabelzoomDotnetSdk.Tests
{
    public class FluentApiTests: TestBase
    {
        [Fact]
        public async Task FluentApi_PdfToZpl_ReturnsValidZpl()
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
        public async Task FluentApi_PdfToZpl_Streaming_ReturnsValidZpl()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            int labelCount = 0;
            string zplBuffer = "";

            await _client.Convert()
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

        [Fact]
        public void FluentApi_FromPdf_WithNullPath_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _client.Convert().FromPdf((string)null));
        }

        [Fact]
        public void FluentApi_FromPdf_WithEmptyPath_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _client.Convert().FromPdf(""));
        }

        [Fact]
        public void FluentApi_FromPdf_WithNonExistentFile_ThrowsException()
        {
            Assert.Throws<FileNotFoundException>(() => _client.Convert().FromPdf("nonexistent.pdf"));
        }

        [Fact]
        public void FluentApi_FromPdf_WithNullStream_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _client.Convert().FromPdf((Stream)null));
        }

        [Fact]
        public void FluentApi_FromPng_WithNullPath_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _client.Convert().FromPng((string)null));
        }

        [Fact]
        public void FluentApi_FromPng_WithNonExistentFile_ThrowsException()
        {
            Assert.Throws<FileNotFoundException>(() => _client.Convert().FromPng("nonexistent.png"));
        }

        [Fact]
        public void FluentApi_FromZpl_WithNullContent_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _client.Convert().FromZpl(null));
        }

        [Fact]
        public void FluentApi_FromZpl_WithEmptyContent_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _client.Convert().FromZpl(""));
        }

        [Fact]
        public async Task FluentApi_StreamAsync_WithNullCallback_ThrowsException()
        {
            var pdfPath = Path.Combine(AppContext.BaseDirectory, "TestData", "4x6_document.pdf");
            Assert.True(File.Exists(pdfPath), $"Missing test PDF: {pdfPath}");

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _client.Convert()
                    .FromPdf(pdfPath)
                    .ToZpl()
                    .StreamAsync(null);
            });
        }

        [Fact]
        public async Task FluentApi_WithBuilder_PdfToZpl_ReturnsValidZpl()
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
    }
}

