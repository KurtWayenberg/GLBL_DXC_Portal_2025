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

namespace DXC.Technology.Logging
{
    #region TechnologyFileLoggerOptions

    /// <summary>
    /// Represents options for the Technology File Logger.
    /// </summary>
    public class TechnologyFileLoggerOptions
    {
        /// <summary>
        /// Gets or sets the file path for the logger.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the folder path for the logger.
        /// </summary>
        public string FolderPath { get; set; }
    }

    #endregion

    #region TechnologyFileLoggerProvider

    [ProviderAlias("TechnologyFile")]
    public class TechnologyFileLoggerProvider : ILoggerProvider
    {
        #region Instance Fields

        /// <summary>
        /// Options for the Technology File Logger.
        /// </summary>
        public TechnologyFileLoggerOptions Options { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnologyFileLoggerProvider"/> class.
        /// </summary>
        /// <param name="options">The options for the logger provider.</param>
        public TechnologyFileLoggerProvider(IOptions<TechnologyFileLoggerOptions> options)
        {
            Options = options.Value;

            if (!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a logger for the specified category name.
        /// </summary>
        /// <param name="categoryName">The category name for the logger.</param>
        /// <returns>An instance of <see cref="TechnologyFileLogger"/>.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new TechnologyFileLogger(this);
        }

        /// <summary>
        /// Disposes the logger provider.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }

    #endregion
}
