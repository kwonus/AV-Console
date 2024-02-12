using AVSearch.Model.Expressions;
using AVSearch.Model.Results;
using AVXFramework;
using AVXLib.Memory;
using Blueprint.Blue;
using Blueprint.Model.Implicit;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using static Blueprint.Model.Implicit.QFormat;

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
                        error = !tuple.message.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
                        if (error)
                            Console.Error.WriteLine(tuple.message);
                    }
                    if (!error && (tuple.find != null && tuple.find.Expressions != null))
                    {
                        QueryResult result = tuple.find;

                        foreach (SearchExpression exp in result.Expressions)
                        {
                            if (exp.Hits > 0)
                            {
                                foreach (QueryBook book in exp.Books.Values)
                                {
                                    foreach (QueryChapter chapter in book.Chapters.Values)
                                    {
                                        foreach (var match in book.Matches)
                                        {
                                            byte c = match.Value.Start.C;
                                            byte v = match.Value.Start.V;

                                            VerseRendering vrend = engine.GetVerse(book.BookNum, c, v, book.Matches);
                                            SoloVerseRendering vsolo = new(vrend);
                                            StringBuilder builder = new();
                                            if (engine.RenderVerseSolo(builder, vsolo, exp.Settings))
                                                Console.WriteLine(builder.ToString());
                                            else
                                                Console.WriteLine("ERROR: Unable to render verse");
                                        }
                                    }
                                }
                            }
                        }


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