namespace SharpMongo.Language.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ShowDbsCommand : ICommand
    {
        public object Execute(Context context)
        {
            return context.Engine.GetDocumentBaseNames().ToList();
        }
    }
}
