//using System;
//using DXC.Technology.Data.General;

//namespace DXC.Technology.UnitTesting.Helpers
//{
//    /// <summary>
//    /// Helper class for managing technology keys.
//    /// </summary>
//    public static class TechnologyKeyHelper
//    {
//        #region Public Static Methods

//        /// <summary>
//        /// Retrieves the highest ID for a given functionality.
//        /// </summary>
//        /// <param name="functionality">The functionality name.</param>
//        /// <returns>The highest ID.</returns>
//        public static long GetHighestId(string functionality)
//        {
//            try
//            {
//                return functionality switch
//                {
//                    "BlockChain" => GetHighestBlockChainTransactionId(),
//                    "Timeslot" => GetHighestTimeslotId(),
//                    "BulletinBoard" => GetHighestBulletinBoardId(),
//                    "PrintAction" => GetHighestPrintActionId(),
//                    "Document" => GetHighestDocumentId(),
//                    "TextContent" => GetHighestTextContentId(),
//                    "AttachmentInfo" => GetHighestAttachmentInfoId(),
//                    "AttachmentSet" => GetHighestAttachmentSetId(),
//                    "Assessment" => GetHighestAssessmentId(),
//                    "Probe" => GetHighestProbeId(),
//                    "WorkflowInstance" => GetHighestWorkflowInstanceId(),
//                    _ => throw new ArgumentException("Invalid functionality", nameof(functionality))
//                };
//            }
//            catch (Exception)
//            {
//                TechnologyAssertHelper.Fail("GetHighestId for " + functionality);
//            }
//            return 0;
//        }

//        /// <summary>
//        /// Retrieves the highest code for a given functionality.
//        /// </summary>
//        /// <param name="functionality">The functionality name.</param>
//        /// <returns>The highest code.</returns>
//        public static string GetHighestCode(string functionality)
//        {
//            try
//            {
//                return functionality switch
//                {
//                    "User" => GetHighestUserCode(),
//                    "Information" => GetHighestInformationCode(),
//                    "FlexibleReporting" => GetHighestFlexibleReportingCode(),
//                    "BlockChainParty" => GetHighestBlockChainPartyCode(),
//                    _ => throw new ArgumentException("Invalid functionality", nameof(functionality))
//                };
//            }
//            catch (Exception)
//            {
//                TechnologyAssertHelper.Fail("GetHighestCode for " + functionality);
//            }
//            return string.Empty;
//        }

//        /// <summary>
//        /// Retrieves the highest BlockChain transaction ID.
//        /// </summary>
//        public static long GetHighestBlockChainTransactionId() => KeyDataManager.GetHighestId("BlockChainTransaction", "Id");

//        /// <summary>
//        /// Retrieves the highest BlockChain party code.
//        /// </summary>
//        public static string GetHighestBlockChainPartyCode() => KeyDataManager.GetHighestCode("BlockChainParty", "Identity");

//        /// <summary>
//        /// Retrieves the highest Timeslot ID.
//        /// </summary>
//        public static long GetHighestTimeslotId() => KeyDataManager.GetHighestId("Timeslot", "Id");

//        /// <summary>
//        /// Retrieves the highest BulletinBoard ID.
//        /// </summary>
//        public static long GetHighestBulletinBoardId() => KeyDataManager.GetHighestId("BulletinBoard", "BulletinBoardId");

//        /// <summary>
//        /// Retrieves the highest CustomerApplication ID.
//        /// </summary>
//        public static long GetHighestCustomerApplicationId() => KeyDataManager.GetHighestId("CustomerApplication", "CUAP_ID");

//        /// <summary>
//        /// Retrieves the highest PrintAction ID.
//        /// </summary>
//        public static long GetHighestPrintActionId() => KeyDataManager.GetHighestId("PrintAction", "PrintActionId");

//        /// <summary>
//        /// Retrieves the highest Document ID.
//        /// </summary>
//        public static long GetHighestDocumentId() => KeyDataManager.GetHighestId("TextContentDocument", "TextContentDocumentId");

//        /// <summary>
//        /// Retrieves the highest TextContent ID.
//        /// </summary>
//        public static long GetHighestTextContentId() => KeyDataManager.GetHighestId("TextContent", "TextContentId");

//        /// <summary>
//        /// Retrieves the highest AttachmentInfo ID.
//        /// </summary>
//        public static long GetHighestAttachmentInfoId() => KeyDataManager.GetHighestId("AttachmentInfo", "Id");

//        /// <summary>
//        /// Retrieves the highest AttachmentSet ID.
//        /// </summary>
//        public static long GetHighestAttachmentSetId() => KeyDataManager.GetHighestId("AttachmentSet", "Id");

//        /// <summary>
//        /// Retrieves the highest Assessment ID.
//        /// </summary>
//        public static long GetHighestAssessmentId() => KeyDataManager.GetHighestId("DynamicInformationGroup", "Owner");

//        /// <summary>
//        /// Retrieves the highest Probe ID.
//        /// </summary>
//        public static long GetHighestProbeId() => KeyDataManager.GetHighestId("Probe", "ProbeId");

//        /// <summary>
//        /// Retrieves the highest WorkflowInstance ID.
//        /// </summary>
//        public static long GetHighestWorkflowInstanceId() => KeyDataManager.GetHighestId("WorkflowInstance", "Id");

//        /// <summary>
//        /// Retrieves the highest User code.
//        /// </summary>
//        public static string GetHighestUserCode() => KeyDataManager.GetHighestCode("LogOn", "UserName");

//        /// <summary>
//        /// Retrieves the highest Information code.
//        /// </summary>
//        public static string GetHighestInformationCode() => KeyDataManager.GetHighestCode("TextTemplateDefinition", "TextTemplateCode");

//        /// <summary>
//        /// Retrieves the highest FlexibleReporting code.
//        /// </summary>
//        public static string GetHighestFlexibleReportingCode() => KeyDataManager.GetHighestCode("ReportingFlexibleReport", "ReportCode");

//        #endregion
//    }
//}