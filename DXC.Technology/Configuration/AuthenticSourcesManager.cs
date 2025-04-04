using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DXC.Technology.Configuration
{
    /// <summary>
    /// Options for authentic sources configuration.
    /// </summary>
    public class AuthenticSourcesOptions
    {
        #region Public Properties

        /// <summary>
        /// URL for WegWijs API.
        /// </summary>
        public string WegWijsApiUrl { get; set; }

        /// <summary>
        /// API Key for WegWijs.
        /// </summary>
        public string WegWijsApiKey { get; set; }

        /// <summary>
        /// URL for Adres Historiek.
        /// </summary>
        public string AdresHistoriekUrl { get; set; }

        /// <summary>
        /// URL for Gezins Samenstelling.
        /// </summary>
        public string GezinsSamenstellingUrl { get; set; }

        /// <summary>
        /// URL for Kinderbijslag.
        /// </summary>
        public string KinderbijslagUrl { get; set; }

        /// <summary>
        /// URL for Kadaster Gegevens.
        /// </summary>
        public string KadasterGegevensUrl { get; set; }

        /// <summary>
        /// URL for Inkomen Gegevens.
        /// </summary>
        public string InkomenGegevensUrl { get; set; }

        /// <summary>
        /// URL for Pensioen Gegevens.
        /// </summary>
        public string PensioenGegevensUrl { get; set; }

        /// <summary>
        /// URL for Leefloon Gegevens.
        /// </summary>
        public string LeefloonGegevensUrl { get; set; }

        /// <summary>
        /// URL for Persoons Gegevens.
        /// </summary>
        public string PersoonsGegevensUrl { get; set; }

        /// <summary>
        /// URL for Handicap Gegevens.
        /// </summary>
        public string HandicapGegevensUrl { get; set; }

        /// <summary>
        /// URL for Inburgering.
        /// </summary>
        public string InburgeringUrl { get; set; }

        /// <summary>
        /// URL for Werkloosheid Gegevens.
        /// </summary>
        public string WerkloosheidGegevensUrl { get; set; }

        /// <summary>
        /// URL for Wettelijke Vertegenwoordiger Gegevens.
        /// </summary>
        public string WettelijkeVertegenwoordigerGegevensUrl { get; set; }

        /// <summary>
        /// Application ID.
        /// </summary>
        public string ApplicatieId { get; set; }

        /// <summary>
        /// KBO Number.
        /// </summary>
        public string KboNummer { get; set; }

        /// <summary>
        /// URL for CRPR Huurpremie.
        /// </summary>
        public string CrprHuurpremieUrl { get; set; }

        /// <summary>
        /// Username for CRPR.
        /// </summary>
        public string CrprUsername { get; set; }

        /// <summary>
        /// Password for CRPR.
        /// </summary>
        public string CrprPassword { get; set; }

        #endregion
    }

    /// <summary>
    /// Accessor for AuthenticSourcesOptions.
    /// </summary>
    public class AuthenticSourcesOptionsAccessor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticSourcesOptionsAccessor"/> class.
        /// </summary>
        /// <param name="optionsAccessor">The options accessor.</param>
        public AuthenticSourcesOptionsAccessor(IOptions<AuthenticSourcesOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the authentic sources options.
        /// </summary>
        public AuthenticSourcesOptions Options { get; }

        #endregion
    }

    /// <summary>
    /// Manager for AuthenticSourcesOptions.
    /// </summary>
    public class AuthenticSourcesOptionsManager
    {
        #region Static Fields

        private static AuthenticSourcesOptionsManager current = null;

        #endregion

        #region Private Static Properties

        /// <summary>
        /// Gets the current HTTP context.
        /// </summary>
        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the current instance of the AuthenticSourcesOptionsManager.
        /// </summary>
        public static AuthenticSourcesOptionsManager Current
        {
            get
            {
                if (current == null)
                    current = new AuthenticSourcesOptionsManager();
                return current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the authentic sources options.
        /// </summary>
        /// <returns>The authentic sources options.</returns>
        public AuthenticSourcesOptions GetAuthenticSourcesOptions()
        {
            return HttpContext.RequestServices.GetRequiredService<AuthenticSourcesOptionsAccessor>().Options;
        }

        #endregion
    }
}