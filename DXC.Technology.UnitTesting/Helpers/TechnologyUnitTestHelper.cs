//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DXC.Technology.UnitTesting.Enumerations;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace DXC.Technology.UnitTesting.Helpers
//{
//    public class TechnologyUnitTestHelper
//    {

//        #region Public Static Properties

//        /// <summary>
//        /// Location of unit test results.
//        /// </summary>
//        public static string UnitTestResultsLocation { get; } = DXC.Technology.Configuration.AppSettingsHelper.GetAsString("TechnologySettings:UnitTestingRoot");

//        /// <summary>
//        /// Location of unit test files.
//        /// </summary>
//        public static string UnitTestFilesLocation { get; } = DXC.Technology.Configuration.AppSettingsHelper.GetAsString("TechnologySettings:UnitTestingRoot") + @"Files\";

//        #endregion

//        #region Public Static Methods

//        /// <summary>
//        /// Logs an assert call.
//        /// </summary>
//        /// <param name="assertCallType">The type of assert call.</param>
//        /// <param name="comment">Additional comments for the assert call.</param>
//        public static void LogAssertCall(AssertCallTypeEnum assertCallType, string comment)
//        {
//            unitTestOverview.ActiveUnitTestResultRow.LogAssertCall(assertCallType);
//            //unitTestOverview.ActiveUnitTestClassResultRow.LogAssertCall(assertCallType);
//            //unitTestOverview.ActiveUnitTestTypeResultRow.LogAssertCall(assertCallType);
//            //unitTestOverview.SummaryResultRow.LogAssertCall(assertCallType);
//            //unitTestOverview.ThreadStatistics.LogAssertCall();
//            //unitTestOverview.LogAssertCall(assertCallType, comment);
//        }

//        /// <summary>
//        /// Logs an assert call failure.
//        /// </summary>
//        /// <param name="ex">The exception that occurred.</param>
//        /// <param name="assertCallType">The type of assert call.</param>
//        /// <param name="comment">Additional comments for the assert call failure.</param>
//        public static void LogAssertCallFailure(Exception ex, AssertCallTypeEnum assertCallType, string comment)
//        {
//            DXC.Technology.Exceptions.ExceptionHelper.Publish(ex);

//            unitTestOverview.ActiveUnitTestResultRow.LogAssertCallFailure(assertCallType);
//            unitTestOverview.ActiveUnitTestClassResultRow.LogAssertCallFailure(assertCallType);
//            unitTestOverview.ActiveUnitTestTypeResultRow.LogAssertCallFailure(assertCallType);
//            unitTestOverview.SummaryResultRow.LogAssertCallFailure(assertCallType);
//            unitTestOverview.ThreadStatistics.LogAssertCallFailure();
//            unitTestOverview.LogAssertCallFailure(assertCallType, comment);
//            unitTestOverview.LogException(ex, assertCallType);
//        }

//        /// <summary>
//        /// Performs assembly cleanup.
//        /// </summary>
//        public static void HelpAssemblyCleanup()
//        {
//            try
//            {
//                unitTestOverview.SummaryResultRow.LogEnd();
//                unitTestOverview.WriteResults();
//                UnitTestOverviewDataSet dsTotals = new UnitTestOverviewDataSet();
//                string totalsXmlFile = UnitTestResultsLocation + "Totals.xml";
//                if (System.IO.File.Exists(totalsXmlFile))
//                    dsTotals.ReadXml(totalsXmlFile);
//                unitTestOverview.UpdateTotals(dsTotals);
//                dsTotals.WriteTotalResults();
//                if (System.IO.File.Exists(totalsXmlFile))
//                    System.IO.File.Delete(totalsXmlFile);
//                dsTotals.WriteXml(totalsXmlFile);
//            }
//            catch (Exception ex)
//            {
//                DXC.Technology.Exceptions.ExceptionHelper.Publish(ex);
//            }
//        }

//        /// <summary>
//        /// Performs assembly initialization.
//        /// </summary>
//        /// <param name="uiAssemblyRequest">Indicates whether the initialization is requested by the UI assembly.</param>
//        public static void HelpAssemblyInitialization(bool uiAssemblyRequest)
//        {
//            if ((uiAssemblyRequest) && (unitTestOverview != null)) return;

//            unitTestOverview = new UnitTestOverviewDataSet();

//            //// Activate the specified tracing strategy
//            //DXC.Technology.Configuration.TracingManagementSectionHandler.Current.ActivateTracing();

//            //LogTestEvent("Assembly Start - Demo App Unit Tests", TechnologyUnitTestTypeEnum.Unspecified, "", "");

//            //unitTestOverview.SummaryResultRow.LogStart();

//            //DXC.Technology.UseCase.UseCaseBase.UseCaseDurationLogger = new DXC.Technology.Business.TechnologyUseCaseDurationLogger();

