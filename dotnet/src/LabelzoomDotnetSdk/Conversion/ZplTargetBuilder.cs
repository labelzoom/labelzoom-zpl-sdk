using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk.Conversion
{
    /// <summary>
    /// Builder for ZPL target conversions with execution methods.
    /// </summary>
    public class ZplTargetBuilder: TargetBuilderBase
    {
        internal ZplTargetBuilder(LabelzoomClient client, string sourcePath, string contentType) : base(client, sourcePath, contentType)
        {
        }

        internal ZplTargetBuilder(LabelzoomClient client, Stream sourceStream, string contentType) : base(client, sourceStream, contentType)
        {
        }
    }
}

