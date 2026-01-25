using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for PNG target conversions (placeholder for future implementation).
    /// </summary>
    public class RasterTargetBuilder: TargetBuilderBase
    {
        internal RasterTargetBuilder(LabelzoomClient client, string sourcePath, string contentType) : base(client, sourcePath, contentType) { }

        internal RasterTargetBuilder(LabelzoomClient client, Stream sourceStream, string contentType) : base(client, sourceStream, contentType) { }

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

