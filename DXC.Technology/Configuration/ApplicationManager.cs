using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.Technology.Configuration;
using DXC.Technology.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DXC.Technology.Configuration
{
    /// <summary>
    /// ApplicationSecurity indicates if security is on or off
    /// </summary>
    public enum ApplicationModeEnum
    {
        Normal = 0,
        Debug = 1,
        ShowUntranslated = 2,
        LogUntranslated = 3,
    }

    public class ApplicationOptions
    {
        #region Public Properties

        /// <summary>
        /// The mode of the application (e.g., Normal, Debug).
        /// </summary>
        public string ApplicationMode { get; set; }

        /// <summary>
        /// ApplicationSecurity indicates if security is on or off
        /// </summary>
        public ApplicationModeEnum ApplicationModeAsEnum
        {
            get
            {
                return DXC.Technology.Enumerations.EnumerationHelper.CodeToEnum<ApplicationModeEnum>(this.ApplicationMode);
            }
            set
            {
                this.ApplicationMode = DXC.Technology.Enumerations.EnumerationHelper.EnumToCode(value);
            }
        }

        /// <summary>
        /// Indicates the security level of the application.
        /// </summary>
        public string ApplicationSecurity { get; set; }

        /// <summary>
        /// The tier of the application (e.g., frontend, backend).
        /// </summary>
        public string ApplicationTier { get; set; }

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The root URL of the application.
        /// </summary>
        public string ApplicationRootUrl { get; set; }

        /// <summary>
        /// The root URL for redirection purposes.
        /// </summary>
        public string RedirectRootUrl { get; set; }

        /// <summary>
        /// The absolute root URL of the application.
        /// </summary>
        public string AbsoluteRootUrl { get; set; }

        /// <summary>
        /// The root URL for interfacing purposes.
        /// </summary>
        public string InterfacingRoot { get; set; }

        /// <summary>
        /// The root URL for referral purposes.
        /// </summary>
        public string ReferralRootUrl { get; set; }

        /// <summary>
        /// The URL for accessing external information.
        /// </summary>
        public string ExternalInformationUrl { get; set; }

        /// <summary>
        /// The root URL for REST API access.
        /// </summary>
        public string RestApplicationRootUrl { get; set; }

        /// <summary>
        /// The root URL for attachment-related operations.
        /// </summary>
        public string AttachmentApplicationRootUrl { get; set; }

        /// <summary>
        /// The root URL for OpenAPI access.
        /// </summary>
        public string OpenApiApplicationRootUrl { get; set; }

        /// <summary>
        /// The URL for common WCF services.
        /// </summary>
        public string WcfServicesCommonUrl { get; set; }

        /// <summary>
        /// The URL for business WCF services.
        /// </summary>
        public string WcfServicesBusinessUrl { get; set; }

        /// <summary>
        /// The encryptor used for auto logon.
        /// </summary>
        public string AutoLogOnEncryptor { get; set; }

        /// <summary>
        /// The encryptor used for web requests.
        /// </summary>
        public string WebRequestEncryptor { get; set; }

        /// <summary>
        /// Indicates whether web request encryption is mandatory.
        /// </summary>
        public bool WebRequestEncryptionMandatory { get; set; }

        /// <summary>
        /// The default country for the application.
        /// </summary>
        public string DefaultCountry { get; set; }

        /// <summary>
        /// A CSV list of default roles.
        /// </summary>
        public string DefaultRolesCsv { get; set; }

        /// <summary>
        /// The default menu for the application.
        /// </summary>
        public string DefaultMenu { get; set; }

        /// <summary>
        /// The default organization for the application.
        /// </summary>
        public string DefaultOrganization { get; set; }

        /// <summary>
        /// The default managing organization for the application.
        /// </summary>
        public string DefaultManagingOrganzation { get; set; }

        /// <summary>
        /// The default department for the application.
        /// </summary>
        public string DefaultDepartment { get; set; }

        /// <summary>
        /// The default party for the application.
        /// </summary>
        public string DefaultParty { get; set; }

        /// <summary>
        /// The default facility for the application.
        /// </summary>
        public string DefaultFacility { get; set; }

        /// <summary>
        /// The default managing organization for the application.
        /// </summary>
        public string DefaultManagingOrganization { get; set; }

        /// <summary>
        /// Indicates whether blockchain is enabled.
        /// </summary>
        public bool BlockChainEnabled { get; set; }

        /// <summary>
        /// The binding type for the application.
        /// </summary>
        public int BindingType { get; set; }

        /// <summary>
        /// Indicates whether data scrambling is enabled.
        /// </summary>
        public bool ScrumbleData { get; set; }

        /// <summary>
        /// Indicates whether the application operates in a session-less mode.
        /// </summary>
        public bool SessionLess { get; set; }

        /// <summary>
        /// The default application for the environment.
        /// </summary>
        public string DefaultApplication { get; set; }

        /// <summary>
        /// The default affinity for the application.
        /// </summary>
        public string DefaultAffinity { get; set; }

        /// <summary>
        /// The site affinity for the application.
        /// </summary>
        public string SiteAffinity { get; set; }

        /// <summary>
        /// The base name for application resources.
        /// </summary>
        public string ApplicationResourceBaseName { get; set; }

        /// <summary>
        /// The languages supported by the application.
        /// </summary>
        public string ApplicationLanguages { get; set; }

        /// <summary>
        /// The default language for the application.
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// The caching time in hours.
        /// </summary>
        public int CachingTimeInHours { get; set; }

        /// <summary>
        /// The time in minutes after which a session warning is shown.
        /// </summary>
        public string StartSessionWarningAfterMinutes { get; set; }

        /// <summary>
        /// The time in minutes after which a session expiration message is shown.
        /// </summary>
        public string ShowSessionExpiredAfterMinutes { get; set; }

        /// <summary>
        /// Indicates whether zero blockchain transactions are enabled.
        /// </summary>
        public string ZeroBlockChainTransactionEnabled { get; set; }

        /// <summary>
        /// The root server path for uploads.
        /// </summary>
        public string UploadRootServerPath { get; set; }

        /// <summary>
        /// The root server path for temporary files.
        /// </summary>
        public string TempFilesRootServerPath { get; set; }

        /// <summary>
        /// The root server path for CKEditor uploads.
        /// </summary>
        public string CkEditorUploadRootServerPath { get; set; }

        /// <summary>
        /// The root URL for CKEditor uploads.
        /// </summary>
        public string CkEditorUploadRootUrl { get; set; }

        /// <summary>
        /// The URL for retrieving CKEditor images from the database.
        /// </summary>
        public string CkEditorImageFromDbUrl { get; set; }

        /// <summary>
        /// The header widget for MBP.
        /// </summary>
        public string MbpHeaderWidget { get; set; }

        /// <summary>
        /// The footer widget for MBP.
        /// </summary>
        public string MbpFooterWidget { get; set; }

        /// <summary>
        /// The client ID for notifications.
        /// </summary>
        public string NotificationClientId { get; set; }

        /// <summary>
        /// The client ID for test APIs.
        /// </summary>
        public string TestApiClientId { get; set; }

        /// <summary>
        /// The environment of the application (e.g., DEV, PROD).
        /// </summary>
        public string ApplicationEnvironment { get; set; }

        /// <summary>
        /// The access methods allowed for the application.
        /// </summary>
        public string ApplicationAccessMethods { get; set; }

        /// <summary>
        /// The storage location for Defender AV.
        /// </summary>
        public string DefenderAvStorage { get; set; }

        /// <summary>
        /// The server address for ClamAV.
        /// </summary>
        public string ClamAvServer { get; set; }

        /// <summary>
        /// The port number for the ClamAV server.
        /// </summary>
        public int ClamAvServerPort { get; set; }

        /// <summary>
        /// The client ID for managed identity.
        /// </summary>
        public string ManagedIdentityClientId { get; set; }

        /// <summary>
        /// The IPDC configuration for the application.
        /// </summary>
        public string Ipdc { get; set; }

        /// <summary>
        /// The notification URL for MBP.
        /// </summary>
        public string MbpNotificationUrl { get; set; }

        /// <summary>
        /// The URL for identity management.
        /// </summary>
        public string IdentityManagementUrl { get; set; }

        /// <summary>
        /// The client ID for the service bus managed identity.
        /// </summary>
        public string ServiceBusManagedIdentityClientId { get; set; }

        /// <summary>
        /// The URI for the service bus.
        /// </summary>
        public string ServiceBusUri { get; set; }

        /// <summary>
        /// The delay in milliseconds for batch items.
        /// </summary>
        public string BatchItemDelayInMilliseconds { get; set; }

        /// <summary>
        /// The webhook URL for the service bus.
        /// </summary>
        public string ServiceBusWebHookUrl { get; set; }

        /// <summary>
        /// The tracking URL for the site.
        /// </summary>
        public string SiteTrackingUrl { get; set; }

        /// <summary>
        /// The site ID for tracking purposes.
        /// </summary>
        public string SiteTrackingSiteId { get; set; }

        /// <summary>
        /// ApplicationVersion is the version of the application
        /// </summary>
        public HashEncryptionHelper ApplicationHashEncryptionHelper
        {
            get
            {
                //Todo - configure the default hashing algorithm
                return new HashEncryptionHelper(HashAlgorithmEnum.SHA256, System.Text.ASCIIEncoding.ASCII);
            }
        }

        /// <summary>
        /// The path for interfacing documents.
        /// </summary>
        public string InterfacingDocumentsPath { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the application cookie suffix based on the environment.
        /// </summary>
        /// <returns>The application cookie suffix.</returns>
        public string ApplicationCookieSuffix()
        {
            return ApplicationCookieSuffix(ApplicationEnvironment);
        }

        /// <summary>
        /// Checks if access is allowed via ACM.
        /// </summary>
        /// <returns>True if access is allowed via ACM; otherwise, false.</returns>
        public bool AccessViaAcm()
        {
            return (!string.IsNullOrEmpty(ApplicationAccessMethods) && ApplicationAccessMethods.Contains("ACM"));
        }

        /// <summary>
        /// Checks if access is allowed via IDM.
        /// </summary>
        /// <returns>True if access is allowed via IDM; otherwise, false.</returns>
        public bool AccessViaIdm()
        {
            return (!string.IsNullOrEmpty(ApplicationAccessMethods) && ApplicationAccessMethods.Contains("IDM"));
        }

        /// <summary>
        /// Checks if access is allowed via simulation.
        /// </summary>
        /// <returns>True if access is allowed via simulation; otherwise, false.</returns>
        public bool AccessViaSimulation()
        {
            return (string.IsNullOrEmpty(ApplicationAccessMethods) || ApplicationAccessMethods.Contains("SIM"));
        }

        /// <summary>
        /// Checks if access is allowed via a system account.
        /// </summary>
        /// <returns>True if access is allowed via a system account; otherwise, false.</returns>
        public bool AccessViaSystemAccount()
        {
            return (string.IsNullOrEmpty(ApplicationAccessMethods) || ApplicationAccessMethods.Contains("SYS"));
        }

        /// <summary>
        /// Generates the application cookie suffix based on the specified environment.
        /// </summary>
        /// <param name="applicationEnvironment">The application environment.</param>
        /// <returns>The application cookie suffix.</returns>
        public static string ApplicationCookieSuffix(string applicationEnvironment)
        {
            switch (applicationEnvironment)
            {
                case "DEV":
                case "TEST":
                case "ACPT":
                case "PPRD":
                case "MIG":
                    return "_" + applicationEnvironment;
                case "PROD":
                    return "";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Converts the batch item delay from a string to an integer value.
        /// </summary>
        /// <returns>The batch item delay in milliseconds as an integer.</returns>
        public int BatchItemDelayInMillisecondsAsValue()
        {
            if (int.TryParse(BatchItemDelayInMilliseconds, out int result))
                return result;
            return 0;
        }

        #endregion
    }

    public class ApplicationOptionsAccessor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationOptionsAccessor"/> class.
        /// </summary>
        /// <param name="optionsAccessor">The options accessor for application options.</param>
        public ApplicationOptionsAccessor(IOptions<ApplicationOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the application options.
        /// </summary>
        public ApplicationOptions Options { get; } //set only via Secret Manager

        #endregion
    }

    public class ApplicationOptionsManager
    {
        #region Static Fields

        private static ApplicationOptionsManager current = null;

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the current HTTP context.
        /// </summary>
        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        /// <summary>
        /// Gets the current instance of the <see cref="ApplicationOptionsManager"/>.
        /// </summary>
        public static ApplicationOptionsManager Current
        {
            get
            {
                if (current == null)
                    current = new ApplicationOptionsManager();
                return current;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates whether the application is initialized.
        /// </summary>
        public bool ApplicationInitialized { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the application options from the current HTTP context.
        /// </summary>
        /// <returns>The application options.</returns>
        public ApplicationOptions GetApplicationOptions()
        {
            return HttpContext.RequestServices.GetRequiredService<ApplicationOptionsAccessor>().Options;
        }

        #endregion
    }
}