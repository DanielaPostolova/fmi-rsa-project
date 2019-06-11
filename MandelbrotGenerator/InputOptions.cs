namespace MandelbrotGenerator
{
    class InputOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Output { get; set; }
        public int Tasks { get; set; }
        public Range<double> ARange { get; set; }
        public Range<double> BRange { get; set; }
        public bool IsQuiet { get; set; }
        public int Granularity { get; set; }

    }
}
