using AVXFramework;
using Blueprint.Blue;
using System.Text.Json;

namespace AVConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

             var engine = new AVEngine(@"C:\Users\Me\AVX\Quelle\", @"C:\src\AVX\omega\AVX-Omega-3911.data");

            (UInt32 expected, bool okay) version = Pinshot.Blue.Pinshot_RustFFI.LibraryVersion;

            if (!version.okay)
            {
                Console.WriteLine("Unexpected library version encountered ...\n");
            }
            Console.WriteLine("Hello AV-Console!\n");

            bool done = false;
            do
            {
                bool error = false;
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    input = input.Trim();

                    var tuple = engine.Execute(input);

                    var message = !string.IsNullOrWhiteSpace(tuple.message);
                    if (message)
                    {
                        error = true;
                        Console.Error.WriteLine(tuple.message);
                    }
                    if (error)
                    {
                        Console.Error.WriteLine("One or more errors were encountered");
                    }
                }
                else
                {
                    done = true;
                }
            }   while (!done);
        }
    }
}