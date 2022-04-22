using CommandLine;

namespace TeaTimeAdvance.SDL2
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
            EmulationWindow window = new EmulationWindow(options);

            window.Execute();
            window.Dispose();
        }
    }
}
