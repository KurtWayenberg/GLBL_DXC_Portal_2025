using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.ObjectModel;

//using DXC.Technology.UnitTesting.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.Data.Schema.UnitTesting;

namespace DXC.Technology.UnitTesting.Helpers
{


    public partial class UnitTestOverviewDataSet
    {

        public UnitTestOverviewDataSet.UnitTestResultRow ActiveUnitTestResultRow
        {
            get
            {
                return this.UnitTestResult.GetUnitTestResultRow(ActiveUnitTestType, ActiveTestClass, ActiveTestMethod);
            }
        }
        public UnitTestOverviewDataSet.UnitTestResultRow ActiveUnitTestClassResultRow
        {
            get
            {
                return this.UnitTestResult.GetUnitTestResultRow(ActiveUnitTestType, ActiveTestClass, "*");
            }
        }
        public UnitTestOverviewDataSet.UnitTestResultRow ActiveUnitTestTypeResultRow
        {
            get
            {
                return this.UnitTestResult.GetUnitTestResultRow(ActiveUnitTestType, "*", "*");
            }
        }
        public UnitTestOverviewDataSet.UnitTestResultRow SummaryResultRow
        {
            get
            {
                return this.UnitTestResult.GetUnitTestResultRow("*", "*", "*");
            }
        }
        public UnitTestOverviewDataSet.UnitTestResultRow GetUnitTestResultRow(DXC.Technology.UnitTesting.Enumerations.TechnologyUnitTestTypeEnum pUnitTestType, string pClassName, string pMethodName)
        {
            return this.UnitTestResult.GetUnitTestResultRow(pUnitTestType, pClassName, pMethodName);
        }
        public UnitTestOverviewDataSet.UnitTestResultRow GetUnitTestResultRow(string pUnitTestType, string pClassName, string pMethodName)
        {
            return this.UnitTestResult.GetUnitTestResultRow(pUnitTestType, pClassName, pMethodName);
        }

        public DXC.Technology.UnitTesting.Enumerations.TechnologyUnitTestTypeEnum ActiveUnitTestTypeAsEnum
        {
            get
            {
                return this.ThreadStatistics.CurrentUnitTestTypeAsEnum;
            }
            set
            {
                this.ThreadStatistics.CurrentUnitTestTypeAsEnum = value;
            }
        }

        public void WriteTotalResults()
        {
            string totalsfolder = string.Concat(
                "Totals_",
                DXC.Technology.Utilities.Date.NowYYYYMMString());
            if (System.IO.Directory.Exists(TechnologyUnitTestHelper.UnitTestResultsLocation + totalsfolder))
                System.IO.Directory.Delete(TechnologyUnitTestHelper.UnitTestResultsLocation + totalsfolder, true);
            System.IO.Directory.CreateDirectory(TechnologyUnitTestHelper.UnitTestResultsLocation + totalsfolder);


            foreach (string testtype in this.UnitTestResult.Select(p => p.TestType).Distinct().Where(p => !p.Equals("*")))
            {
                UnitTestResultDataTable unittestresultextract = new UnitTestResultDataTable();
                double counter = 0;
                double success = 0;
                foreach (var row in this.UnitTestResult.Where(p => p.TestType == testtype))
                {
                    counter += row.TotalTestMethodCount;
                    success += row.TotalTestMethodPassedCount;
                    unittestresultextract.ImportRow(row);
                }
                string successrate = System.Math.Round((success / counter) * 100, 0).ToString().PadLeft(3, '0');
                string status = ((success - counter) < 1) ? "Passed" : "Failed";

                System.IO.StringWriter lsw = new System.IO.StringWriter();
                WriteUnitTests(unittestresultextract, lsw);

                string lFileName = string.Concat(TechnologyUnitTestHelper.UnitTestResultsLocation + totalsfolder + "\\", testtype, "_", successrate, "Perc_Result_", status, ".txt");
                DXC.Technology.Publishers.FilePublisher lfp = new DXC.Technology.Publishers.FilePublisher(lFileName);
                lfp.Publish(lsw.ToString());
            }
        }

