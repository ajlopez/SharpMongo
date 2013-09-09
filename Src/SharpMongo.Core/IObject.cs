namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;

    public interface IObject
    {
        object GetMember(string name);

        void SetMember(string name, object value);

        IEnumerable<string> GetMemberNames();
    }
}
