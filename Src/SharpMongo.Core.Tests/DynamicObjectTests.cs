namespace SharpMongo.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicObjectTests
    {
        [TestMethod]
        public void CreateAndGetUndefinedMember()
        {
            DynamicObject document = new DynamicObject();

            Assert.IsNull(document.GetMember("Name"));
        }

        [TestMethod]
        public void GetDefinedMember()
        {
            DynamicObject document = new DynamicObject("Name", "Adam");

            Assert.AreEqual("Adam", document.GetMember("Name"));
        }

        [TestMethod]
        public void Seal()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            document.Seal();

            try
            {
                document.SetMember("Age", 700);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void SetAndGetMember()
        {
            DynamicObject document = new DynamicObject();

            document.SetMember("Name", "Eve");

            Assert.AreEqual("Eve", document.GetMember("Name"));
        }

        [TestMethod]
        public void MatchOnePropertyQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", 800);

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void MatchOneLessThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lt", 900));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchOneLessThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lt", 800));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchOneLessOrEqualThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lte", 800));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchOneLessOrEqualThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lte", 700));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchOneGreaterThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$gt", 700));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchOneGreaterThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$gt", 800));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchOneGreaterOrEqualThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$gte", 800));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchOneGreaterOrEqualThanQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$gte", 801));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchRangeQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lt", 900, "$gt", 700));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchRangeQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lt", 800, "$gt", 700));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchNotEqualQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$ne", 700));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchNotEqualQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$ne", 800));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchEqualQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$eq", 800));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchEqualQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$eq", 700));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchInQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$in", new int[] { 700, 600, 800, 900 }));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchInQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$in", new int[] { 700, 600, 801, 900 }));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchNotInQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$nin", new int[] { 700, 600, 900 }));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchNotInQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$nin", new int[] { 700, 600, 800, 900 }));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchNotQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$not", new DynamicObject("$gt", 900)));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchNotQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$not", new DynamicObject("$lt", 900)));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchOrQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$or", new DynamicObject[] { new DynamicObject("$gt", 900), new DynamicObject("$lt", 1000) }));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchOrQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$or", new DynamicObject[] { new DynamicObject("$gt", 900), new DynamicObject("$lt", 700) }));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchNorQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$nor", new DynamicObject[] { new DynamicObject("$gt", 900), new DynamicObject("$lt", 700) }));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchNorQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$nor", new DynamicObject[] { new DynamicObject("$gt", 900), new DynamicObject("$lt", 1000) }));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchAndQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$and", new DynamicObject[] { new DynamicObject("$gt", 700), new DynamicObject("$lt", 1000) }));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchAndQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$and", new DynamicObject[] { new DynamicObject("$gt", 900), new DynamicObject("$lt", 700) }));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void InvalidMatchOperator()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$foo", 700));

            try
            {
                query.Match(document);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("Invalid operator '$foo'", ex.Message);
            }
        }

        [TestMethod]
        public void DontMatchOnePropertyQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", 700);

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void UpdateExistingProperty()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject update = new DynamicObject("Age", 700);

            document.Update(update);

            Assert.AreEqual(700, document.GetMember("Age"));
        }

        [TestMethod]
        public void UpdateNotExistingProperty()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject update = new DynamicObject("Height", 180);

            document.Update(update);

            Assert.AreEqual(180, document.GetMember("Height"));
        }

        [TestMethod]
        public void UpdateProperties()
        {
            DynamicObject obj = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject update = new DynamicObject("Height", 180, "Age", 700);

            obj.Update(update);

            Assert.AreEqual(180, obj.GetMember("Height"));
            Assert.AreEqual(700, obj.GetMember("Age"));
        }

        [TestMethod]
        public void GetMemberNames()
        {
            DynamicObject obj = new DynamicObject("Name", "Adam", "Age", 800, "Wife", "Eve");

            var result = obj.GetMemberNames().ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Name", result[0]);
            Assert.AreEqual("Age", result[1]);
            Assert.AreEqual("Wife", result[2]);
        }

        [TestMethod]
        public void ToJsonString()
        {
            DynamicObject obj = new DynamicObject("Name", "Adam", "Age", 800, "Wife", "Eve");

            var result = obj.ToJsonString();

            Assert.IsNotNull(result);
            Assert.AreEqual("{ \"Name\": \"Adam\", \"Age\": 800, \"Wife\": \"Eve\" }", result);
        }

        [TestMethod]
        public void EmptyToJsonString()
        {
            DynamicObject obj = new DynamicObject();

            var result = obj.ToJsonString();

            Assert.IsNotNull(result);
            Assert.AreEqual("{ }", result);
        }
    }
}
