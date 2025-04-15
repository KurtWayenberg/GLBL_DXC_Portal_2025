using DXC.Technology.Publishers;
using DXC.Technology.UnitTesting.TestStatistic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXC.Technology.UnitTesting.Helpers
{
    public class TestStatisticsHelper
    {
        private static readonly Lazy<TestStatisticsHelper> _instance = new Lazy<TestStatisticsHelper>(() => new TestStatisticsHelper());
        private readonly object _lock = new object();

        public static TestStatisticsHelper Current => _instance.Value;

        public List<UnitTestResult> UnitTestResults { get; set; } = new List<UnitTestResult>();
        public List<UnitTestSummaryResult> UnitTestSummaryResults { get; set; } = new List<UnitTestSummaryResult>();

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

        public void LogAssertCall(string testType, string testClass, string testMethod, AssertCallTypeEnum assertCallType, bool isFailure)
        {
            lock (_lock)
            {
                var record = GetStatisticRecordCreateOneIfNull(testType, testClass, testMethod);
                record.LogAssertCall(assertCallType, isFailure);
            }
        }

        public void SummarizeUnitTestResultsInUnitTestSummaryResults()
        {
            lock (_lock)
            {
                UnitTestSummaryResults.Clear();

                var groupedResults = UnitTestResults
                    .GroupBy(r => new { r.TestType, r.TestClass })
                    .Select(g => new UnitTestSummaryResult
                    {
                        TestType = g.Key.TestType,
                        TestClass = g.Key.TestClass,
                        TotalTestTypeCount = g.Count(),
                        TotalTestClassCount = g.Count(),
                        TotalTestMethodCount = g.Select(r => r.TestMethod).Distinct().Count(),
                        TotalTestMethodPassedCount = g.Count(r => r.OverallStatus == UnitTestOutcome.Passed),
                        TotalTestMethodFailedCount = g.Count(r => r.OverallStatus == UnitTestOutcome.Failed),
                        TotalTestMethodTimeoutCount = g.Count(r => r.OverallStatus == UnitTestOutcome.Timeout),
                        TotalAssertsCount = g.Sum(r => r.TotalAssertsCount),
                        TotalAssertsFailedCount = g.Sum(r => r.TotalAssertsFailedCount),
                        OverallStatus = g.All(r => r.OverallStatus == UnitTestOutcome.Passed) ? UnitTestOutcome.Passed : UnitTestOutcome.Failed,
                        TotalDurationInMilliSeconds = g.Sum(r => r.DurationInMilliSeconds)
                    }).ToList();

                UnitTestSummaryResults.AddRange(groupedResults);

                var overallSummary = new UnitTestSummaryResult
                {
                    TestType = "Overall",
                    TestClass = "All",
                    TotalTestTypeCount = UnitTestResults.Select(r => r.TestType).Distinct().Count(),
                    TotalTestClassCount = UnitTestResults.Select(r => r.TestClass).Distinct().Count(),
                    TotalTestMethodCount = UnitTestResults.Select(r => r.TestMethod).Distinct().Count(),
                    TotalTestMethodPassedCount = UnitTestResults.Count(r => r.OverallStatus == UnitTestOutcome.Passed),
                    TotalTestMethodFailedCount = UnitTestResults.Count(r => r.OverallStatus == UnitTestOutcome.Failed),
                    TotalTestMethodTimeoutCount = UnitTestResults.Count(r => r.OverallStatus == UnitTestOutcome.Timeout),
                    TotalAssertsCount = UnitTestResults.Sum(r => r.TotalAssertsCount),
                    TotalAssertsFailedCount = UnitTestResults.Sum(r => r.TotalAssertsFailedCount),
                    OverallStatus = UnitTestResults.All(r => r.OverallStatus == UnitTestOutcome.Passed) ? UnitTestOutcome.Passed : UnitTestOutcome.Failed,
                    TotalDurationInMilliSeconds = UnitTestResults.Sum(r => r.DurationInMilliSeconds)
                };

                UnitTestSummaryResults.Add(overallSummary);

                var testTypeSummaries = UnitTestResults
                    .GroupBy(r => r.TestType)
                    .Select(g => new UnitTestSummaryResult
                    {
                        TestType = g.Key,
                        TestClass = "All",
                        TotalTestTypeCount = g.Count(),
                        TotalTestClassCount = g.Select(r => r.TestClass).Distinct().Count(),
                        TotalTestMethodCount = g.Select(r => r.TestMethod).Distinct().Count(),
                        TotalTestMethodPassedCount = g.Count(r => r.OverallStatus == UnitTestOutcome.Passed),
                        TotalTestMethodFailedCount = g.Count(r => r.OverallStatus == UnitTestOutcome.Failed),
                        TotalTestMethodTimeoutCount = g.Count(r => r.OverallStatus == UnitTestOutcome.Timeout),
                        TotalAssertsCount = g.Sum(r => r.TotalAssertsCount),
                        TotalAssertsFailedCount = g.Sum(r => r.TotalAssertsFailedCount),
                        OverallStatus = g.All(r => r.OverallStatus == UnitTestOutcome.Passed) ? UnitTestOutcome.Passed : UnitTestOutcome.Failed,
                        TotalDurationInMilliSeconds = g.Sum(r => r.DurationInMilliSeconds)
                    }).ToList();

                UnitTestSummaryResults.AddRange(testTypeSummaries);
            }
        }

        public IEnumerable<UnitTestSummaryResult> GetSummaryResults()
        {
            lock (_lock)
            {
                return UnitTestSummaryResults;
            }
        }

        internal void ReportTestOutCome(TestContext testContext)
        {
            lock (_lock)
            {
                var record = GetStatisticRecordCreateOneIfNull(
                    TestContextPropertiesHelper.GetUnitTestType(testContext).ToString(),
                    TestContextPropertiesHelper.GetTestClassName(testContext),
                    TestContextPropertiesHelper.GetMethodName(testContext)
                );
                record.LogEnd(testContext.CurrentTestOutcome, TestContextPropertiesHelper.GetComment(testContext));
            }
        }

        internal void ReportResults()
        {
            string unitTestingRoot = DXC.Technology.Configuration.AppSettingsHelper.GetAsString("TechnologySettings:UnitTestingRoot");
            string timestamp = DXC.Technology.Utilities.Date.ToYYYYMMDDHHMMSSFFFString(DateTime.Now);
            string currentUnitTestResults = $"{unitTestingRoot}/Files/DetailUnitTestResults{timestamp}.json";
            string currentUnitTestSummaryResults = $"{unitTestingRoot}/Files/DetailUnitTestSummaryResults{timestamp}.json";
            string currentUnitTestUserReport = $"{unitTestingRoot}/Files/UserReport{timestamp}.json";

            //First refresh the current results
            TestStatisticsHelper.Current.SummarizeUnitTestResultsInUnitTestSummaryResults();

            //Then Write the current results to the file
            {
                //Serialize the UnitTestResults and UnitTestSummaryResults to JSON
                string unitTestResultsJson = Newtonsoft.Json.JsonConvert.SerializeObject(UnitTestResults);
                //Write the file to currentUnitTestResults     
                System.IO.File.WriteAllText(currentUnitTestResults, unitTestResultsJson);
                //Serialize the UnitTestSummaryResults to JSON
                string unitTestSummaryResultsJson = Newtonsoft.Json.JsonConvert.SerializeObject(UnitTestSummaryResults);
                //Write the file to currentUnitTestSummaryResults
                System.IO.File.WriteAllText(currentUnitTestSummaryResults, unitTestSummaryResultsJson);



            }
            string totalsUnitTestResults = unitTestingRoot + "TotalsUnitTestResults.json";
            string totalsUnitTestSummaryResults = unitTestingRoot + "TotalsUnitTestSummaryResults.json";

            //First check if the totalsUnitTestResults file exists and if it does read it
            if (System.IO.File.Exists(totalsUnitTestResults))
            {
                string unitTestResultsJson = System.IO.File.ReadAllText(totalsUnitTestResults);
                // If the file is empty, set it to an empty JSON array
                if (string.IsNullOrEmpty(unitTestResultsJson)) unitTestResultsJson = "[]";
                var previousUnitTestResults = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UnitTestResult>>(unitTestResultsJson)!;
                //Merge the previous results with the current results. Records are updated based on the TestType, TestClass and TestMethod
                foreach (var previousRecord in previousUnitTestResults)
                {
                    var currentRecord = UnitTestResults.FirstOrDefault(r => r.TestType == previousRecord.TestType && r.TestClass == previousRecord.TestClass && r.TestMethod == previousRecord.TestMethod);
                    if (currentRecord == null)
                    {
                        UnitTestResults.Add(previousRecord);
                    }
                }
            }

            TestStatisticsHelper.Current.SummarizeUnitTestResultsInUnitTestSummaryResults();

            //Write the results to the file
            {
                //Serialize the UnitTestResults and UnitTestSummaryResults to JSON
                string unitTestResultsJson = Newtonsoft.Json.JsonConvert.SerializeObject(UnitTestResults);
                //Write the file to totalsUnitTestResults     
                System.IO.File.WriteAllText(totalsUnitTestResults, unitTestResultsJson);
                //Serialize the UnitTestSummaryResults to JSON
                string unitTestSummaryResultsJson = Newtonsoft.Json.JsonConvert.SerializeObject(UnitTestSummaryResults);
                //Write the file to totalsUnitTestSummaryResults
                System.IO.File.WriteAllText(totalsUnitTestSummaryResults, unitTestSummaryResultsJson);
            }

            //    UnitTestResults
            //           UnitTestSummaryResults 

            //TestStatisticsHelper.Current.UpdateSummaryResultFile();
            //    UnitTestOverviewDataSet dsTotals = new UnitTestOverviewDataSet();
            //    string totalsXMLFile = unitTestingRoot + "Totals.xml";
            //    if (System.IO.File.Exists(totalsXMLFile))
            //        dsTotals.ReadXml(totalsXMLFile);
            //    UnitTestOverview.UpdateTotals(dsTotals);
            //    dsTotals.WriteTotalResults();
            //    if (System.IO.File.Exists(totalsXMLFile))
            //        System.IO.File.Delete(totalsXMLFile);
            //    dsTotals.WriteXml(totalsXMLFile);

            //    TestStatisticsHelper.Current.GetSummaryResults()

        }
    }
}
