using DXC.Technology.Configuration;
using DXC.Technology.UnitTesting.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.UnitTesting
{
    [TestClass]
    public class TechnologyTestSetup
    {
        [AssemblyInitialize]
        public static void Init(TestContext _)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            AppSettingsHelper.Initialize(config);
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            TestStatisticsHelper.Current.ReportResults();
        }

        [TestMethod]
        public void Can_Read_Configuration()
        {
            var apiUrl = AppSettingsHelper.GetAsString("TechnologySettings:UnitTestingRoot");
            Assert.AreEqual("https://localhost/test-api", apiUrl);
        }
    }
}
