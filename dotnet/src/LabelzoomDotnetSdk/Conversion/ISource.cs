using System;
using System.Collections.Generic;
using System.Text;

namespace LabelzoomDotnetSdk.Conversion
{
    internal interface ISource
    {
        ZplTargetBuilder ToZpl();

        PdfTargetBuilder ToPdf();

        RasterTargetBuilder ToBmp();

        RasterTargetBuilder ToGif();

        RasterTargetBuilder ToJpeg();

        RasterTargetBuilder ToPng();
    }
}
