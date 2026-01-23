using System;
using System.Threading;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for PNG target conversions (placeholder for future implementation).
    /// </summary>
    public class PngTargetBuilder
    {
        private readonly LabelzoomClient client;

        internal PngTargetBuilder(LabelzoomClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Executes the conversion and returns the PNG as a byte array.
        /// </summary>
        /// <param name="ct">Optional cancellation token.</param>
        /// <returns>The PNG data as a byte array.</returns>
        public Task<byte[]> ExecuteAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException("PNG target conversion is not yet supported by the API.");
        }
    }
}

