namespace SharpMongo.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Core;

    public class CollectionObject : DynamicObject
    {
        private Collection collection;

        public CollectionObject(Collection collection)
        {
            this.collection = collection;
            this.SetMember("insert", new InsertMethod(this));
            this.SetMember("find", new FindMethod(this));
            this.SetMember("findOne", new FindOneMethod(this));
            this.SetMember("update", new UpdateMethod(this));
            this.SetMember("remove", new RemoveMethod(this));
            this.SetMember("save", new SaveMethod(this));
            this.SetMember("count", new CountMethod(this));
        }

        public Collection Collection { get { return this.collection; } }

        private class FindMethod : IFunction
        {
            private CollectionObject self;

            public FindMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                DynamicObject query = null;
                DynamicObject projection = null;

                if (arguments != null && arguments.Count > 0)
                    query = (DynamicObject)arguments[0];
                if (arguments != null && arguments.Count > 1)
                    projection = (DynamicObject)arguments[1];

                return this.self.Collection.Find(query, projection);
            }
        }

        private class FindOneMethod : IFunction
        {
            private CollectionObject self;

            public FindOneMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                DynamicObject query = null;
                DynamicObject projection = null;

                if (arguments != null && arguments.Count > 0)
                    query = (DynamicObject)arguments[0];
                if (arguments != null && arguments.Count > 1)
                    projection = (DynamicObject)arguments[1];

                return this.self.Collection.FindOne(query, projection);
            }
        }

        private class CountMethod : IFunction
        {
            private CollectionObject self;

            public CountMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                DynamicObject query = null;

                if (arguments != null && arguments.Count > 0)
                    query = (DynamicObject)arguments[0];

                return this.self.Collection.Count(query);
            }
        }

        private class UpdateMethod : IFunction
        {
            private CollectionObject self;

            public UpdateMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                DynamicObject query = (DynamicObject)arguments[0];
                DynamicObject update = (DynamicObject)arguments[1];
                bool multi = false;

                if (arguments.Count > 2)
                    multi = (bool)arguments[2];

                this.self.Collection.Update(query, update, multi);

                return null;
            }
        }

        private class RemoveMethod : IFunction
        {
            private CollectionObject self;

            public RemoveMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                DynamicObject query = null;
                bool justone = false;
                
                if (arguments.Count > 0)
                    query = (DynamicObject)arguments[0];
                if (arguments.Count > 1)
                    justone = (bool)arguments[1];

                this.self.Collection.Remove(query, justone);

                return null;
            }
        }

        private class InsertMethod : IFunction
        {
            private CollectionObject self;

            public InsertMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                IObject dobj = (IObject)arguments[0];

                DynamicDocument doc = new DynamicDocument();

                foreach (var name in dobj.GetMemberNames())
                    doc.SetMember(name, dobj.GetMember(name));

                if (doc.Id == null)
                    doc.Id = Guid.NewGuid();

                this.self.Collection.Insert(doc);

                return null;
            }
        }

        private class SaveMethod : IFunction
        {
            private CollectionObject self;

            public SaveMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                IObject dobj = (IObject)arguments[0];

                DynamicDocument doc = new DynamicDocument();

                foreach (var name in dobj.GetMemberNames())
                    doc.SetMember(name, dobj.GetMember(name));

                if (doc.Id == null)
                    doc.Id = Guid.NewGuid();

                this.self.Collection.Save(doc);

                return null;
            }
        }
    }
}