//            //DXC.Technology.Common.TechnologyActivityHelper.ActivityUserCodeHelper = new TechnologyActivityUserCodeHelper();
//            //DXC.Technology.Common.TechnologyActivityHelper.ActivitySecurityHelper = new TechnologyActivitySecurityHelper();
//            //DXC.Technology.Common.TechnologyActivityHelper.ActivityDecisionTableHelper = new TechnologyActivityDecisionTableHelper();

//            //// Set Validation exception info
//            //DXC.Technology.Business.TechnologyUserCodeManager.RefreshUserCodes();

//            //DXC.Technology.Exceptions.NamedExceptions.ValidationException.ValidationMessageTemplateDictionary =
//            //    DXC.Technology.Business.TechnologyUserCodeManager.UserCodeDomain.GetUserCodeGroupAsStringDictionary(
//            //        DXC.Technology.Enumerations.EnumerationHelper.EnumToCode(DXC.Technology.Common.Enumerations.TechnologyUserCodeEnum.ASGV_AttributeSetGroupSet));

//            //DXC.Technology.Common.TechnologyParameterHelper.TechnologyParameterResolver = new TechnologyParameterResolver();
//            //DXC.Technology.Exceptions.ExceptionHelper.TranslationManager = DXC.Technology.Translations.TranslationManager.Current;

//            //DXC.Technology.Common.ServiceModel.Dynamics.DynamicInformationAttribute.UserCodes = DXC.Technology.Business.TechnologyUserCodeManager.UserCodeDomain;
//            //DXC.Technology.Common.ServiceModel.Xmls.XmlDocumentHelper.UserCodes = DXC.Technology.Business.TechnologyUserCodeManager.UserCodeDomain;

//            //bool reloadDatabaseOnlyAsTestMethod = true;
//            //if (!reloadDatabaseOnlyAsTestMethod)
//            //{
//            //    TechnologyLoadScriptHelper.ReloadBaseDB();
//            //    if (DXC.Technology.Configuration.AppSettingsHelper.GetAsBoolean("runIISRESET"))
//            //        TechnologyLoadScriptHelper.ResetIIS();
//            //}
//            //if (!uiAssemblyRequest)
//            //{
//            //    DXC.Technology.Caching.TechnologyBasicContextInfo.SecurityManagerFactory = new DXC.Technology.Business.TechnologySecurityManagerFactory(DXC.Technology.Business.TechnologySecurityManager.PermissionDomain);
//            //    DXC.Technology.Caching.TechnologyServiceContextInfo.TransactionProvider = new DXC.Technology.Business.TechnologyContextFactory();
//            //    DXC.Technology.Caching.TechnologyServiceContextInfo.SetContextProvider(new DXC.Technology.Business.TechnologyContextFactory());

//            //    DXC.Technology.Security.MembershipProvider.SecurityProvider = new DXC.Technology.Business.TechnologySecurityProvider();

//            //    // Set Validation exception info
//            //    DXC.Technology.Business.TechnologyUserCodeManager.RefreshUserCodes();
//            //    DXC.Technology.Exceptions.NamedExceptions.ValidationException.ValidationMessageTemplateDictionary =
//            //        DXC.Technology.Business.TechnologyUserCodeManager.UserCodeDomain.GetUserCodeGroupAsStringDictionary(
//            //            DXC.Technology.Enumerations.EnumerationHelper.EnumToCode(DXC.Technology.Common.Enumerations.TechnologyUserCodeEnum.ACMSG_ActivityMessages));
//            //}

//            //DXC.Technology.UnitTesting.TestData.Populators.Common.TestFileGenerator.CleanupTestFiles();

//            //System.Diagnostics.Trace.WriteLine(new DXC.Technology.Listeners.TraceMessage("Webservices Started", -854, "Business.Global.asax", DXC.Technology.Listeners.TraceCategoryEnum.Info));
//        }

//        /// <summary>
//        /// Initializes a test class.
//        /// </summary>
//        /// <param name="unitTestType">The type of the unit test.</param>
//        /// <param name="className">The name of the test class.</param>
//        /// <param name="testContext">The test context.</param>
//        public static void HelpTestClassInitialization(TechnologyUnitTestTypeEnum unitTestType, string className, TestContext testContext)
//        {
//            string testTypeTestClassId = string.Format("{0}/{1}", unitTestType, className);
//            LogTestEvent("Test Class Initialization", unitTestType, className, string.Empty);

//            unitTestOverview.ActiveUnitTestTypeAsEnum = unitTestType;
//            unitTestOverview.ActiveTestClass = className;
//            unitTestOverview.ActiveUnitTestClassResultRow.LogStart();
//            unitTestOverview.ActiveUnitTestTypeResultRow.LogStart();
//        }

