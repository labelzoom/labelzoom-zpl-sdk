using System;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for ZPL source conversions.
    /// </summary>
    public class ZplSourceBuilder: ISource
    {
        private readonly LabelzoomClient client;
        private readonly string zplContent;

        internal ZplSourceBuilder(LabelzoomClient client, string zplContent)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.zplContent = zplContent ?? throw new ArgumentNullException(nameof(zplContent));
        }

        public RasterTargetBuilder ToBmp() => throw new NotImplementedException("PDF to BMP conversion is not yet supported.");
        public RasterTargetBuilder ToGif() => throw new NotImplementedException("GIF to BMP conversion is not yet supported.");
        public RasterTargetBuilder ToJpeg() => throw new NotImplementedException("JPEG to BMP conversion is not yet supported.");

        /// <summary>
        /// Converts the ZPL to PDF format.
        /// </summary>
        /// <returns>A builder for executing the PDF conversion.</returns>
        public PdfTargetBuilder ToPdf()
        {
            throw new NotImplementedException("ZPL to PDF conversion is not yet supported.");
        }

        /// <summary>
        /// Converts the ZPL to PNG format.
        /// </summary>
        /// <returns>A builder for executing the PNG conversion.</returns>
        public RasterTargetBuilder ToPng()
        {
            throw new NotImplementedException("ZPL to PNG conversion is not yet supported.");
        }

        /// <summary>
        /// Converts the ZPL to ZPL format.
        /// </summary>
        /// <returns>A builder for executing the ZPL conversion.</returns>
        public ZplTargetBuilder ToZpl()
        {
            throw new NotImplementedException("ZPL to ZPL conversion is not yet supported.");
        }
    }
}

