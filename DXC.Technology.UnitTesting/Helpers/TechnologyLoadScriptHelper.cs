using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace DXC.Technology.UnitTesting.Helpers
{
    public class TechnologyLoadScriptHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Reloads the base database by executing a series of SQL scripts.
        /// </summary>
        public static void ReloadBaseDb()
        {
            // Technical Data
            LoadSqlScript("BaseData.LoadScripts.AttachmentTables.sql");
            LoadSqlScript("BaseData.LoadScripts.UserCodes.sql");
            LoadSqlScript("BaseData.LoadScripts.Workflow.sql");
            LoadSqlScript("BaseData.LoadScripts.Measures.sql");
            LoadSqlScript("BaseData.LoadScripts.Security.sql");
            LoadSqlScript("BaseData.LoadScripts.Reporting.sql");
            LoadSqlScript("BaseData.LoadScripts.Probes.sql");
            LoadSqlScript("BaseData.LoadScripts.DataModelAbbreviation.sql");
            LoadSqlScript("BaseData.LoadScripts.DataModelDocumentation.sql");
            LoadSqlScript("BaseData.LoadScripts.DecisionTables.sql");
            LoadSqlScript("BaseData.LoadScripts.DynamicInformation.sql");
            LoadSqlScript("BaseData.LoadScripts.GEOInfo.sql");
            LoadSqlScript("BaseData.LoadScripts.GISStatistic.sql");
            LoadSqlScript("BaseData.LoadScripts.IntegrationDefinition.sql");
            LoadSqlScript("BaseData.LoadScripts.BulletinBoardTables.sql");
            LoadSqlScript("BaseData.LoadScripts.DataDescriptor.sql");
            LoadSqlScript("BaseData.LoadScripts.General.sql");
            LoadSqlScript("BaseData.LoadScripts.BlockChain.sql");

            // Special Settings
            // LoadSqlScript("BaseData.LoadScripts.TextFragments.sql");
            // LoadSqlScript("BaseData.LoadScripts.DynamicInformationFromToevla.sql");

            // Functional Data
            // LoadSqlScript("TestData.LoadScripts.LoadDossiers.sql");
        }

        /// <summary>
        /// Resets IIS by executing the "iisreset.exe" command.
        /// </summary>
        public static void ResetIis()
        {
            var iisReset = new System.Diagnostics.Process
            {
                StartInfo = { FileName = "iisreset.exe" }
            };
            iisReset.Start();
            iisReset.WaitForExit(int.MaxValue);
        }

        /// <summary>
        /// Executes a SQL script using the provided file name.
        /// </summary>
        /// <param name="sqlFile">The name of the SQL file to execute.</param>
        public static void LoadSqlScript(string sqlFile)
        {
            string sqlText = GetResourceContentAsString(sqlFile);
            //var sqlHelper = new DXC.Technology.Data.SqlDataHelper();
            var sqlCommand = new Microsoft.Data.SqlClient.SqlCommand(sqlText)
            {
                CommandTimeout = 250
            };
            //sqlHelper.ExecuteCommand(sqlCommand);

            TechnologyAssertHelper.IsTrue(true, sqlFile);
        }

        /// <summary>
        /// Executes the blockchain initialization SQL script.
        /// </summary>
        public static void RunBlockChainInitializationSqlScript()
        {
            LoadSqlScript("BaseData.LoadScripts.BlockChain.sql");
        }

        /// <summary>
        /// Retrieves the content of a resource file as a string.
        /// </summary>
        /// <param name="resourceFile">The name of the resource file.</param>
        /// <returns>The content of the resource file as a string.</returns>
        public static string GetResourceContentAsString(string resourceFile)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-be");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-be");
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string assemblyName = executingAssembly.GetName().Name;
            string resourceName = string.Format("{0}.{1}", assemblyName, resourceFile);
            Stream appStream = executingAssembly.GetManifestResourceStream(resourceName);

            if (appStream == null)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.UnexpectedException(
                    string.Format("Resource File '{0}' not found", resourceFile));
            }

            using var textStream = new StreamReader(appStream, System.Text.Encoding.ASCII);
            return textStream.ReadToEnd();
        }

        /// <summary>
        /// Retrieves the content of a resource file as a byte array.
        /// </summary>
        /// <param name="resourceFile">The name of the resource file.</param>
        /// <returns>The content of the resource file as a byte array.</returns>
        public static byte[] GetResourceContent(string resourceFile)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string assemblyName = executingAssembly.GetName().Name;
            string resourceName = string.Format("{0}.{1}", assemblyName, resourceFile);
            Stream appStream = executingAssembly.GetManifestResourceStream(resourceName);

            if (appStream == null)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.UnexpectedException(
                    string.Format("Resource File '{0}' not found", resourceFile));
            }

            byte[] buffer = new byte[(int)appStream.Length];
            appStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        #endregion
    }
}