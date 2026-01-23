using System;
using System.IO;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for PDF source conversions.
    /// </summary>
    public class PdfSourceBuilder
    {
        private readonly LabelzoomClient client;
        private readonly string pdfPath;
        private readonly Stream pdfStream;

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

        /// <summary>
        /// Converts the PDF to another PDF (placeholder for future transformations).
        /// </summary>
        /// <returns>A builder for executing the PDF conversion.</returns>
        public PdfTargetBuilder ToPdf()
        {
            throw new NotImplementedException("PDF to PDF conversion is not yet supported.");
        }
    }
}

