using System;
using System.IO;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Entry point for building fluent conversion requests.
    /// </summary>
    public class ConversionRequestBuilder
    {
        private readonly LabelzoomClient client;

        internal ConversionRequestBuilder(LabelzoomClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Specifies a PDF file as the source for conversion.
        /// </summary>
        /// <param name="pdfPath">Path to the PDF file.</param>
        /// <returns>A builder for specifying the target format.</returns>
        public PdfSourceBuilder FromPdf(string pdfPath)
        {
            if (string.IsNullOrWhiteSpace(pdfPath))
            {
                throw new ArgumentException("PDF path cannot be null or empty.", nameof(pdfPath));
            }

            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException($"PDF file not found: {pdfPath}", pdfPath);
            }

            return new PdfSourceBuilder(client, pdfPath);
        }

        /// <summary>
        /// Specifies a PDF stream as the source for conversion.
        /// </summary>
        /// <param name="pdfStream">Stream containing PDF data.</param>
        /// <returns>A builder for specifying the target format.</returns>
        public PdfSourceBuilder FromPdf(Stream pdfStream)
        {
            if (pdfStream == null)
            {
                throw new ArgumentNullException(nameof(pdfStream));
            }

            if (!pdfStream.CanRead)
            {
                throw new ArgumentException("Stream must be readable.", nameof(pdfStream));
            }

            return new PdfSourceBuilder(client, pdfStream);
        }

        /// <summary>
        /// Specifies ZPL content as the source for conversion.
        /// </summary>
        /// <param name="zplContent">The ZPL content string.</param>
        /// <returns>A builder for specifying the target format.</returns>
        public ZplSourceBuilder FromZpl(string zplContent)
        {
            if (string.IsNullOrWhiteSpace(zplContent))
            {
                throw new ArgumentException("ZPL content cannot be null or empty.", nameof(zplContent));
            }

            return new ZplSourceBuilder(client, zplContent);
        }

        public RasterSourceBuilder FromBmp(string bmpPath) => FromRaster(bmpPath, "BMP", "image/bmp");
        public RasterSourceBuilder FromBmp(Stream bmpStream) => FromRaster(bmpStream, "image/bmp");
        public RasterSourceBuilder FromGif(string gifPath) => FromRaster(gifPath, "GIF", "image/gif");
        public RasterSourceBuilder FromGif(Stream gifStream) => FromRaster(gifStream, "image/gif");
        public RasterSourceBuilder FromJpeg(string jpegPath) => FromRaster(jpegPath, "JPEG", "image/jpeg");
        public RasterSourceBuilder FromJpeg(Stream jpegStream) => FromRaster(jpegStream, "image/jpeg");
        public RasterSourceBuilder FromPng(string pngPath) => FromRaster(pngPath, "PNG", "image/png");
        public RasterSourceBuilder FromPng(Stream pngStream) => FromRaster(pngStream, "image/png");

        public RasterSourceBuilder FromRaster(string rasterPath, string fileType, string contentType)
        {
            if (string.IsNullOrWhiteSpace(rasterPath))
            {
                throw new ArgumentException(fileType + " path cannot be null or empty.", nameof(rasterPath));
            }

            if (!File.Exists(rasterPath))
            {
                throw new FileNotFoundException($"{fileType} file not found: {rasterPath}", rasterPath);
            }

            return new RasterSourceBuilder(client, contentType, rasterPath);
        }

        public RasterSourceBuilder FromRaster(Stream rasterStream, string contentType)
        {
            if (rasterStream == null)
            {
                throw new ArgumentNullException(nameof(rasterStream));
            }

            if (!rasterStream.CanRead)
            {
                throw new ArgumentException("Stream must be readable.", nameof(rasterStream));
            }

            return new RasterSourceBuilder(client, contentType, rasterStream);
        }
    }
}

