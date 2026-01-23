using System;
using System.Threading;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for PDF target conversions (placeholder for future implementation).
    /// </summary>
    public class PdfTargetBuilder
    {
        private readonly LabelzoomClient client;

        internal PdfTargetBuilder(LabelzoomClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

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

