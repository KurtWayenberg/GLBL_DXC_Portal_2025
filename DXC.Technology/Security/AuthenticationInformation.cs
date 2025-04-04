using System;

namespace DXC.Technology.Security
{
    public class AuthenticationInformation
    {
        #region Public Properties

        /// <summary>
        /// The date and time of the request.
        /// </summary>
        public DateTime RequestDateTime { get; set; }

        /// <summary>
        /// The client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The redirect URI.
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// The response type.
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// The scope of the request.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// The state parameter.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The nonce value.
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// The username.
        /// </summary>
        public string UserName { get; set; }

        #endregion
    }
}