using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace DXC.Technology.Translations
{
    public interface ITranslationProvider
    {
        /// <summary>
        /// Localizes a string identifier to its corresponding translation.
        /// </summary>
        /// <param name="identifier">The string identifier to localize.</param>
        /// <returns>The localized string.</returns>
        string Localize(string identifier);

        /// <summary>
        /// Localizes an exception to its corresponding translation.
        /// </summary>
        /// <param name="identifier">The exception to localize.</param>
        /// <returns>The localized string representation of the exception.</returns>
        string Localize(Exception identifier);
    }

    public interface ILocalizable
    {
        /// <summary>
        /// Localizes the object using the provided translation provider.
        /// </summary>
        /// <param name="translationProvider">The translation provider to use for localization.</param>
        void Localize(ITranslationProvider translationProvider);
    }

    /// <summary>
    /// Use the translation manager to do your translations.
    /// </summary>
    public class TranslationManager : ITranslationProvider
    {
        #region Static Fields

        /// <summary>
        /// The current translation provider instance.
        /// </summary>
        private static ITranslationProvider current = null;

        #endregion

        #region Instance Fields

        /// <summary>
        /// The resource manager used for localization.
        /// </summary>
        private readonly ResourceManager localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationManager"/> class.
        /// </summary>
        /// <param name="factory">The string localizer factory.</param>
        public TranslationManager(IStringLocalizerFactory factory)
        {
            var resourceBaseName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationResourceBaseName;
            localizer = new ResourceManager(resourceBaseName, Assembly.GetEntryAssembly());
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the current translation provider.
        /// </summary>
        public static ITranslationProvider Current
        {
            get
            {
                if (current == null)
                {
                    current = DXC.Technology.Caching.ContextHelper.GetTranslationProvider();
                }
                return current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Given an identifying text string, gets the translated text. If no text is found, returns the identifying text string as a best guess.
        /// </summary>
        /// <param name="identifier">An identifying text string.</param>
        /// <returns>The localized string.</returns>
        public string Localize(string identifier)
        {
            if (string.IsNullOrEmpty(identifier)) return string.Empty;

            if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.Equals("az"))
                return "+++++++++++";

            string result = localizer.GetString(identifier);
            if (result == null) result = identifier;

            if (!Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.Equals("en"))
            {
                bool missingTranslation = result.Equals(identifier);
                if (missingTranslation)
                {
                    string alternativeIdentifier = identifier.Replace(" :", "");
                    result = localizer.GetString(alternativeIdentifier);
                    if (result == null) result = alternativeIdentifier;
                    missingTranslation = result.Equals(alternativeIdentifier);
                    result += " :";
                }
                if (missingTranslation)
                {
                    string alternativeIdentifier = identifier.Replace(":", "");
                    result = localizer.GetString(alternativeIdentifier);
                    if (result == null) result = alternativeIdentifier;
                    missingTranslation = result.Equals(alternativeIdentifier);
                    result += ":";
                }
                if (missingTranslation)
                {
                    string alternativeIdentifier = identifier + ":";
                    result = localizer.GetString(alternativeIdentifier);
                    if (result == null) result = alternativeIdentifier;
                    missingTranslation = result.Equals(alternativeIdentifier);
                }
                if (missingTranslation)
                {
                    string alternativeIdentifier = identifier + " :";
                    result = localizer.GetString(alternativeIdentifier);
                    if (result == null) result = alternativeIdentifier;
                    missingTranslation = result.Equals(alternativeIdentifier);
                }

                if (missingTranslation)
                {
                    result = identifier;
                    RegisterMissingTranslation(identifier);
                    if (DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationModeAsEnum
                        == DXC.Technology.Configuration.ApplicationModeEnum.ShowUntranslated)
                        return "********";
                }
            }

            result = result.Replace("|", "<br/>");
            return result;
        }

        /// <summary>
        /// Localizes an exception to its corresponding translation.
        /// </summary>
        /// <param name="exception">The exception to localize.</param>
        /// <returns>The localized string representation of the exception.</returns>
        public string Localize(Exception exception)
        {
            if (exception == null) return "";
            if (exception is DXC.Technology.Exceptions.ApplicationExceptionBase applicationExceptionBase)
            {
                DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException brokenRulesException =
                    applicationExceptionBase as DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException;

                if (brokenRulesException != null)
                {
                    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                    foreach (Exception ex in brokenRulesException.BrokenRulesExceptions)
                    {
                        DXC.Technology.Exceptions.ApplicationExceptionBase appException = ex as DXC.Technology.Exceptions.ApplicationExceptionBase;
                        if (appException != null)
                        {
                            string[] arguments = appException.ExceptionArgumentsAsCSV.Split(',', ';');
                            for (int i = 0; i <= arguments.GetUpperBound(0); i++)
                            {
                                arguments[i] = this.Localize(arguments[i]);
                            }
                            stringWriter.WriteLine(this.Localize(string.Format(this.Localize(appException.ExceptionPatternString()), arguments)));
                        }
                        else
                        {
                            stringWriter.WriteLine(ex.Message);
                        }
                    }
                    return stringWriter.ToString();
                }
                else
                {
                    string[] arguments = applicationExceptionBase.ExceptionArgumentsAsCSV.Split(',', ';');
                    for (int i = 0; i <= arguments.GetUpperBound(0); i++)
                    {
                        arguments[i] = this.Localize(arguments[i]);
                    }
                    return this.Localize(string.Format(this.Localize(applicationExceptionBase.ExceptionPatternString()), arguments));
                }
            }
            else
                return this.Localize(exception.Message);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers a missing translation for logging or other purposes.
        /// </summary>
        /// <param name="identifier">The identifier of the missing translation.</param>
        private void RegisterMissingTranslation(string identifier)
        {
            if (DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationModeAsEnum
                == DXC.Technology.Configuration.ApplicationModeEnum.LogUntranslated)
            {
                DXC.Technology.Configuration.LoggingManager.Current.Log(
                    Microsoft.Extensions.Logging.LogLevel.Information,
                    new Microsoft.Extensions.Logging.EventId(90001, "Untranslated" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName),
                    identifier);
            }
        }

        #endregion
    }

}