namespace AVConsole
{
    using AVSearch.Model.Expressions;
    using AVSearch.Model.Results;
    using AVXFramework;
    using System.Text;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

             var engine = new AVEngine(@"C:\src\Digital-AV\omega\AVX-Omega.data");

            (UInt32 expected, bool okay) version = Pinshot.Blue.Pinshot_RustFFI.LibraryVersion;

            if (!version.okay)
            {
                Console.WriteLine("Unexpected library version encountered ...\n");
            }
            Console.WriteLine("Hello AV-Console!\n");

            byte lastBook = 0;
            byte lastChapter = 0;
            bool wholeChapter = false;
            bool done = false;
            do
            {
                bool error = false;
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    input = input.Trim();

                    if (input.Equals("whole-chapter"))
                    {
                        wholeChapter = true;
                        continue;
                    }
                    else if (input.Equals("only-verse"))
                    {
                        wholeChapter = false;
                        continue;
                    }

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

                                            if (wholeChapter)
                                            {
                                                if (c != lastChapter && book.BookNum != lastBook)
                                                {
                                                    ChapterRendering crend = engine.GetChapter(book.BookNum, c, book.Matches);
                                                    StringBuilder builder = new();
                                                    if (engine.RenderChapter(builder, crend, exp.Settings))
                                                        Console.WriteLine(builder.ToString());
                                                    else
                                                        Console.WriteLine("ERROR: Unable to render chapter");

                                                    lastChapter = c;
                                                    lastBook = book.BookNum;
                                                }
                                                Console.Write("Continue? (y/n)");
                                                string? answer = Console.ReadLine();
                                                if ((answer != null) && answer.StartsWith("n", StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    done = true;
                                                    goto DONE;
                                                }
                                            }
                                            else
                                            {
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
                }
                else
                {
                    done = true;
                }
            DONE:
            ;
            }   while (!done);
        }
    }
}