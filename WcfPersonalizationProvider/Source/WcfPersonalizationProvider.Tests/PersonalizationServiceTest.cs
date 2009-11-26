using WcfPersonalizationProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
namespace WcfPersonalizationProvider.Tests
{
    
    
    /// <summary>
    ///This is a test class for PersonalizationServiceTest and is intended
    ///to contain all PersonalizationServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PersonalizationServiceTest
    {
        private const string PATH = "~/Pages/Home.aspx";
        private const string USERNAME = "auefho";

        private const string SHARED_BLOB = "eisufhesiufh e e  eiuheisfsiufhsiugisgiugsiio";
        private const string USER_BLOB = "iofjaw omfasirjm oijoeifjeos mjgosjg msoe";

        /// <summary>
        ///A test for GetSharedPersonalizationBlob
        ///</summary>
        [TestMethod()]
        public void GetSharedPersonalizationBlob()
        {
            PersonalizationService target = new PersonalizationService();
            string path = PATH;
            string userName = null;
            string expected = SHARED_BLOB;
            byte[] actual = target.GetSharedPersonalizationBlob(path, userName);
            Assert.AreEqual(expected, GetString(actual));
        }

        private static byte[] GetBytes(string text)
        {
            return Encoding.Default.GetBytes(text);
        }

        private static string GetString(byte[] text)
        {
            return Encoding.Default.GetString(text);
        }

        /// <summary>
        ///A test for GetUserPersonalizationBlob
        ///</summary>
        [TestMethod()]
        public void GetUserPersonalizationBlob()
        {
            PersonalizationService target = new PersonalizationService();
            string path = PATH;
            string userName = USERNAME;
            string expected = USER_BLOB;
            byte[] actual = target.GetUserPersonalizationBlob(path, userName);
            Assert.AreEqual(expected, GetString(actual));
        }

        /// <summary>
        ///A test for SavePersonalizationBlob
        ///</summary>
        [TestMethod()]
        public void SavePersonalizationBlob_Shared()
        {
            PersonalizationService target = new PersonalizationService();
            string path = PATH;
            string userName = null;
            byte[] dataBlob = GetBytes(SHARED_BLOB);
            target.SavePersonalizationBlob(path, userName, dataBlob);
        }

        [TestMethod()]
        public void SavePersonalizationBlob_User()
        {
            PersonalizationService target = new PersonalizationService();
            string path = PATH;
            string userName = USERNAME;
            byte[] dataBlob = GetBytes(USER_BLOB);
            target.SavePersonalizationBlob(path, userName, dataBlob);
        }
    }
}
