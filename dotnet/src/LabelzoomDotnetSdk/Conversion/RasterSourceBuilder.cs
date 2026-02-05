using System;
using System.IO;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for raster source conversions.
    /// </summary>
    public class RasterSourceBuilder: ISource
    {
        private readonly LabelzoomClient client;
        private readonly string contentType;
        private readonly string rasterPath;
        private readonly Stream rasterStream;

        internal RasterSourceBuilder(LabelzoomClient client, string contentType, string rasterPath)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.contentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            this.rasterPath = rasterPath ?? throw new ArgumentNullException(nameof(rasterPath));
        }

        internal RasterSourceBuilder(LabelzoomClient client, string contentType, Stream rasterStream)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.contentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            this.rasterStream = rasterStream ?? throw new ArgumentNullException(nameof(rasterStream));
        }

        public RasterTargetBuilder ToBmp() => throw new NotSupportedException("PDF to BMP conversion is not supported.");
        public RasterTargetBuilder ToGif() => throw new NotSupportedException("GIF to BMP conversion is not supported.");
        public RasterTargetBuilder ToJpeg() => throw new NotSupportedException("JPEG to BMP conversion is not supported.");
        public RasterTargetBuilder ToPng() => throw new NotSupportedException("PNG to BMP conversion is not supported.");
        public PdfTargetBuilder ToPdf() => throw new NotSupportedException("PDF to PDF conversion is not supported.");

        /// <summary>
        /// Converts the raster to ZPL format.
        /// </summary>
        /// <returns>A builder for executing the ZPL conversion.</returns>
        public ZplTargetBuilder ToZpl()
        {
            if (rasterPath != null)
            {
                return new ZplTargetBuilder(client, rasterPath, contentType);
            }
            else
            {
                return new ZplTargetBuilder(client, rasterStream, contentType);
            }
        }
    }
}

