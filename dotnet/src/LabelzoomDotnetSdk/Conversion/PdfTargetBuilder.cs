using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for PDF target conversions (placeholder for future implementation).
    /// </summary>
    public class PdfTargetBuilder: TargetBuilderBase
    {
        internal PdfTargetBuilder(LabelzoomClient client, string sourcePath, string contentType) : base(client, sourcePath, contentType) { }

        internal PdfTargetBuilder(LabelzoomClient client, Stream sourceStream, string contentType) : base(client, sourceStream, contentType) { }

        /// <summary>
        /// Executes the conversion and returns the PDF as a byte array.
        /// </summary>
        /// <param name="ct">Optional cancellation token.</param>
        /// <returns>The PDF data as a byte array.</returns>
        public Task<byte[]> ExecuteAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException("PDF target conversion is not yet supported by the API.");
        }
    }
}

