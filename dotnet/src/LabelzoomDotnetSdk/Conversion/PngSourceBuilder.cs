using System;
using System.IO;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for PNG source conversions.
    /// </summary>
    public class PngSourceBuilder
    {
        private readonly LabelzoomClient client;
        private readonly string pngPath;
        private readonly Stream pngStream;

        internal PngSourceBuilder(LabelzoomClient client, string pngPath)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.pngPath = pngPath ?? throw new ArgumentNullException(nameof(pngPath));
        }

        internal PngSourceBuilder(LabelzoomClient client, Stream pngStream)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.pngStream = pngStream ?? throw new ArgumentNullException(nameof(pngStream));
        }

        /// <summary>
        /// Converts the PNG to ZPL format.
        /// </summary>
        /// <returns>A builder for executing the ZPL conversion.</returns>
        public ZplTargetBuilder ToZpl()
        {
            if (pngPath != null)
            {
                return new ZplTargetBuilder(client, pngPath, "image/png");
            }
            else
            {
                return new ZplTargetBuilder(client, pngStream, "image/png");
            }
        }
    }
}

