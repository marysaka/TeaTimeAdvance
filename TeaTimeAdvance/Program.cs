using CommandLine;
using System;
using System.IO;

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

            byte[] bios = File.ReadAllBytes(options.BiosPath);
            byte[] rom = File.ReadAllBytes(options.GamePath);

            context.Initialize(bios, rom);

            while (context.IsRunning())
            {
                context.ExecuteFrame();
            }
        }
    }
}
