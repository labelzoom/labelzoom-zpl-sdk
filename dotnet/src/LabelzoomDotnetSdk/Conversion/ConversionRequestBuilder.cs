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

        /// <summary>
        /// Specifies a PNG file as the source for conversion.
        /// </summary>
        /// <param name="pngPath">Path to the PNG file.</param>
        /// <returns>A builder for specifying the target format.</returns>
        public PngSourceBuilder FromPng(string pngPath)
        {
            if (string.IsNullOrWhiteSpace(pngPath))
            {
                throw new ArgumentException("PNG path cannot be null or empty.", nameof(pngPath));
            }

            if (!File.Exists(pngPath))
            {
                throw new FileNotFoundException($"PNG file not found: {pngPath}", pngPath);
            }

            return new PngSourceBuilder(client, pngPath);
        }

        /// <summary>
        /// Specifies a PNG stream as the source for conversion.
        /// </summary>
        /// <param name="pngStream">Stream containing PNG data.</param>
        /// <returns>A builder for specifying the target format.</returns>
        public PngSourceBuilder FromPng(Stream pngStream)
        {
            if (pngStream == null)
            {
                throw new ArgumentNullException(nameof(pngStream));
            }

            if (!pngStream.CanRead)
            {
                throw new ArgumentException("Stream must be readable.", nameof(pngStream));
            }

            return new PngSourceBuilder(client, pngStream);
        }
    }
}

