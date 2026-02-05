using System;
using System.IO;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for PDF source conversions.
    /// </summary>
    public class PdfSourceBuilder: ISource
    {
        private readonly LabelzoomClient client;
        private readonly string pdfPath;
        private readonly Stream pdfStream;
        private readonly PdfSourceOptions options = new PdfSourceOptions();

        internal PdfSourceBuilder(LabelzoomClient client, string pdfPath)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.pdfPath = pdfPath ?? throw new ArgumentNullException(nameof(pdfPath));
        }

        internal PdfSourceBuilder(LabelzoomClient client, Stream pdfStream)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.pdfStream = pdfStream ?? throw new ArgumentNullException(nameof(pdfStream));
        }

        public PdfSourceBuilder WithFixedWidth(int? width)
        {
            options.FixedWidth = width;
            return this;
        }

        public PdfSourceBuilder WithFixedHeight(int? height)
        {
            options.FixedHeight = height;
            return this;
        }

        public PdfSourceBuilder WithPageNumber(int? pageNumber)
        {
            options.PageNumber = pageNumber;
            return this;
        }

        public PdfSourceBuilder WithRenderingMode(PdfRenderMode renderMode)
        {
            options.RenderMode = renderMode;
            return this;
        }

        public RasterTargetBuilder ToBmp() => throw new NotSupportedException("PDF to BMP conversion is not supported.");
        public RasterTargetBuilder ToGif() => throw new NotSupportedException("GIF to BMP conversion is not supported.");
        public RasterTargetBuilder ToJpeg() => throw new NotSupportedException("JPEG to BMP conversion is not supported.");
        public RasterTargetBuilder ToPng() => throw new NotSupportedException("PNG to BMP conversion is not supported.");
        public PdfTargetBuilder ToPdf() => throw new NotSupportedException("PDF to PDF conversion is not supported.");

        /// <summary>
        /// Converts the PDF to ZPL format.
        /// </summary>
        /// <returns>A builder for executing the ZPL conversion.</returns>
        public ZplTargetBuilder ToZpl()
        {
            if (pdfPath != null)
            {
                return new ZplTargetBuilder(client, pdfPath, "application/pdf");
            }
            else
            {
                return new ZplTargetBuilder(client, pdfStream, "application/pdf");
            }
        }
    }
}

