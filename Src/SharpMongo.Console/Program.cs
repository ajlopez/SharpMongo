namespace SharpMongo.Console
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Core;
    using SharpMongo.Language;
    using SharpMongo.Language.Commands;
    using SharpMongo.Language.Compiler;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("SharpMongo 0.0.1");

            Context context = new Context();
            context.Engine = new Engine();

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                Parser parser = new Parser(line);
                ICommand command = parser.ParseCommand();

                if (command == null)
                    continue;

                if (command is ExitCommand)
                {
                    Console.WriteLine("bye");
                    return;
                }

                var result = command.Execute(context);

                if (result == null)
                    continue;

                if (result is string)
                {
                    Console.WriteLine(result);
                    continue;
                }

                if (result is DynamicObject)
                {
                    Console.WriteLine(((DynamicObject)result).ToJsonString());
                    continue;
                }

                if (result is IEnumerable<DynamicObject>)
                {
                    foreach (var doc in ((IEnumerable<DynamicObject>)result))
                        Console.WriteLine(doc.ToJsonString());
                    continue;
                }

                if (result is IEnumerable)
                {
                    foreach (var value in ((IEnumerable)result))
                        Console.WriteLine(value.ToString());
                    continue;
                }

                Console.WriteLine(result.ToString());
            }
        }
    }
}
