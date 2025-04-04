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
    #region Enums

    /// <summary>
    /// Specifies whether OpenIdAuthentication is active or not
    /// </summary>
    public enum OpenIdAuthenticationModeEnum
    {
        On = 0,
        Off = 1
    }

    /// <summary>
    /// Provides the enabler for realizing OpenIdAuthentication
    /// </summary>
    public enum OpenIdAuthenticationProviderEnum
    {
        /// <summary>
        /// Initializer
        /// </summary>
        None = 0,
        /// <summary>
        /// OpenIdAuthentication will take place versus a "custom Database"
        /// </summary>
        JWT = 1
    }

    #endregion

    public class OpenIdAuthenticationOptionsSets
    {
        #region Public Properties

        /// <summary>
        /// Collection of OpenIdAuthenticationOptions
        /// </summary>
        public OpenIdAuthenticationOptions[] Sets { get; set; }

        #endregion
    }

    public class OpenIdAuthenticationOptions
    {
        #region Public Properties

        /// <summary>
        /// Purpose of the OpenIdAuthentication
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// Mode of OpenIdAuthentication as a string
        /// </summary>
        public string OpenIdAuthenticationMode { get; set; }

        /// <summary>
        /// ApplicationSecurity indicates if security is on or off
        /// </summary>
        public OpenIdAuthenticationModeEnum OpenIdAuthenticationModeAsEnum
        {
            get
            {
                return DXC.Technology.Enumerations.EnumerationHelper.CodeToEnum<OpenIdAuthenticationModeEnum>(OpenIdAuthenticationMode);
            }
            set
            {
                OpenIdAuthenticationMode = DXC.Technology.Enumerations.EnumerationHelper.EnumToCode(value);
            }
        }

        /// <summary>
        /// Provider of OpenIdAuthentication as a string
        /// </summary>
        public string OpenIdAuthenticationProvider { get; set; }

        /// <summary>
        /// ApplicationSecurity indicates if security is on or off
        /// </summary>
        public OpenIdAuthenticationProviderEnum OpenIdAuthenticationProviderAsEnum
        {
            get
            {
                return DXC.Technology.Enumerations.EnumerationHelper.CodeToEnum<OpenIdAuthenticationProviderEnum>(OpenIdAuthenticationProvider);
            }
            set
            {
                OpenIdAuthenticationProvider = DXC.Technology.Enumerations.EnumerationHelper.EnumToCode(value);
            }
        }

        /// <summary>
        /// Key used for OpenIdAuthentication
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Issuer of the OpenIdAuthentication token
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Audience for the OpenIdAuthentication token
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Token audience for OpenIdAuthentication
        /// </summary>
        public string TokenAudience { get; set; }

        /// <summary>
        /// Base URL for OpenIdAuthentication
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Application ID for OpenIdAuthentication
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Client ID for OpenIdAuthentication
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Callback endpoint for OpenIdAuthentication
        /// </summary>
        public string CallbackEndpoint { get; set; }

        /// <summary>
        /// End session endpoint for OpenIdAuthentication
        /// </summary>
        public string EndSessionEndpoint { get; set; }

        /// <summary>
        /// Authorization endpoint for OpenIdAuthentication
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Token subject for OpenIdAuthentication
        /// </summary>
        public string TokenSubject { get; set; }

        /// <summary>
        /// Token issuer for OpenIdAuthentication
        /// </summary>
        public string TokenIssuer { get; set; }

        /// <summary>
        /// Token endpoint for OpenIdAuthentication
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// Grant types for OpenIdAuthentication
        /// </summary>
        public string GrantTypes { get; set; }

        /// <summary>
        /// Scope for OpenIdAuthentication
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Proxy server for OpenIdAuthentication
        /// </summary>
        public string ProxyServer { get; set; }

        /// <summary>
        /// Certificate Key Identifier (Kid) for OpenIdAuthentication
        /// </summary>
        public string CertificateKid { get; set; }

        /// <summary>
        /// Certificate thumbprint for OpenIdAuthentication
        /// </summary>
        public string CertificateThumbPrint { get; set; }

        /// <summary>
        /// Certificate name for OpenIdAuthentication
        /// </summary>
        public string CertificateName { get; set; }

        /// <summary>
        /// Certificate Key Vault URL for OpenIdAuthentication
        /// </summary>
        public string CertificateKeyVaultUrl { get; set; }

        #endregion
    }

    public class OpenIdAuthenticationOptionsAccessor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OpenIdAuthenticationOptionsAccessor class.
        /// </summary>
        /// <param name="optionsAccessor">Options accessor for OpenIdAuthenticationOptions</param>
        public OpenIdAuthenticationOptionsAccessor(IOptions<OpenIdAuthenticationOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// OpenIdAuthenticationOptions set via Secret Manager
        /// </summary>
        public OpenIdAuthenticationOptions Options { get; } //set only via Secret Manager

        #endregion
    }

    public class OpenIdAuthenticationOptionsSetsAccessor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OpenIdAuthenticationOptionsSetsAccessor class.
        /// </summary>
        /// <param name="optionsSetsAccessor">Options accessor for OpenIdAuthenticationOptionsSets</param>
        public OpenIdAuthenticationOptionsSetsAccessor(IOptions<OpenIdAuthenticationOptionsSets> optionsSetsAccessor)
        {
            OptionsSets = optionsSetsAccessor.Value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// OpenIdAuthenticationOptionsSets set via Secret Manager
        /// </summary>
        public OpenIdAuthenticationOptionsSets OptionsSets { get; } //set only via Secret Manager

        #endregion
    }

    public class OpenIdAuthenticationOptionsManager
    {
        #region Static Fields

        /// <summary>
        /// Singleton instance of OpenIdAuthenticationOptionsManager
        /// </summary>
        private static OpenIdAuthenticationOptionsManager current = null;

        #endregion

        #region Private Static Properties

        /// <summary>
        /// Current HttpContext
        /// </summary>
        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the singleton instance of OpenIdAuthenticationOptionsManager
        /// </summary>
        public static OpenIdAuthenticationOptionsManager Current
        {
            get
            {
                if (current == null)
                    current = new OpenIdAuthenticationOptionsManager();
                return current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the OpenIdAuthenticationOptions from the current HttpContext
        /// </summary>
        /// <returns>OpenIdAuthenticationOptions</returns>
        public OpenIdAuthenticationOptions GetOpenIdAuthenticationOptions()
        {
            return HttpContext.RequestServices.GetRequiredService<OpenIdAuthenticationOptionsAccessor>().Options;
        }

        #endregion
    }

    public class OpenIdAuthenticationOptionsSetsManager
    {
        #region Static Fields

        /// <summary>
        /// Singleton instance of OpenIdAuthenticationOptionsSetsManager
        /// </summary>
        private static OpenIdAuthenticationOptionsSetsManager current = null;

        #endregion

        #region Private Static Properties

        /// <summary>
        /// Current HttpContext
        /// </summary>
        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the singleton instance of OpenIdAuthenticationOptionsSetsManager
        /// </summary>
        public static OpenIdAuthenticationOptionsSetsManager Current
        {
            get
            {
                if (current == null)
                    current = new OpenIdAuthenticationOptionsSetsManager();
                return current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the OpenIdAuthenticationOptionsSets from the current HttpContext
        /// </summary>
        /// <returns>OpenIdAuthenticationOptionsSets</returns>
        public OpenIdAuthenticationOptionsSets GetOpenIdAuthenticationOptionsSets()
        {
            return HttpContext.RequestServices.GetRequiredService<OpenIdAuthenticationOptionsSetsAccessor>().OptionsSets;
        }

        #endregion
    }
}