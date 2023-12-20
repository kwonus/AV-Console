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

            var singletons = new LocalStatementProcessor();
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
                        Console.Error.WriteLine(tuple.message);
                    }
                    if (tuple.stmt != null)
                    {
                        //singletons.ProcessStatement(tuple.stmt, tuple.result); // process singletons and persistent settings
                        /*
                        if (tuple.stmt.Commands != null)
                        {
                            int cnt = 0;
                            foreach (var segment in tuple.stmt.Commands.Searches)
                            {
                                if (++cnt == 1)
                                    Console.Error.WriteLine("SEARCH SEGMENTS:");

                                foreach (var line in segment.AsYaml())
                                {
                                    Console.WriteLine(line);
                                }
                            }
                            cnt = 0;
                            foreach (var segment in tuple.stmt.Commands.Assignments)
                            {
                                if (++cnt == 1)
                                    Console.Error.WriteLine("ASSIGNMENTS:");

                                foreach (var line in segment.AsYaml())
                                {
                                    Console.WriteLine(line);
                                }
                            }
                            cnt = 0;
                            foreach (var segment in tuple.stmt.Commands.Filters)
                            {
                                if (++cnt == 1)
                                    Console.Error.WriteLine("FILTERS:");

                                foreach (var line in segment.AsYaml())
                                {
                                    Console.WriteLine(line);
                                }
                            }
                            if (tuple.stmt.Commands.Display != null)
                            {
                                Console.Error.WriteLine("DISPLAY:");

                                foreach (var line in tuple.stmt.Commands.Display.AsYaml())
                                {
                                    Console.WriteLine(line);
                                }
                            }
                            if (tuple.stmt.Commands.Export != null)
                            {
                                Console.Error.WriteLine("EXPORT:");

                                foreach (var line in tuple.stmt.Commands.Export.AsYaml())
                                {
                                    Console.WriteLine(line);
                                }
                            }
                            var stmt = new NativeStatement(tuple.stmt.Blueprint);
                        
                        }*/
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