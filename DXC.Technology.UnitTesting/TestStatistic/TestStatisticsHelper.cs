using DXC.Technology.Publishers;
using System;
using System.Collections.Generic;
using System.Linq;
using static DXC.Technology.UnitTesting.Helpers.UnitTestOverviewDataSet;

namespace DXC.Technology.UnitTesting.Helpers
{
    public class TestStatisticsHelper
    {
        private static readonly Lazy<TestStatisticsHelper> _instance = new Lazy<TestStatisticsHelper>(() => new TestStatisticsHelper());
        private readonly object _lock = new object();

        public static TestStatisticsHelper Current => _instance.Value;

        public List<UnitTestResult> UnitTestResults { get; set; } = new List<UnitTestResult>();

        private TestStatisticsHelper() { }

        public UnitTestResult GetStatisticRecordCreateOneIfNull(string testType, string testClass, string testMethod)
        {
            lock (_lock)
            {
                var record = UnitTestResults.FirstOrDefault(r => r.TestType == testType && r.TestClass == testClass && r.TestMethod == testMethod);
                if (record == null)
                {
                    record = new UnitTestResult
                    {
                        TestType = testType,
                        TestClass = testClass,
                        TestMethod = testMethod,
                        TotalTestTypeCount = 0,
                        TotalTestClassCount = 0,
                        TotalTestMethodCount = 0,
                        TotalTestMethodPassedCount = 0,
                        TotalTestMethodFailedCount = 0,
                        TotalTestMethodTimeoutCount = 0,
                        TotalAssertsCount = 0,
                        TotalAssertsFailedCount = 0,
                        AreEqualCount = 0,
                        AreEqualFailedCount = 0,
                        AreNotEqualCount = 0,
                        AreNotEqualFailedCount = 0,
                        AreNotSameCount = 0,
                        AreNotSameFailedCount = 0,
                        AreSameCount = 0,
                        AreSameFailedCount = 0,
                        ExceptionAssertCount = 0,
                        FailCount = 0,
                        FailFailedCount = 0,
                        InconclusiveCount = 0,
                        IsFalseCount = 0,
                        IsFalseFailedCount = 0,
                        IsInstanceOfTypeCount = 0,
                        IsInstanceOfTypeFailedCount = 0,
                        IsNotInstanceOfTypeCount = 0,
                        IsNotInstanceOfTypeFailedCount = 0,
                        IsNotNullCount = 0,
                        IsNotNullFailedCount = 0,
                        IsNullCount = 0,
                        IsNullFailedCount = 0,
                        IsTrueCount = 0,
                        IsTrueFailedCount = 0,
                        StartDateTime = DateTime.MinValue,
                        EndDateTime = DateTime.MinValue,
                        DurationInMilliSeconds = 0
                    };
                    UnitTestResults.Add(record);
                }
                return record;
            }
        }

    }
}
