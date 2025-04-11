using System;
using static DXC.Technology.UnitTesting.Helpers.UnitTestOverviewDataSet;

namespace DXC.Technology.UnitTesting.Helpers
{
    public class UnitTestResult
    {
        public string TestType { get; set; }
        public string TestClass { get; set; }
        public string TestMethod { get; set; }
        public int TotalTestTypeCount { get; set; }
        public int TotalTestClassCount { get; set; }
        public int TotalTestMethodCount { get; set; }
        public int TotalTestMethodPassedCount { get; set; }
        public int TotalTestMethodFailedCount { get; set; }
        public int TotalTestMethodTimeoutCount { get; set; }
        public int TotalAssertsCount { get; set; }
        public int TotalAssertsFailedCount { get; set; }
        public int AreEqualCount { get; set; }
        public int AreEqualFailedCount { get; set; }
        public int AreNotEqualCount { get; set; }
        public int AreNotEqualFailedCount { get; set; }
        public int AreNotSameCount { get; set; }
        public int AreNotSameFailedCount { get; set; }
        public int AreSameCount { get; set; }
        public int AreSameFailedCount { get; set; }
        public int ExceptionAssertCount { get; set; }
        public int FailCount { get; set; }
        public int FailFailedCount { get; set; }
        public int InconclusiveCount { get; set; }
        public int IsFalseCount { get; set; }
        public int IsFalseFailedCount { get; set; }
        public int IsInstanceOfTypeCount { get; set; }
        public int IsInstanceOfTypeFailedCount { get; set; }
        public int IsNotInstanceOfTypeCount { get; set; }
        public int IsNotInstanceOfTypeFailedCount { get; set; }
        public int IsNotNullCount { get; set; }
        public int IsNotNullFailedCount { get; set; }
        public int IsNullCount { get; set; }
        public int IsNullFailedCount { get; set; }
        public int IsTrueCount { get; set; }
        public int IsTrueFailedCount { get; set; }
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;
        public DateTime EndDateTime { get; set; }
        public double DurationInMilliSeconds { get; set; }



        public void LogStart()
        {
            if (this.StartDateTime == DateTime.MinValue)
                this.StartDateTime = DateTime.Now;
            //Else do nothing already initialized....
        }

        public void LogEnd()
        {
            this.EndDateTime = DateTime.Now;
            TimeSpan lts = this.EndDateTime.Subtract(this.StartDateTime);
            this.DurationInMilliSeconds = lts.TotalMilliseconds;
        }

        public void LogEnd(UnitTestOutcome pUnitTestOutcome)
        {
            this.EndDateTime = DateTime.Now;
            TimeSpan lts = this.EndDateTime.Subtract(this.StartDateTime);
            this.DurationInMilliSeconds = lts.TotalMilliseconds;
            switch (pUnitTestOutcome)
            {
                case UnitTestOutcome.Passed:
                    this.TotalTestMethodPassedCount++;
                    break;
                case UnitTestOutcome.Failed:
                    this.TotalTestMethodFailedCount++;
                    break;
                case UnitTestOutcome.Timeout:
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

        public void AddFromRow(UnitTestResult row)
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
}
