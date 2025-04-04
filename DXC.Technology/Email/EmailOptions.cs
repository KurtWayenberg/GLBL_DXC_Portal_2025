using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DXC.Technology.Email
{
    /// <summary>
    /// Represents configuration options for the email provider.
    /// </summary>
    public class TechnologyEmailOptions
    {
        #region Public Properties

        /// <summary>
        /// The email provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// The user associated with the email provider.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The API key for the email provider.
        /// </summary>
        public string ApiKey { get; set; }

        #endregion
    }
}