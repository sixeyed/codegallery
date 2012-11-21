using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sixeyed.CodeSamples.EntityTagger.Tests
{
    [TestClass]
    public class EntityTaggerTests
    {
        [TestMethod]
        public void SetETag()
        {
            var user = GetUser();
            EntityTagger.SetETag(user, x => x.ETag);
            AssertETag(user.ETag);
            var oldETag = user.ETag;
            user.DateOfBirth = user.DateOfBirth.AddMilliseconds(1);
            EntityTagger.SetETag(user, x => x.ETag);
            AssertETag(user.ETag);
            Assert.AreNotEqual(oldETag, user.ETag);
        }

        [TestMethod]
        public void HasChanges()
        {
            var user = GetUser();
            EntityTagger.SetETag(user, x => x.ETag);
            var oldETag = user.ETag;
            user.Name = Guid.NewGuid().ToString();
            Assert.IsTrue(EntityTagger.HasChanges(user, x=>x.ETag));
            Assert.AreEqual(oldETag, user.ETag);
        }

        [TestMethod]
        public void ResetETag()
        {
            var user = GetUser();
            EntityTagger.SetETag(user, x => x.ETag);
            var oldETag = user.ETag;
            EntityTagger.ResetETag(user, x => x.ETag);
            Assert.AreEqual(Guid.Empty.ToString(), user.ETag);
            Assert.AreNotEqual(oldETag, user.ETag);
        }

        private static User GetUser()
        {
            var user = new User();
            user.DateOfBirth = new DateTime(1980, 01, 10);
            user.Name = Guid.NewGuid().ToString();
            user.ETag = string.Empty;
            return user;
        }

        private static void AssertETag(string eTag)
        {
            Assert.IsNotNull(eTag);
            Assert.IsFalse(string.IsNullOrEmpty(eTag));
            Guid guid = Guid.Empty;
            Guid.TryParse(eTag, out guid);
            Assert.AreNotEqual(Guid.Empty, guid);
        }
    }

    public class User
    {
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string ETag { get; set; }
    }
}
