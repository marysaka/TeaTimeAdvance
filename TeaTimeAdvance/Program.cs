using CommandLine;
using System;

namespace TeaTimeAdvance
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                        .WithParsed(options => Start(options))
                        .WithNotParsed(errors => errors.Output());
        }

        private static void Start(Options options)
        {
            EmulationContext context = new EmulationContext();
            context.LoadBios(options.BiosPath);
            context.LoadRom(options.InputPath);
            context.Reset(true);

            while (context.IsRunning())
            {
                context.ExecuteFrame();
            }
        }
    }
}
