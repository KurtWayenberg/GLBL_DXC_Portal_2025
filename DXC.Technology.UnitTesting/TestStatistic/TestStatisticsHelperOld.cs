//using DXC.Technology.Publishers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static DXC.Technology.UnitTesting.Helpers.UnitTestOverviewDataSet;

//namespace DXC.Technology.UnitTesting.Helpers
//{
//    public class TestStatisticsHelper
//    {
//        private static readonly Lazy<TestStatisticsHelper> _instance = new Lazy<TestStatisticsHelper>(() => new TestStatisticsHelper());
//        private readonly object _lock = new object();

//        public static TestStatisticsHelper Current => _instance.Value;

//        public List<UnitTestResult> UnitTestResults { get; set; } = new List<UnitTestResult>();
//        public List<ThreadStatistic> ThreadStatistics { get; set; } = new List<ThreadStatistic>();

//        private TestStatisticsHelper() { }

//        public UnitTestResult GetStatisticRecordCreateOneIfNull(string testType, string testClass, string testMethod)
//        {
//            lock (_lock)
//            {
//                var record = UnitTestResults.FirstOrDefault(r => r.TestType == testType && r.TestClass == testClass && r.TestMethod == testMethod);
//                if (record == null)
//                {
//                    record = new UnitTestResult
//                    {
//                        TestType = testType,
//                        TestClass = testClass,
//                        TestMethod = testMethod,
//                        TotalTestTypeCount = 0,
//                        TotalTestClassCount = 0,
//                        TotalTestMethodCount = 0,
//                        TotalTestMethodPassedCount = 0,
//                        TotalTestMethodFailedCount = 0,
//                        TotalTestMethodTimeoutCount = 0,
//                        TotalAssertsCount = 0,
//                        TotalAssertsFailedCount = 0,
//                        AreEqualCount = 0,
//                        AreEqualFailedCount = 0,
//                        AreNotEqualCount = 0,
//                        AreNotEqualFailedCount = 0,
//                        AreNotSameCount = 0,
//                        AreNotSameFailedCount = 0,
//                        AreSameCount = 0,
//                        AreSameFailedCount = 0,
//                        ExceptionAssertCount = 0,
//                        FailCount = 0,
//                        FailFailedCount = 0,
//                        InconclusiveCount = 0,
//                        IsFalseCount = 0,
//                        IsFalseFailedCount = 0,
//                        IsInstanceOfTypeCount = 0,
//                        IsInstanceOfTypeFailedCount = 0,
//                        IsNotInstanceOfTypeCount = 0,
//                        IsNotInstanceOfTypeFailedCount = 0,
//                        IsNotNullCount = 0,
//                        IsNotNullFailedCount = 0,
//                        IsNullCount = 0,
//                        IsNullFailedCount = 0,
//                        IsTrueCount = 0,
//                        IsTrueFailedCount = 0,
//                        StartDateTime = DateTime.MinValue,
//                        EndDateTime = DateTime.MinValue,
//                        DurationInMilliSeconds = 0
//                    };
//                    UnitTestResults.Add(record);
//                }
//                return record;
//            }
//        }

//        public void WriteTotalResults()
//        {
//            string totalsfolder = string.Concat("Totals_", DXC.Technology.Utilities.Date.NowYYYYMMString());
//            string resultsLocation = TechnologyUnitTestHelper.UnitTestResultsLocation + totalsfolder;

//            if (Directory.Exists(resultsLocation))
//                Directory.Delete(resultsLocation, true);

//            Directory.CreateDirectory(resultsLocation);

//            foreach (string testtype in UnitTestResults.Select(p => p.TestType).Distinct().Where(p => !p.Equals("*")))
//            {
//                List<DXC.Technology.UnitTesting.Helpers.UnitTestResult> unittestresultextract = new();
//                double counter = 0;
//                double success = 0;

//                foreach (var row in UnitTestResults.Where(p => p.TestType == testtype))
//                {
//                    counter += row.TotalTestMethodCount;
//                    success += row.TotalTestMethodPassedCount;
//                    unittestresultextract.Add(row);
//                }

