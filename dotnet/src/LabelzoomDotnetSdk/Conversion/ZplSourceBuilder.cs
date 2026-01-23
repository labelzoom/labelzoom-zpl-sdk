using System;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for ZPL source conversions.
    /// </summary>
    public class ZplSourceBuilder
    {
        private readonly LabelzoomClient client;
        private readonly string zplContent;

        internal ZplSourceBuilder(LabelzoomClient client, string zplContent)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.zplContent = zplContent ?? throw new ArgumentNullException(nameof(zplContent));
        }

        /// <summary>
        /// Converts the ZPL to PDF format.
        /// </summary>
        /// <returns>A builder for executing the PDF conversion.</returns>
        public PdfTargetBuilder ToPdf()
        {
            throw new NotImplementedException("ZPL to PDF conversion is not yet supported by the API.");
        }

        /// <summary>
        /// Converts the ZPL to PNG format.
        /// </summary>
        /// <returns>A builder for executing the PNG conversion.</returns>
        public PngTargetBuilder ToPng()
        {
            throw new NotImplementedException("ZPL to PNG conversion is not yet supported by the API.");
        }
    }
}