//        /// <summary>
//        /// Cleans up a test class.
//        /// </summary>
//        /// <param name="unitTestType">The type of the unit test.</param>
//        /// <param name="className">The name of the test class.</param>
//        public static void HelpTestClassCleanup(TechnologyUnitTestTypeEnum unitTestType, string className)
//        {
//            unitTestOverview.ActiveTestClass = className;
//            unitTestOverview.ActiveUnitTestTypeAsEnum = unitTestType;
//            unitTestOverview.ActiveUnitTestClassResultRow.LogEnd();
//            unitTestOverview.ActiveUnitTestTypeResultRow.LogEnd();
//            LogTestEvent("Test Class Cleanup", unitTestType, className, string.Empty);
//        }

//        /// <summary>
//        /// Initializes a test.
//        /// </summary>
//        /// <param name="unitTestType">The type of the unit test.</param>
//        /// <param name="className">The name of the test class.</param>
//        /// <param name="testContext">The test context.</param>
//        public static void HelpTestInitialization(TechnologyUnitTestTypeEnum unitTestType, string className, TestContext testContext)
//        {
//            testContext.Properties.Add("ActiveTestClass", className);
//            testContext.Properties.Add("ActiveUnitTestTypeAsEnum", unitTestType);
//            testContext.Properties.Add("ActiveTestMethod", className);
//            unitTestOverview.ActiveTestClass = className;
//            unitTestOverview.ActiveUnitTestTypeAsEnum = unitTestType;
//            unitTestOverview.ActiveTestMethod = testContext.TestName;
//            unitTestOverview.ActiveUnitTestResultRow.LogStart();
//            LogTestEvent("Test Initialization", unitTestType, className, string.Empty);
//        }

//        /// <summary>
//        /// Cleans up a test.
//        /// </summary>
//        /// <param name="unitTestType">The type of the unit test.</param>
//        /// <param name="className">The name of the test class.</param>
//        /// <param name="testContext">The test context.</param>
//        public static void HelpTestCleanup(TechnologyUnitTestTypeEnum unitTestType, string className, TestContext testContext)
//        {
//            unitTestOverview.ActiveTestClass = className;
//            unitTestOverview.ActiveUnitTestTypeAsEnum = unitTestType;
//            unitTestOverview.ActiveTestMethod = testContext.TestName;
//            unitTestOverview.ActiveUnitTestResultRow.LogEnd(testContext.CurrentTestOutcome);
//            unitTestOverview.ActiveUnitTestClassResultRow.LogEnd(testContext.CurrentTestOutcome);
//            unitTestOverview.ActiveUnitTestTypeResultRow.LogEnd(testContext.CurrentTestOutcome);
//            unitTestOverview.SummaryResultRow.LogEnd(testContext.CurrentTestOutcome);
//            LogTestEvent("Test Cleanup", unitTestType, className, string.Empty);
//        }

//        #endregion

//        #region Private Methods

//        /// <summary>
//        /// Retrieves a unit test result row.
//        /// </summary>
//        /// <param name="unitTestType">The type of the unit test.</param>
//        /// <param name="className">The name of the test class.</param>
//        /// <param name="methodName">The name of the test method.</param>
//        /// <returns>The unit test result row.</returns>
//        private static UnitTestOverviewDataSet.UnitTestResultRow GetUnitTestResultRow(TechnologyUnitTestTypeEnum unitTestType, string className, string methodName)
//        {
//            return GetUnitTestResultRow(unitTestType.ToString(), className, methodName);
//        }

//        /// <summary>
//        /// Retrieves a unit test result row.
//        /// </summary>
//        /// <param name="unitTestType">The type of the unit test as a string.</param>
//        /// <param name="className">The name of the test class.</param>
//        /// <param name="methodName">The name of the test method.</param>
//        /// <returns>The unit test result row.</returns>
//        private static UnitTestOverviewDataSet.UnitTestResultRow GetUnitTestResultRow(string unitTestType, string className, string methodName)
//        {
//            return unitTestOverview.GetUnitTestResultRow(unitTestType, className, methodName);
//        }

//        /// <summary>
//        /// Logs a test event.
//        /// </summary>
//        /// <param name="eventMessage">The message describing the event.</param>
//        /// <param name="unitTestType">The type of the unit test.</param>
//        /// <param name="className">The name of the test class.</param>
//        /// <param name="methodName">The name of the test method.</param>
//        private static void LogTestEvent(string eventMessage, TechnologyUnitTestTypeEnum unitTestType, string className, string methodName)
//        {
//            string testTypeTestClassId = string.Format("{0}/{1}", unitTestType, className);
//            string message = string.Format("{0}|{1}|{2}|{3}", unitTestType, className, methodName, eventMessage);
//            DXC.Technology.Listeners.TraceMessage traceMessage = new DXC.Technology.Listeners.TraceMessage(message, -6000, "TechnologyUnitTestHelper", DXC.Technology.Listeners.TraceCategoryEnum.UnitTest);
//            System.Diagnostics.Trace.Write(traceMessage);
//        }

//        #endregion
//    }
//}