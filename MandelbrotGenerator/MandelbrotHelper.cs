using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotGenerator
{
    class MandelbrotHelper
    {
        private readonly InputOptions options;
        private readonly int width; // px
        private readonly int height; // px

        private readonly double aMin;
        private readonly double aMax;

        private readonly double yMin;
        private readonly double yMax;

        private readonly int tasks;
        private readonly int granularity;

        private readonly int[] data;
        private readonly int maxIterations;

        public MandelbrotHelper(int maxIterations, InputOptions options)
        {
            this.options = options;

            this.width = options.Width;
            this.height = options.Height;
            this.aMin = options.ARange.Min;
            this.aMax = options.ARange.Max;
            this.yMin = options.BRange.Min;
            this.yMax = options.BRange.Max;
            this.tasks = options.Tasks;
            this.granularity = options.Granularity;

            this.data = new int[width * height];
            this.maxIterations = maxIterations;
        }

        public int CalcPoint(Complex startPoint)
        {
            var currentPoint = startPoint;
            int iterations = 0;
            while (iterations < maxIterations)
            {
                if (Complex.Abs(currentPoint) > 2)
                {
                    break;
                }

                var currentPointSqrt = Complex.Multiply(currentPoint, currentPoint);
                var temp = Complex.Pow(Math.E, currentPointSqrt);
                currentPoint = Complex.Multiply(currentPointSqrt, temp) + startPoint;
                iterations++;
            }

            return iterations;
        }

        public void CalcSection(int start, int end)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var currentTaskId = Thread.CurrentThread.ManagedThreadId % tasks + 1;
            if (!options.IsQuiet)
            {
                Console.WriteLine("Running tasks:" + count);
                Console.WriteLine($"Thread-{currentTaskId} started.");
            }

            for (int i = start; i < end; i++)
            {
                var x = i % width;
                var y = i / width + 1;
                var xc = aMin + (aMax - aMin) * x / width;
                var yc = yMin + (yMax - yMin) * y / height;
                data[i] = CalcPoint(new Complex(xc, yc));
            }

            stopWatch.Stop();
            var time = stopWatch.Elapsed;

            if (!options.IsQuiet)
            {
                Console.WriteLine($"Thread-{currentTaskId} stopped.");
                Console.WriteLine($"Thread-{currentTaskId} execution time was (millis) {time.TotalMilliseconds}.");
            }
        }

        public int count = 0;
        public int[] Calculate()
        {
            var sectionsCount = tasks == 1 ? 1 : tasks * granularity;
            var sectionLength = (int)Math.Ceiling((double)data.Length / sectionsCount);

            using (var s = new SemaphoreSlim(tasks))
            {
                var tasksList = new List<Task>();

                for (int i = 0; i < sectionsCount; i++)
                {
                    s.Wait();

                    count = Math.Max(count, tasks - s.CurrentCount);

                    var start = i * sectionLength;
                    var end = Math.Min((i + 1) * sectionLength, data.Length);

                    var task = Task.Factory.StartNew(() =>
                    {
                        CalcSection(start, end);

                        s.Release();
                    });
                    tasksList.Add(task);
                }

                Task.WaitAll(tasksList.ToArray());
            }

            return this.data;

        }

        public Color LookupColor(int value)
        {

            if (value >= this.maxIterations)
            {
                return Color.Brown;
            }

            if (value < 1)
            {
                return Color.Black;
            }

            if (value < 2)
            {
                return Color.FromArgb(20, 0, 0);
            }

            if (value < 3)
            {
                return Color.FromArgb(40, 0, 0);
            }

            if (value < 4)
            {
                return Color.FromArgb(70, 0, 0);
            }

            if (value < 5)
            {
                return Color.FromArgb(120, 0, 0);
            }

            if (value < 6)
            {
                return Color.FromArgb(150, 0, 0);
            }

            if (value < 7)
            {
                return Color.FromArgb(190, 0, 0);
            }

            if (value < 8)
            {
                return Color.FromArgb(220, 0, 0);
            }
            if (value < 9)
            {
                return Color.FromArgb(240, 0, 0);
            }
            if (value < 10)
            {
                return Color.Red;
            }

            if (value < 15)
            {
                return Color.LightCoral;
            }
            if (value < 15)
            {
                return Color.Coral;
            }

            if (value < 17)
            {
                return Color.RosyBrown;
            }
            if (value < 20)
            {
                return Color.Yellow;
            }

            if (value < 25)
            {
                return Color.DarkSalmon;
            }
            if (value < 30)
            {
                return Color.DarkOrange;
            }
            if (value < 60)
            {
                return Color.YellowGreen;
            }

            return Color.LightYellow;
        }
    }
}
