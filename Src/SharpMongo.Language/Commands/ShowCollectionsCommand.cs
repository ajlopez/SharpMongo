namespace SharpMongo.Language.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ShowCollectionsCommand : ICommand
    {
        public object Execute(Context context)
        {
            if (context.DocumentBase == null)
                return new string[] { };

            return context.DocumentBase.GetCollectionNames().ToList();
        }
    }
}
