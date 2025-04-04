using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DXC.Technology.Email
{
    /// <summary>
    /// Provides functionality to send emails using SendGrid.
    /// </summary>
    public class EmailSender
    {
        #region Instance Fields

        /// <summary>
        /// Options for configuring the email sender, set only via Secret Manager.
        /// </summary>
        public TechnologyEmailOptions Options { get; init; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for EmailSender.
        /// </summary>
        /// <param name="optionsAccessor">Options accessor for email configuration.</param>
        public EmailSender(IOptions<TechnologyEmailOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="email">Recipient email address.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="message">Message content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.ApiKey, email, subject, message);
        }

        /// <summary>
        /// Executes the email sending process.
        /// </summary>
        /// <param name="apiKey">API key for SendGrid.</param>
        /// <param name="email">Recipient email address.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="message">Message content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task Execute(string apiKey, string email, string subject, string message)
        {
            string[] emailParts = email.Split('<', '>', '(', ')');
            string username = null;
            string emailAddress = email;

            if (emailParts.Length >= 2)
            {
                username = emailParts[1];
                emailAddress = emailParts[2];
            }

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(Options.User, username),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(emailAddress));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }

        #endregion
    }
}