        public void WriteResults()
        {
            var summary = SummaryResultRow;
            double overallsuccessrate = (summary.TotalTestMethodPassedCount * 100) / summary.TotalTestMethodCount;
            string summaryfolder = string.Concat(
                "Run_",
                DXC.Technology.Utilities.Date.ToYYYYMMDDHHMMSSFFFString(DateTime.Now),
                "_", System.Math.Round(overallsuccessrate, 0).ToString().PadLeft(3, '0'),
                "PercOk_Status_",
                summary.OverallStatus);
            System.IO.Directory.CreateDirectory(TechnologyUnitTestHelper.UnitTestResultsLocation + summaryfolder);

            foreach (string testtype in this.UnitTestResult.Select(p => p.TestType).Distinct().Where(p => !p.Equals("*")))
            {
                UnitTestResultDataTable unittestresultextract = new UnitTestResultDataTable();
                double counter = 0;
                double success = 0;
                foreach (var row in this.UnitTestResult.Where(p => p.TestType == testtype))
                {
                    counter += row.TotalTestMethodCount;
                    success += row.TotalTestMethodPassedCount;
                    unittestresultextract.ImportRow(row);
                }
                string successrate = System.Math.Round((success / counter) * 100, 0).ToString().PadLeft(3, '0');
                string status = ((success - counter) < 1) ? "Passed" : "Failed";

                System.IO.StringWriter lsw = new System.IO.StringWriter();
                WriteUnitTests(unittestresultextract, lsw);

                string lFileName = string.Concat(TechnologyUnitTestHelper.UnitTestResultsLocation + summaryfolder + "\\", testtype, "_", successrate, "Perc_Result_", status, ".txt");
                DXC.Technology.Publishers.FilePublisher lfp = new DXC.Technology.Publishers.FilePublisher(lFileName);
                lfp.Publish(lsw.ToString());
            }
        }

        private void WriteUnitTests(UnitTestResultDataTable unittestresultextract, System.IO.StringWriter lsw)
        {
            UnitTestResultRow.WriteSummaryTitle(lsw, 0, false);
            foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='*'")))
                lutrRow.WriteSummaryResults(lsw, 0, false);
            lsw.WriteLine();

            UnitTestResultRow.WriteTestTypeTitle(lsw, 0, false);
            foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestTypeColumn.ColumnName))
            {
                lutrRow.WriteTestTypeResults(lsw, 0, false);
            }
            lsw.WriteLine();

