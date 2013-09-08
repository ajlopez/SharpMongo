namespace SharpMongo.Console
{
    using System;
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
            Context context = new Context();
            context.Engine = new Engine();

            while (true)
            {
                string line = Console.ReadLine();
                Parser parser = new Parser(line);
                ICommand command = parser.ParseCommand();

                if (command == null)
                    continue;

                var result = command.Execute(context);

                if (result == null)
                    continue;

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

                Console.WriteLine(result.ToString());
            }
        }
    }
}
