namespace SharpMongo.Language.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UseCommand : ICommand
    {
        private string name;

        public UseCommand(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public object Execute(Context context)
        {
            context.DocumentBase = context.Engine.GetOrCreateDocumentBase(this.name);
            DbObject dobj = new DbObject(context.DocumentBase);
            context.SetMember("db", dobj);
            return null;
        }
    }
}