            UnitTestResultRow.WriteClassTitle(lsw, 0, false);
            foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestTypeColumn.ColumnName))
            {
                foreach (UnitTestResultRow lutrcRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestMethodColumn.ColumnName, "='*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
                {
                    lutrcRow.WriteClassResults(lsw, 0, false);
                }
            }
            lsw.WriteLine();


            UnitTestResultRow.WriteMethodTitle(lsw, 0, false);
            foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestClassColumn.ColumnName))
            {
                foreach (UnitTestResultRow lutrcRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestMethodColumn.ColumnName, "='*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
                {
                    foreach (UnitTestResultRow lutrmRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "='", lutrcRow.TestClass, "' AND ", unittestresultextract.TestMethodColumn.ColumnName, "<>'*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName, ",", unittestresultextract.TestMethodColumn.ColumnName)))
                    {
                        lutrmRow.WriteMethodResults(lsw, 0, false);
                    }
                }
            }
            lsw.WriteLine();


            UnitTestResultRow.WriteMethodTitle(lsw, 0, false);
            foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestClassColumn.ColumnName))
            {
                foreach (UnitTestResultRow lutrcRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestMethodColumn.ColumnName, "='*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
                {
                    foreach (UnitTestResultRow lutrmRow in unittestresultextract.Select(
                                string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ",
                                              unittestresultextract.TestClassColumn.ColumnName, "='", lutrcRow.TestClass, "' AND ",
                                              unittestresultextract.TestMethodColumn.ColumnName, "<> '*' AND (",
                                              unittestresultextract.TotalTestMethodFailedCountColumn.ColumnName, ">0 OR ",
                                              unittestresultextract.TotalTestMethodTimeoutCountColumn.ColumnName, ">0)"),
                                 string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
                    {
                        lutrmRow.WriteMethodResults(lsw, 0, true);
                        foreach (UnitTestDetailRow lutdRow in this.UnitTestDetail.Select(string.Concat(this.UnitTestDetail.TestTypeColumn.ColumnName, "='", lutrmRow.TestType, "' AND ", this.UnitTestDetail.TestClassColumn.ColumnName, "='", lutrcRow.TestClass, "' AND ", this.UnitTestDetail.TestMethodColumn.ColumnName, "='", lutrmRow.TestMethod, "'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName, ",", unittestresultextract.TestMethodColumn.ColumnName)))
                        {
                            lutdRow.WriteDetailResults(lsw, 5, false);
                        }
                    }
                }
            }
            lsw.WriteLine();

            UnitTestResultRow.WriteMethodTitle(lsw, 0, false);
            foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestClassColumn.ColumnName))
            {
                foreach (UnitTestResultRow lutrcRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestMethodColumn.ColumnName, "='*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
                {
                    foreach (UnitTestResultRow lutrmRow in unittestresultextract.Select(
                                string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ",
                                              unittestresultextract.TestClassColumn.ColumnName, "='", lutrcRow.TestClass, "'"),
                                 string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
                    {
                        lutrmRow.WriteMethodResults(lsw, 0, true);
                        foreach (UnitTestDetailRow lutdRow in this.UnitTestDetail.Select(string.Concat(this.UnitTestDetail.TestTypeColumn.ColumnName, "='", lutrcRow.TestType, "' AND ", this.UnitTestDetail.TestClassColumn.ColumnName, "='", lutrcRow.TestClass, "' AND ", this.UnitTestDetail.TestMethodColumn.ColumnName, "='", lutrmRow.TestMethod, "'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName, ",", unittestresultextract.TestMethodColumn.ColumnName)))
                        {
                            lutdRow.WriteDetailResults(lsw, 5, false);
                        }

                    }
                }
            }
            lsw.WriteLine();
        }

        internal void UpdateTotals(UnitTestOverviewDataSet dsTotals)
        {
            dsTotals.Merge(this.UnitTestResult);
            dsTotals.RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            var totalsrow = UnitTestResult.FindByTestTypeTestClassTestMethod("*", "*", "*");
            foreach (var row in UnitTestResult.ToList().Where(p => p.TestType != "*" && p.TestClass != "*" && p.TestMethod == "*"))
            {
                row.InitializeWithZeroes();
            }

            foreach (var row in UnitTestResult.ToList().Where(p => p.TestType != "*" && p.TestClass != "*" && p.TestMethod != "*"))
            {
                var classtotalsrow = UnitTestResult.FindByTestTypeTestClassTestMethod(row.TestType, row.TestClass, "*");
                classtotalsrow.AddFromRow(row);
                totalsrow.AddFromRow(row);
            }
        }


        //public void WriteResultsFull(UnitTestResultDataTable unittestresultextract)
        //{
        //    System.IO.StringWriter lsw = new System.IO.StringWriter();
        //    UnitTestResultRow.WriteSummaryTitle(lsw, 0, false);
        //    foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='*'")))
        //        lutrRow.WriteSummaryResults(lsw, 0, false);
        //    lsw.WriteLine();

        //    UnitTestResultRow.WriteTestTypeTitle(lsw, 0, false);
        //    foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestTypeColumn.ColumnName))
        //    {
        //        lutrRow.WriteTestTypeResults(lsw, 0, false);
        //    }
        //    lsw.WriteLine();
        //    lsw.WriteLine();

        //    foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestTypeColumn.ColumnName))
        //    {
        //        //UnitTestResultRow.WriteTestTypeTitle(lsw,0,true);
        //        lutrRow.WriteTestTypeResults(lsw, 0, true);
        //        lsw.WriteLine();
        //        UnitTestResultRow.WriteClassTitle(lsw, 3, false);
        //        foreach (UnitTestResultRow lutrcRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestMethodColumn.ColumnName, "='*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
        //        {
        //            lutrcRow.WriteClassResults(lsw, 3, false);
        //        }
        //        lsw.WriteLine();
        //    }
        //    lsw.WriteLine();


        //    foreach (UnitTestResultRow lutrRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestClassColumn.ColumnName, "='*'"), unittestresultextract.TestClassColumn.ColumnName))
        //    {
        //        //UnitTestResultRow.WriteTestTypeTitle(lsw,0, true);
        //        lutrRow.WriteTestTypeResults(lsw, 0, true);
        //        lsw.WriteLine();
        //        foreach (UnitTestResultRow lutrcRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "<>'*' AND ", unittestresultextract.TestMethodColumn.ColumnName, "='*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName)))
        //        {
        //            //UnitTestResultRow.WriteClassTitle(lsw,3, true);
        //            lutrcRow.WriteClassResults(lsw, 3, true);

        //            lsw.WriteLine();
        //            UnitTestResultRow.WriteMethodTitle(lsw, 6, false);
        //            foreach (UnitTestResultRow lutrmRow in unittestresultextract.Select(string.Concat(unittestresultextract.TestTypeColumn.ColumnName, "='", lutrRow.TestType, "' AND ", unittestresultextract.TestClassColumn.ColumnName, "='", lutrcRow.TestClass, "' AND ", unittestresultextract.TestMethodColumn.ColumnName, "<>'*'"), string.Concat(unittestresultextract.TestTypeColumn.ColumnName, ",", unittestresultextract.TestClassColumn.ColumnName, ",", unittestresultextract.TestMethodColumn.ColumnName)))
        //            {
        //                lutrmRow.WriteMethodResults(lsw, 6, false);
        //            }

        //            lsw.WriteLine();
        //        }
        //        lsw.WriteLine();
        //    }
        //    lsw.WriteLine();

        //    System.Diagnostics.Debug.Write(lsw.ToString());
        //}


        public string ActiveUnitTestType
        {
            get
            {
                return this.ThreadStatistics.CurrentUnitTestType;
            }
            set
            {
                this.ThreadStatistics.CurrentUnitTestType = value;
            }
        }

        public string ActiveTestClass
        {
            get
            {
                return this.ThreadStatistics.CurrentTestClass;
            }
            set
            {
                this.ThreadStatistics.CurrentTestClass = value;
            }
        }

        public void RegisterTestStart(string pMethodName)
        {
            ActiveTestMethod = pMethodName;
        }

        public void RegisterTestEnd(string pMethodName)
        {
            ActiveTestMethod = pMethodName;
        }


        public string ActiveTestComment
        {
            get
            {
                return this.ThreadStatistics.CurrentTestComment;
            }
            set
            {
                this.ThreadStatistics.CurrentTestComment = value;
            }
        }

        public string ActiveTestMethod
        {
            get
            {
                return this.ThreadStatistics.CurrentTestMethod;
            }
            set
            {
                this.ThreadStatistics.CurrentTestMethod = value;
            }
        }

        partial class ThreadStatisticsRow
        {
            public DXC.Technology.UnitTesting.Enumerations.TechnologyUnitTestTypeEnum CurrentTestTypeAsEnum
            {
                get
                {
                    return DXC.Technology.Enumerations.EnumerationHelper.CodeToEnum<DXC.Technology.UnitTesting.Enumerations.TechnologyUnitTestTypeEnum>(this.CurrentTestType);
                }
                set
                {
                    this.CurrentTestType = DXC.Technology.Enumerations.EnumerationHelper.EnumToCode(value);
                }
            }

        }

        partial class ThreadStatisticsDataTable
        {
            public ThreadStatisticsRow ActiveThreadRow
            {
                get
                {
                    string lThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                    if (string.IsNullOrEmpty(lThreadID)) lThreadID = "N/A";
                    lock (lThreadID)
                    {
                        if (this.FindByThreadID(lThreadID) == null)
                        {
                            lock (this)
                            {
                                if (this.FindByThreadID(lThreadID) == null)
                                {
                                    ThreadStatisticsRow lRow = this.NewThreadStatisticsRow();
                                    lRow.ThreadID = lThreadID;
                                    lRow.CurrentTestClass = "";
                                    lRow.CurrentTestMethod = "";
                                    lRow.CurrentTestType = "";
                                    this.AddThreadStatisticsRow(lRow);
                                }
                            }
                        }
                    }
                    return this.FindByThreadID(lThreadID);
                }
            }

            public DXC.Technology.UnitTesting.Enumerations.TechnologyUnitTestTypeEnum CurrentUnitTestTypeAsEnum
            {
                get
                {
                    return this.ActiveThreadRow.CurrentTestTypeAsEnum;
                }
                set
                {
                    this.ActiveThreadRow.CurrentTestTypeAsEnum = value;
                }
            }

            public string CurrentUnitTestType
            {
                get
                {
                    return this.ActiveThreadRow.CurrentTestType;
                }
                set
                {
                    this.ActiveThreadRow.CurrentTestType = value;
                }
            }


            public string CurrentTestClass
            {
                get
                {
                    return this.ActiveThreadRow.CurrentTestClass;
                }
                set
                {
                    this.ActiveThreadRow.CurrentTestClass = value;
                }
            }

            public string CurrentTestMethod
            {
                get
                {
                    return this.ActiveThreadRow.CurrentTestMethod;
                }
                set
                {
                    this.ActiveThreadRow.CurrentTestMethod = value;
                }
            }

            public string CurrentTestComment
            {
                get
                {
                    return this.ActiveThreadRow.CurrentTestComment;
                }
                set
                {
                    this.ActiveThreadRow.CurrentTestComment = value;
                }
            }

            public void LogAssertCall()
            {
                this.ActiveThreadRow.TotalAssertsCount++;
            }

            public void LogAssertCallFailure()
            {
                this.ActiveThreadRow.TotalAssertsFailedCount++;
            }

        }

        partial class UnitTestResultDataTable
        {
            public UnitTestOverviewDataSet.UnitTestResultRow GetUnitTestResultRow(DXC.Technology.UnitTesting.Enumerations.TechnologyUnitTestTypeEnum pUnitTestType, string pClassName, string pMethodName)
            {
                return GetUnitTestResultRow(pUnitTestType.ToString(), pClassName, pMethodName);
            }

            public UnitTestOverviewDataSet.UnitTestResultRow GetUnitTestResultRow(string pUnitTestType, string pClassName, string pMethodName)
            {
                if (string.IsNullOrEmpty(pUnitTestType))
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Unit Test Type");
                if (string.IsNullOrEmpty(pClassName))
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Unit Test Class");
                if (string.IsNullOrEmpty(pMethodName))
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Unit Test Method");
                if (this.FindByTestTypeTestClassTestMethod(pUnitTestType.ToString(), pClassName, pMethodName) == null)
                {
                    string key = $"{pUnitTestType}|{pClassName}|{pMethodName}|";
                    lock (key)
                    {
                        if (this.FindByTestTypeTestClassTestMethod(pUnitTestType.ToString(), pClassName, pMethodName) == null)
                        {
                            lock (this)
                            {
                                UnitTestOverviewDataSet.UnitTestResultRow lutrRow = null;
                                if (this.FindByTestTypeTestClassTestMethod(pUnitTestType.ToString(), pClassName, pMethodName) == null)
                                {
                                    lutrRow = this.NewUnitTestResultRow();
                                    lutrRow.TestType = pUnitTestType;
                                    lutrRow.TestClass = pClassName;
                                    lutrRow.TestMethod = pMethodName;

                                    this.AddUnitTestResultRow(lutrRow);

                                }
                                lutrRow = this.FindByTestTypeTestClassTestMethod(pUnitTestType.ToString(), pClassName, pMethodName);
                                    if ((pUnitTestType != "*") && (pClassName != "*") && (pMethodName != "*"))
                                {
                                    //You are creating for the first time a method...
                                    //GetUnitTestResultRow("*", "*", "*").TotalTestMethodCount++;
                                    //GetUnitTestResultRow(pUnitTestType, "*", "*").TotalTestMethodCount++;
                                    //GetUnitTestResultRow(pUnitTestType, pClassName, "*").TotalTestMethodCount++;
                                    lutrRow.TotalTestTypeCount = 1;
                                    lutrRow.TotalTestClassCount = 1;
                                    lutrRow.TotalTestMethodCount = 1;
                                }
                                else
                                {
                                    //you are creating one of the top rows - be carefull
                                    if (pUnitTestType == "*")
                                    {
                                        // All new...
                                        //Do nothing
                                    }
                                    else
                                    {
                                        if (pClassName == "*")
                                        {
                                            //New testtype - do nothing
                                            lutrRow.TotalTestTypeCount++;
                                            GetUnitTestResultRow("*", "*", "*").TotalTestTypeCount++;
                                        }
                                        else
                                        {
                                            //you are creating the first time a class
                                            lutrRow.TotalTestClassCount++;
                                            GetUnitTestResultRow("*", "*", "*").TotalTestClassCount++;
                                            GetUnitTestResultRow(pUnitTestType, "*", "*").TotalTestClassCount++;
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
                return this.FindByTestTypeTestClassTestMethod(pUnitTestType.ToString(), pClassName, pMethodName);
            }
        }
        partial class UnitTestDetailRow
        {
            public void WriteDetailResults(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(this.Comment);
                psw.Write(" (");
                psw.Write(this.AssertType);
                psw.WriteLine(") ");
            }
        }

        partial class UnitTestResultRow
        {

            public void LogStart()
            {
                if (this.IsStartDateTimeNull())
                    this.StartDateTime = DateTime.Now;
                //Else do nothing already initialized....
            }

            public void LogEnd()
            {
                this.EndDateTime = DateTime.Now;
                TimeSpan lts = this.EndDateTime.Subtract(this.StartDateTime);
                this.DurationInMilliSeconds = lts.TotalMilliseconds;
            }

            internal void LogEnd(Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestOutcome pUnitTestOutcome)
            {
                if (this.IsStartDateTimeNull())
                    this.StartDateTime = DateTime.Now;
                this.EndDateTime = DateTime.Now;
                TimeSpan lts = this.EndDateTime.Subtract(this.StartDateTime);
                this.DurationInMilliSeconds = lts.TotalMilliseconds;
                switch (pUnitTestOutcome)
                {
                    case  Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestOutcome.Passed:
                        this.TotalTestMethodPassedCount++;
                        break;
                    case Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestOutcome.Failed:
                        this.TotalTestMethodFailedCount++;
                        break;
                    case Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestOutcome.Timeout:
                        this.TotalTestMethodTimeoutCount++;
                        break;
                }
            }

            public void InitializeWithZeroes()
            {
                this.DurationInMilliSeconds = 0;
                this.TotalTestMethodPassedCount = 0;
                this.TotalTestMethodFailedCount = 0;
                this.TotalTestMethodTimeoutCount = 0;

                this.TotalAssertsCount = 0;
                this.AreEqualCount = 0;
                this.AreNotEqualCount = 0;
                this.AreNotSameCount = 0;
                this.AreSameCount = 0;
                this.FailCount = 0;
                this.InconclusiveCount = 0;
                this.IsFalseCount = 0;
                this.IsInstanceOfTypeCount = 0;
                this.IsNotInstanceOfTypeCount = 0;
                this.IsNotNullCount = 0;
                this.IsNullCount = 0;
                this.IsTrueCount = 0;
                this.ExceptionAssertCount = 0;

            }

            public void AddFromRow(UnitTestResultRow row)
            {
                this.TotalAssertsCount += row.TotalAssertsCount;
                this.AreEqualCount += row.AreEqualCount;
                this.AreNotEqualCount += row.AreNotEqualCount;
                this.AreNotSameCount += row.AreNotSameCount;
                this.AreSameCount += row.AreSameCount;
                this.FailCount += row.FailCount;
                this.InconclusiveCount += row.InconclusiveCount;
                this.IsFalseCount += row.IsFalseCount;
                this.IsInstanceOfTypeCount += row.IsInstanceOfTypeCount;
                this.IsNotInstanceOfTypeCount += row.IsNotInstanceOfTypeCount;
                this.IsNotNullCount += row.IsNotNullCount;
                this.IsNullCount += row.IsNullCount;
                this.IsTrueCount += row.IsTrueCount;
                this.ExceptionAssertCount += row.ExceptionAssertCount;

                this.DurationInMilliSeconds += row.DurationInMilliSeconds;
                this.TotalTestMethodPassedCount += row.TotalTestMethodPassedCount;
                this.TotalTestMethodFailedCount += row.TotalTestMethodFailedCount;
                this.TotalTestMethodTimeoutCount += row.TotalTestMethodTimeoutCount;

            }

            public void LogAssertCall(AssertCallTypeEnum pAssertCallType)
            {
                this.TotalAssertsCount++;
                switch (pAssertCallType)
                {
                    case AssertCallTypeEnum.AreEqual:
                        this.AreEqualCount++;
                        break;
                    case AssertCallTypeEnum.AreNotEqual:
                        this.AreNotEqualCount++;
                        break;
                    case AssertCallTypeEnum.AreNotSame:
                        this.AreNotSameCount++;
                        break;
                    case AssertCallTypeEnum.AreSame:
                        this.AreSameCount++;
                        break;
                    case AssertCallTypeEnum.Fail:
                        this.FailCount++;
                        break;
                    case AssertCallTypeEnum.Inconclusive:
                        this.InconclusiveCount++;
                        break;
                    case AssertCallTypeEnum.IsFalse:
                        this.IsFalseCount++;
                        break;
                    case AssertCallTypeEnum.IsInstanceOfType:
                        this.IsInstanceOfTypeCount++;
                        break;
                    case AssertCallTypeEnum.IsNotInstanceOfType:
                        this.IsNotInstanceOfTypeCount++;
                        break;
                    case AssertCallTypeEnum.IsNotNull:
                        this.IsNotNullCount++;
                        break;
                    case AssertCallTypeEnum.IsNull:
                        this.IsNullCount++;
                        break;
                    case AssertCallTypeEnum.IsTrue:
                        this.IsTrueCount++;
                        break;
                    case AssertCallTypeEnum.ExceptionAssert:
                        this.ExceptionAssertCount++;
                        break;
                }
            }


            public void LogAssertCallFailure(AssertCallTypeEnum pAssertCallType)
            {
                this.TotalAssertsFailedCount++;
                switch (pAssertCallType)
                {
                    case AssertCallTypeEnum.AreEqual:
                        this.AreEqualFailedCount++;
                        break;
                    case AssertCallTypeEnum.AreNotEqual:
                        this.AreNotEqualFailedCount++;
                        break;
                    case AssertCallTypeEnum.AreNotSame:
                        this.AreNotSameFailedCount++;
                        break;
                    case AssertCallTypeEnum.AreSame:
                        this.AreSameFailedCount++;
                        break;
                    case AssertCallTypeEnum.Fail:
                        this.FailFailedCount++;
                        break;
                    case AssertCallTypeEnum.IsFalse:
                        this.IsFalseFailedCount++;
                        break;
                    case AssertCallTypeEnum.IsInstanceOfType:
                        this.IsInstanceOfTypeFailedCount++;
                        break;
                    case AssertCallTypeEnum.IsNotInstanceOfType:
                        this.IsNotInstanceOfTypeFailedCount++;
                        break;
                    case AssertCallTypeEnum.IsNotNull:
                        this.IsNotNullFailedCount++;
                        break;
                    case AssertCallTypeEnum.IsNull:
                        this.IsNullFailedCount++;
                        break;
                    case AssertCallTypeEnum.IsTrue:
                        this.IsTrueFailedCount++;
                        break;
                    case AssertCallTypeEnum.ExceptionAssert:
                        this.IsTrueFailedCount++;
                        break;
                }
            }

            public string OverallStatus
            {
                get
                {
                    if (this.TotalTestMethodFailedCount > 0) return "Failed";
                    if (this.TotalTestMethodTimeoutCount > 0) return "Failed";
                    if (this.TotalTestMethodPassedCount > 0) return "Passed";
                    return "Unknown";
                }
            }


            public static void WriteSummaryTitle(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString("Overall St.", 12));
                if (!pSkipDetail)
                {
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Type", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Cls", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Mth", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Pass", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Fail", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Tmot", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Pas.", 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Fail", 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Dur.sec.", 8));
                }
                psw.WriteLine();
            }

            public void WriteSummaryResults(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.OverallStatus, 12));
                if (!pSkipDetail)
                {
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestTypeCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestClassCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodPassedCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodFailedCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodTimeoutCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsCount, 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsFailedCount, 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString((this.DurationInMilliSeconds / 1000).ToString("0.00", DXC.Technology.Utilities.StringFormatProvider.Default),
                                                                                8, DXC.Technology.Utilities.String.FormatStringAlignmentEnum.Right, " "));
                }
                psw.WriteLine();
            }


            public static void WriteTestTypeTitle(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString("Type", 10));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString("Status", 10));
                if (!pSkipDetail)
                {
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Cls", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Mth", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Pass", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Fail", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Tmot", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Pas.", 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Fail", 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Dur.sec.", 8));
                }
                psw.WriteLine();
            }

            public void WriteTestTypeResults(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.TestType, 10));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.OverallStatus, 10));
                if (!pSkipDetail)
                {
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestClassCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodPassedCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodFailedCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodTimeoutCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsCount, 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsFailedCount, 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString((this.DurationInMilliSeconds / 1000).ToString("0.00", DXC.Technology.Utilities.StringFormatProvider.Default),
                            8, DXC.Technology.Utilities.String.FormatStringAlignmentEnum.Right, " "));
                }
                psw.WriteLine();
            }

            public static void WriteClassTitle(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString("Type", 10));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString("Class", 20));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString("Status", 10));
                psw.Write(" ");
                if (!pSkipDetail)
                {
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Mth", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Pass", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Fail", 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("Tmot", 4));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Pas.", 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Fail", 8));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString("Dur.Sec.", 8));
                }
                psw.WriteLine();
            }

            public void WriteClassResults(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.TestType, 10));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.TestClass, 20));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.OverallStatus, 10));
                psw.Write(" ");
                if (!pSkipDetail)
                {
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodPassedCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodFailedCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalTestMethodTimeoutCount, 4));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsCount, 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsFailedCount, 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString((this.DurationInMilliSeconds / 1000).ToString("0.00", DXC.Technology.Utilities.StringFormatProvider.Default),
                                        8, DXC.Technology.Utilities.String.FormatStringAlignmentEnum.Right, " "));
                }
                psw.WriteLine();

            }

            public static void WriteMethodTitle(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString("Type", 10));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString("Class", 20));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString("Method", 30));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString("Status", 10));
                psw.Write(" ");
                if (!pSkipDetail)
                {
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Pas.", 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString("#Ass.Fail", 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(" T  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString(" F  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString(" E  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString("NE  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString(" S  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString("NS  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString(" I  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString("NI  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString(" N  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString("NN  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString("FL  -", 6));
                    psw.Write(DXC.Technology.Utilities.String.FormatString("?? ", 3));

                    psw.Write(DXC.Technology.Utilities.String.FormatString("Dur.Sec.", 8));
                }
                psw.WriteLine();
            }

            public void WriteMethodResults(System.IO.StringWriter psw, int pIndent, bool pSkipDetail)
            {
                psw.Write(DXC.Technology.Utilities.String.FormatString("", pIndent));
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.TestType, 10));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.TestClass, 20));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.TestMethod, 30));
                psw.Write(" ");
                psw.Write(DXC.Technology.Utilities.String.FormatString(this.OverallStatus, 10));
                psw.Write(" ");
                if (!pSkipDetail)
                {
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsCount, 8));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.TotalAssertsFailedCount, 8));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsTrueCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsTrueFailedCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsFalseCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsFalseFailedCount, 2));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreEqualCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreEqualFailedCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreNotEqualCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreNotEqualFailedCount, 2));

                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreSameCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreSameFailedCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreNotSameCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.AreNotSameFailedCount, 2));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsInstanceOfTypeCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsInstanceOfTypeFailedCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsNotInstanceOfTypeCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsNotInstanceOfTypeFailedCount, 2));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsNullCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsNullFailedCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsNotNullCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.IsNotNullFailedCount, 2));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.FailCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.FailFailedCount, 2));
                    psw.Write(" ");
                    psw.Write(DXC.Technology.Utilities.String.FormatString(this.InconclusiveCount, 2));
                    psw.Write(" ");

                    psw.Write(DXC.Technology.Utilities.String.FormatString((this.DurationInMilliSeconds / 1000).ToString("0.00", DXC.Technology.Utilities.StringFormatProvider.Default),
                                                        8, DXC.Technology.Utilities.String.FormatStringAlignmentEnum.Right, " "));
                }
                psw.WriteLine();

            }

        }

        public void LogException(System.Exception pEx, AssertCallTypeEnum pAssertCallType)
        {
            //            string lThreadID = System.Threading.Thread.CurrentThread.Name;
            //           UnitTestDetail.AddUnitTestDetailRow(lThreadID, ActiveUnitTestType, ActiveTestClass, ActiveTestMethod, pAssertCallType.ToString(), pEx.Message);
        }

        internal void LogAssertCallFailure(AssertCallTypeEnum pAssertCallType, string pComment)
        {
            string lThreadID = System.Threading.Thread.CurrentThread.Name;
            UnitTestDetail.AddUnitTestDetailRow(lThreadID, ActiveUnitTestType, ActiveTestClass, ActiveTestMethod, pAssertCallType.ToString(), false, pComment);
        }

        internal void LogAssertCall(AssertCallTypeEnum pAssertCallType, string pComment)
        {
            string lThreadID = System.Threading.Thread.CurrentThread.Name;
            UnitTestDetail.AddUnitTestDetailRow(lThreadID, ActiveUnitTestType, ActiveTestClass, ActiveTestMethod, pAssertCallType.ToString(), true, pComment);
        }
    }
}
