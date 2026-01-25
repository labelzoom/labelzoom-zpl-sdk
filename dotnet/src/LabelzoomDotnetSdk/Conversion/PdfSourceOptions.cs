namespace LabelzoomDotnetSdk.Conversion
{
    internal class PdfSourceOptions
    {
        public int? FixedWidth { get; set; }

        public int? FixedHeight { get; set; }

        public int? PageNumber { get; set; }

        public PdfRenderMode RenderMode { get; set; } = PdfRenderMode.Image;
    }
}
