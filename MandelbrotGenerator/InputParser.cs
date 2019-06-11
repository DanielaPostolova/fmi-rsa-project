using Mono.Options;
using System;

namespace MandelbrotGenerator
{
    class InputParser
    {
        private readonly InputOptions inputOptions;

        public InputParser()
        {
            this.inputOptions = new InputOptions
            {
                ARange = new Range<double>
                {
                    Min = -2.0,
                    Max = 2.0
                },
                BRange = new Range<double>
                {
                    Min = -2.0,
                    Max = 2.0
                },
                IsQuiet = false,
                Width = 640,
                Height = 480,
                Tasks = 1,
                Output = "zad20.png",
                Granularity = 8
            };
        }


        public InputOptions GetOptions(string[] args)
        {
            var optionSet = new OptionSet()
            {
                { "s|size=", "Image size in pixels. (Default: 480x640)", SetSize },
                { "r|rect=", "Real and imaginery values ranges. (Default: -2.0:2.0:-2.0:2.0)", SetRanges },
                { "t|tasks=", "Tasks count. (Default: 1)", SetTasks },
                { "o|output=", "Output file name. (Default: zad20.png)", SetOutput },
                { "q|quiet", "(Default: false)", v => inputOptions.IsQuiet = (v != null) },
                { "g|granularity=", "(Default: 30)", SetGranularity }
            };

            optionSet.Parse(args);

            return inputOptions;
        }

        private void SetRanges(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var limits = value.Split(":");
                inputOptions.ARange.Min = double.Parse(limits[0]);
                inputOptions.ARange.Max = double.Parse(limits[1]);
                inputOptions.BRange.Min = double.Parse(limits[2]);
                inputOptions.BRange.Max = double.Parse(limits[3]);
            }
        }


        private void SetSize(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var imgSize = value.Split("x");
                inputOptions.Width = int.Parse(imgSize[0]);
                inputOptions.Height = int.Parse(imgSize[1]);
            }
        }

        private void SetOutput(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                inputOptions.Output = value;
            }
        }

        private void SetTasks(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                inputOptions.Tasks = int.Parse(value);
            }
        }

        private void SetGranularity(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                inputOptions.Granularity = int.Parse(value);
            }
        }
    }
}