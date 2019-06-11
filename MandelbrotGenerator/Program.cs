using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace MandelbrotGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var inputParser = new InputParser();
            var inputOptions = inputParser.GetOptions(args);

            var maxIterations = 1000;

            var MandelbrotHelper = new MandelbrotHelper(maxIterations, inputOptions);

            var data = MandelbrotHelper.Calculate();

            using (Bitmap image = new Bitmap(inputOptions.Width, inputOptions.Height))
            {
                for (var y = 0; y < inputOptions.Height; y++)
                {
                    for (var x = 0; x < inputOptions.Width; x++)
                    {
                        var iterations = data[y * inputOptions.Width + x];
                        image.SetPixel(x, y, MandelbrotHelper.LookupColor(iterations));
                    }
                }

                image.Save(inputOptions.Output, ImageFormat.Png);
            }

            stopWatch.Stop();
            var totalTime = stopWatch.Elapsed;

            if (!inputOptions.IsQuiet)
            {
                Console.WriteLine($"Threads used in current run: {inputOptions.Tasks}.");
                Console.WriteLine($"Total execution time for current run (millis): {totalTime.TotalMilliseconds}.");
            }
        }
    }
}
