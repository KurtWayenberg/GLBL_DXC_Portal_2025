using DXC.Technology.Configuration;
using DXC.Technology.UnitTesting.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DXC.Technology.Utilities.Tests
{
    /// <summary>
    /// Test class for 'Boolean'
    /// </summary>
    [TestClass]
    public class BooleanTests : TestingClassBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgeTests"/> class.
        /// </summary>
        public BooleanTests() : base(UnitTesting.Enumerations.TechnologyUnitTestTypeEnum.Core)
        {
            TestClassName = GetType().Name;
        }

        [ClassInitialize]
        public static void Init(TestContext _)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            AppSettingsHelper.Initialize(config);
        }
        /// <summary>
        /// A test for ConvertToString with default format
        /// </summary>
        [TestMethod()]
        public void ConvertToString_DefaultFormat_Test()
        {
            string resultTrue = Boolean.ConvertToString(true);
            TechnologyAssertHelper.IsTrue(resultTrue == "True", "ConvertToString should return 'True' for true value with default format");

            string resultFalse = Boolean.ConvertToString(false);
            TechnologyAssertHelper.IsTrue(resultFalse == "False", "ConvertToString should return 'False' for false value with default format");
        }

        /// <summary>
        /// A test for ConvertToString with custom format
        /// </summary>
        [TestMethod()]
        public void ConvertToString_CustomFormat_Test()
        {
            string resultTrue = Boolean.ConvertToString(true, "1/0");
            TechnologyAssertHelper.IsTrue(resultTrue == "1", "ConvertToString should return '1' for true value with '1/0' format");

            string resultFalse = Boolean.ConvertToString(false, "1/0");
            TechnologyAssertHelper.IsTrue(resultFalse == "0", "ConvertToString should return '0' for false value with '1/0' format");
        }

        /// <summary>
        /// A test for ConvertFromString with default format
        /// </summary>
        [TestMethod()]
        public void ConvertFromString_DefaultFormat_Test()
        {
            bool resultTrue = Boolean.ConvertFromString("True", BooleanFormatEnum.Boolean);
            TechnologyAssertHelper.IsTrue(resultTrue, "ConvertFromString should return true for 'True' with default format");

            bool resultFalse = Boolean.ConvertFromString("False", BooleanFormatEnum.Boolean);
            TechnologyAssertHelper.IsTrue(!resultFalse, "ConvertFromString should return false for 'False' with default format");
        }

        /// <summary>
        /// A test for ConvertFromString with custom format
        /// </summary>
        [TestMethod()]
        public void ConvertFromString_CustomFormat_Test()
        {
            bool resultTrue = Boolean.ConvertFromString("1", "1/0");
            TechnologyAssertHelper.IsTrue(resultTrue, "ConvertFromString should return true for '1' with '1/0' format");

            bool resultFalse = Boolean.ConvertFromString("0", "1/0");
            TechnologyAssertHelper.IsTrue(!resultFalse, "ConvertFromString should return false for '0' with '1/0' format");
        }

        /// <summary>
        /// A test for invalid custom format
        /// </summary>
        [TestMethod()]
        public void ConvertToString_InvalidCustomFormat_Test()
        {
            try
            {
                Boolean.ConvertToString(true, "InvalidFormat");
                TechnologyAssertHelper.IsTrue(false, "ConvertToString should throw exception for invalid format");
            }
            catch (DXC.Technology.Exceptions.NamedExceptions.ToBeImplementedException)
            {
                TechnologyAssertHelper.IsTrue(true, "ConvertToString correctly throws exception for invalid format");
            }
        }
    }
}