//                string successrate = Math.Round((success / counter) * 100, 0).ToString().PadLeft(3, '0');
//                string status = ((success - counter) < 1) ? "Passed" : "Failed";

//                StringWriter lsw = new StringWriter();
//                WriteUnitTests(unittestresultextract, lsw);

//                string lFileName = string.Concat(resultsLocation + "\\", testtype, "_", successrate, "Perc_Result_", status, ".txt");
//                FilePublisher lfp = new FilePublisher(lFileName);
//                lfp.Publish(lsw.ToString());
//            }
//        }

//        public void WriteResults()
//        {
//            var summary = UnitTestResults.FirstOrDefault(r => r.TestType == "*" && r.TestClass == "*" && r.TestMethod == "*");
//            double overallsuccessrate = (summary.TotalTestMethodPassedCount * 100) / summary.TotalTestMethodCount;
//            string summaryfolder = string.Concat(
//                "Run_",
//                DXC.Technology.Utilities.Date.ToYYYYMMDDHHMMSSFFFString(DateTime.Now),
//                "_", Math.Round(overallsuccessrate, 0).ToString().PadLeft(3, '0'),
//                "PercOk_Status_",
//                summary.OverallStatus);

//            Directory.CreateDirectory(TechnologyUnitTestHelper.UnitTestResultsLocation + summaryfolder);

//            foreach (string testtype in UnitTestResults.Select(p => p.TestType).Distinct().Where(p => !p.Equals("*")))
//            {
//                List<UnitTestResult> unittestresultextract = new ();
//                double counter = 0;
//                double success = 0;

//                foreach (var row in UnitTestResults.Where(p => p.TestType == testtype))
//                {
//                    counter += row.TotalTestMethodCount;
//                    success += row.TotalTestMethodPassedCount;
//                    unittestresultextract.Add(row);
//                }

//                string successrate = Math.Round((success / counter) * 100, 0).ToString().PadLeft(3, '0');
//                string status = ((success - counter) < 1) ? "Passed" : "Failed";

//                StringWriter lsw = new StringWriter();
//                WriteUnitTests(unittestresultextract, lsw);

//                string lFileName = string.Concat(TechnologyUnitTestHelper.UnitTestResultsLocation + summaryfolder + "\\", testtype, "_", successrate, "Perc_Result_", status, ".txt");
//                FilePublisher lfp = new FilePublisher(lFileName);
//                lfp.Publish(lsw.ToString());
//            }
//        }

//        private void WriteUnitTests(List<DXC.Technology.UnitTesting.Helpers.UnitTestResult> unittestresultextract, StringWriter lsw)
//        {
//            UnitTestResultRow.WriteSummaryTitle(lsw, 0, false);
//            foreach (UnitTestResult lutrRow in unittestresultextract.Where(p => p.TestType =="*"))
//                lutrRow.WriteSummaryResults(lsw, 0, false);
//            lsw.WriteLine();

//            UnitTestResultRow.WriteTestTypeTitle(lsw, 0, false);
//            foreach (UnitTestResult lutrRow in unittestresultextract.Where(p => p.TestType != "*" && p.TestClass =="*"))
//            {
//                lutrRow.WriteTestTypeResults(lsw, 0, false);
//            }
//            lsw.WriteLine();

//            UnitTestResultRow.WriteClassTitle(lsw, 0, false);
//            foreach (UnitTestResult lutrRow in unittestresultextract.Where(p => p.TestType != "*" && p.TestClass == "*"))
//            {
//                foreach (UnitTestResult lutrcRow in unittestresultextract.Where(p => p.TestType == lutrRow.TestType && p.TestClass != "*" && p.TestMethod == "*"))
//                {
//                    lutrcRow.WriteClassResults(lsw, 0, false);
//                }
//            }
//            lsw.WriteLine();

//            UnitTestResultRow.WriteMethodTitle(lsw, 0, false);

