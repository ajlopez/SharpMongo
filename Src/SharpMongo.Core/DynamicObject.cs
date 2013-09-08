namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicObject : SharpMongo.Core.IObject
    {
        private IList<string> names = new List<string>();
        private IDictionary<string, object> values = new Dictionary<string, object>();
        private bool @sealed;

        public DynamicObject(params object[] arguments)
        {
            for (int k = 0; k < arguments.Length; k += 2)
            {
                var name = arguments[k].ToString();
                this.names.Add(name);
                this.values[name] = arguments[k + 1];
            }
        }

        public virtual object GetMember(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return null;
        }

        public virtual void SetMember(string name, object value)
        {
            if (this.@sealed)
                throw new InvalidOperationException();

            if (!this.names.Contains(name))
                this.names.Add(name);

            this.values[name] = value;
        }

        public bool Match(DynamicObject document)
        {
            foreach (var key in this.values.Keys)
                if (!this.values[key].Equals(document.GetMember(key)))
                    return false;

            return true;
        }

        public void Update(DynamicObject document)
        {
            foreach (var key in document.values.Keys)
                this.values[key] = document.values[key];
        }

        public void Seal()
        {
            this.@sealed = true;
        }

        public IEnumerable<string> GetMemberNames()
        {
            return this.names;
        }
    }
}
