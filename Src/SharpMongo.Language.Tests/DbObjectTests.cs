namespace SharpMongo.Language.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Core;

    [TestClass]
    public class DbObjectTests
    {
        [TestMethod]
        public void GetNewCollection()
        {
            DocumentBase dbase = new DocumentBase("Genesis");
            DbObject dobj = new DbObject(dbase);

            var result = dobj.GetMember("People");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CollectionObject));

            var collobj = (CollectionObject)result;

            Assert.IsNotNull(collobj.Collection);
            Assert.AreEqual("People", collobj.Collection.Name);
            Assert.AreSame(collobj.Collection, dbase.GetCollection("People"));
        }
    }
}
