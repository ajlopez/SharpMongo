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
        public void Exists()
        {
            DynamicObject dynobj = new DynamicObject("Name", "Adam", "Age", 800, "Mother", null);

            Assert.IsTrue(dynobj.Exists("Name"));
            Assert.IsTrue(dynobj.Exists("Age"));
            Assert.IsTrue(dynobj.Exists("Mother"));
            Assert.IsFalse(dynobj.Exists("Weight"));
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
        public void NoMatchOneLessThanQueryUsingExpression()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lt", new DynamicObject("$add", new object[] { 400, 400 })));

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
        public void MatchOneLessOrEqualThanQueryUsingExpression()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$lte", new DynamicObject("$add", new object[] { 400, 400 })));

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
        public void MatchOneGreaterThanQueryUsingExpression()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$gt", new DynamicObject("$add", new object[] {350, 350})));

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
        public void MatchExistsTrueQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$exists", true));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchExistsTrueQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Weight", new DynamicObject("$exists", true));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchExistsTrueQueryOnNull()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", null);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$exists", true));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchExistsFalseQueryOnNull()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", null);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$exists", false));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchExistsFalseQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Weight", new DynamicObject("$exists", false));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchExistsFalseQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$exists", false));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void MatchTypeStringQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Name", new DynamicObject("$type", typeof(string)));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void MatchTypeIntQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", new DynamicObject("$type", typeof(int)));

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void NoMatchTypeIntQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Name", new DynamicObject("$type", typeof(int)));

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void NoMatchTypeIntQueryUndefinedProperty()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Weight", new DynamicObject("$type", typeof(int)));

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

        [TestMethod]
        public void ProjectNewField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("NewField", 1000);

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NewField"));
            Assert.AreEqual(1000, result.GetMember("NewField"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpression()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("Has800", new DynamicObject("$eq", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("Has800"));
            Assert.AreEqual(true, result.GetMember("Has800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionNotEqual()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("HasNot800", new DynamicObject("$ne", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("HasNot800"));
            Assert.AreEqual(false, result.GetMember("HasNot800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionLessThan()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("LessThan800", new DynamicObject("$lt", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("LessThan800"));
            Assert.AreEqual(false, result.GetMember("LessThan800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionLessThanOrEqual()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("LessThanOrEqual800", new DynamicObject("$lte", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("LessThanOrEqual800"));
            Assert.AreEqual(true, result.GetMember("LessThanOrEqual800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionGreaterThan()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("GreaterThan600", new DynamicObject("$gt", new object[] { "$Age", 600 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("GreaterThan600"));
            Assert.AreEqual(true, result.GetMember("GreaterThan600"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionGreaterThanOrEqual()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("GreaterThanOrEqual800", new DynamicObject("$gte", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("GreaterThanOrEqual800"));
            Assert.AreEqual(true, result.GetMember("GreaterThanOrEqual800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionAddFieldNumber()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("NextAge", new DynamicObject("$add", new object[] { "$Age", 1 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NextAge"));
            Assert.AreEqual(801, result.GetMember("NextAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }
        
        [TestMethod]
        public void ProjectNewFieldWithExpressionAddNumberField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("NextAge", new DynamicObject("$add", new object[] { 1, "$Age" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NextAge"));
            Assert.AreEqual(801, result.GetMember("NextAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionAddManyNumbersField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("NewAge", new DynamicObject("$add", new object[] { 1, 2, 3, "$Age" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NewAge"));
            Assert.AreEqual(806, result.GetMember("NewAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionSubtractFieldNumber()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("PreviousAge", new DynamicObject("$subtract", new object[] { "$Age", 1 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("PreviousAge"));
            Assert.AreEqual(799, result.GetMember("PreviousAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionSubtractNumberField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("PreviousAge", new DynamicObject("$subtract", new object[] { 1, "$Age" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("PreviousAge"));
            Assert.AreEqual(-799, result.GetMember("PreviousAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionMultiplyFieldNumber()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("DoubleAge", new DynamicObject("$multiply", new object[] { "$Age", 2 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("DoubleAge"));
            Assert.AreEqual(1600, result.GetMember("DoubleAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionMultiplyNumberField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("DoubleAge", new DynamicObject("$multiply", new object[] { 2, "$Age" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("DoubleAge"));
            Assert.AreEqual(1600, result.GetMember("DoubleAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionMultiplyManyNumbersField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("MultipleAge", new DynamicObject("$multiply", new object[] { 1, 2, 3, 4, "$Age" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("MultipleAge"));
            Assert.AreEqual(800 * 24, result.GetMember("MultipleAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionDivideFieldNumber()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("HalfAge", new DynamicObject("$divide", new object[] { "$Age", 2 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("HalfAge"));
            Assert.AreEqual(400.0, result.GetMember("HalfAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionDivideNumberField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("Two", new DynamicObject("$divide", new object[] { 1600, "$Age" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("Two"));
            Assert.AreEqual(2.0, result.GetMember("Two"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionModFieldNumber()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("ModAge", new DynamicObject("$mod", new object[] { "$Age", 3 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("ModAge"));
            Assert.AreEqual(800 % 3, result.GetMember("ModAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionModNumberField()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("ModAge", new DynamicObject("$mod", new object[] { 1000, "$Age" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("ModAge"));
            Assert.AreEqual(200, result.GetMember("ModAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithNestedAddExpressions()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("NewAge", new DynamicObject("$add", new object[] { new DynamicObject("$add", new object[] { "$Age", 1 }), 2 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NewAge"));
            Assert.AreEqual(803, result.GetMember("NewAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithTwoNestedExpressions()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("NewAge", new DynamicObject("$add", new object[] { new DynamicObject("$add", new object[] { "$Age", 1 }), 2, new DynamicObject("$subtract", new object[] { 5, 4 }) }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NewAge"));
            Assert.AreEqual(804, result.GetMember("NewAge"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithConcatExpression()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("FullName", new DynamicObject("$concat", new object[] { "$Name", " Smith" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("FullName"));
            Assert.AreEqual("Adam Smith", result.GetMember("FullName"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithCmpExpressions()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject(
                "EqualName", 
                new DynamicObject("$cmp", new object[] { "$Name", "Adam" }),
                "GreaterName", 
                new DynamicObject("$cmp", new object[] { "$Name", "Abel" }),
                "LessName", 
                new DynamicObject("$cmp", new object[] { "$Name", "Eve" }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("EqualName"));
            Assert.AreEqual(0, result.GetMember("EqualName"));
            Assert.IsNotNull(result.GetMember("LessName"));
            Assert.AreEqual(-1, result.GetMember("LessName"));
            Assert.IsNotNull(result.GetMember("GreaterName"));
            Assert.AreEqual(1, result.GetMember("GreaterName"));
            Assert.AreEqual(5, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithLiteralExpression()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("Literal", new DynamicObject("$literal", "$Age"));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("Literal"));
            Assert.AreEqual("$Age", result.GetMember("Literal"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithToUpperToLowerExpressions()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject(
                "NameToUpper", 
                new DynamicObject("$toupper", "$Name"), 
                "NameToLower", 
                new DynamicObject("$tolower", "$Name"));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NameToUpper"));
            Assert.AreEqual("ADAM", result.GetMember("NameToUpper"));
            Assert.IsNotNull(result.GetMember("NameToLower"));
            Assert.AreEqual("adam", result.GetMember("NameToLower"));
            Assert.AreEqual(4, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectExcludingAge()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject projection = new DynamicObject("Age", false);

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNull(result.GetMember("Age"));
            Assert.AreEqual(1, result.GetMemberNames().Count());
            Assert.AreEqual("Name", result.GetMemberNames().First());
        }
    }
}