//            foreach (UnitTestResult lutrRow in unittestresultextract.Where(p => p.TestType != "*" && p.TestClass == "*"))
//            {
//                foreach (UnitTestResult lutrcRow in unittestresultextract.Where(p => p.TestType == lutrRow.TestType && p.TestClass != "*" && p.TestMethod == "*"))
//                {
//                    foreach (UnitTestResult lutrmRow in unittestresultextract.Where(p => p.TestType == lutrRow.TestType && p.TestClass == lutrcRow.TestClass && p.TestMethod != "*"))
//                    {
//                        lutrmRow.WriteMethodResults(lsw, 0, false);
//                    }
//                }
//            }

//            lsw.WriteLine();

//            UnitTestResultRow.WriteMethodTitle(lsw, 0, false);

//            foreach (UnitTestResult lutrRow in unittestresultextract.Where(p => p.TestType != "*" && p.TestClass == "*"))
//            {
//                foreach (UnitTestResult lutrcRow in unittestresultextract.Where(p => p.TestType == lutrRow.TestType && p.TestClass != "*" && p.TestMethod == "*"))
//                {
//                    foreach (UnitTestResult lutrmRow in unittestresultextract.Where
//                        (p => p.TestType == lutrRow.TestType 
//                        && p.TestClass == lutrcRow.TestClass
//                        && p.TestMethod != "*"
//                        && (p.TotalTestMethodFailedCount > 0 || p.TotalTestMethodTimeoutCount > 0)
//                        ))
//                    {
//                        lutrmRow.WriteMethodResults(lsw, 0, true);
//                        foreach (UnitTestResult lutdRow in unittestresultextract.Where
//                                    (p => p.TestType == lutrmRow.TestType
//                                    && p.TestClass == lutrmRow.TestClass
//                                    && p.TestMethod == lutrmRow.TestMethod))
//                        {
//                            //lutdRow.WriteDetailResults(lsw, 5, false);
//                        }
//                    }
//                }
//            }
//            lsw.WriteLine();

//            UnitTestResultRow.WriteMethodTitle(lsw, 0, false);
//            //foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestClassColumn.ColumnName))
//            //{
//            //    foreach (UnitTestResultRow lutrcRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestMethodColumn.ColumnName, "='*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
//            //    {
//            //        foreach (UnitTestResult lutrmRow in unittestresultextract.Where
//            //                    (p => p.TestType == lutrRow.TestType
//            //                    && p.TestClass == lutrcRow.TestClass
//            //                    ))
//            //        {
//            //            lutrmRow.WriteMethodResults(lsw, 0, true);
//            //            foreach (UnitTestResult lutdRow in unittestresultextract.Where
//            //                        (p => p.TestType == lutrmRow.TestType
//            //                        && p.TestClass == lutrmRow.TestClass
//            //                        && p.TestMethod == lutrmRow.TestMethod
//            //                        && (p.TotalTestMethodFailedCount > 0 || p.TotalTestMethodTimeoutCount > 0)
//            //                        ))
//            //            {
//            //                //lutdRow.WriteDetailResults(lsw, 5, false);
//            //            }
//            //        }
//            //    }
//            //}
//            lsw.WriteLine();
//        }

//        public void UpdateTotals()
//        {
//            RecalculateTotal();
//        }

//        private void RecalculateTotal()
//        {
//            var totalsrow = UnitTestResults.FirstOrDefault(r => r.TestType == "*" && r.TestClass == "*" && r.TestMethod == "*");
//            foreach (var row in UnitTestResults.Where(p => p.TestType != "*" && p.TestClass != "*" && p.TestMethod == "*"))
//            {
//                row.InitializeWithZeroes();
//            }

//            foreach (var row in UnitTestResults.Where(p => p.TestType != "*" && p.TestClass != "*" && p.TestMethod != "*"))
//            {
//                var classtotalsrow = UnitTestResults.FirstOrDefault(r => r.TestType == row.TestType && r.TestClass == row.TestClass && r.TestMethod == "*");
//                classtotalsrow.AddFromRow(row);
//                totalsrow.AddFromRow(row);
//            }
//        }

//    }
//}
