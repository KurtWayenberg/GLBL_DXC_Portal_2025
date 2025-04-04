using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Security
{
    public class SecurityManager
    {
        #region Static Fields

        /// <summary>
        /// Represents the security task for a user.
        /// </summary>
        public const string SecurityTask = "USER";

        /// <summary>
        /// Represents the system account.
        /// </summary>
        public const string SystemAccount = "UAADM";

        /// <summary>
        /// Represents the guest account.
        /// </summary>
        public const string GuestAccount = "GUEST";

        /// <summary>
        /// Represents the service account.
        /// </summary>
        public const string ServiceAccount = "IUSR";

        /// <summary>
        /// Represents the API account.
        /// </summary>
        public const string ApiAccount = "UA4API";

        /// <summary>
        /// Represents the ASP.NET v4.0 account.
        /// </summary>
        public const string AspNet40Account = "ASP.NET v4.0";

        /// <summary>
        /// Represents the information owner user setting.
        /// </summary>
        public const string InformationOwnerUserSetting = "INF_OWN";

        #endregion
    }

    public class DummySecurityManager : ISecurityManager
    {
        #region Private Properties

        /// <summary>
        /// Stores the username of the active user.
        /// </summary>
        private string userName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DummySecurityManager"/> class.
        /// </summary>
        /// <param name="userName">The username of the active user.</param>
        public DummySecurityManager(string userName)
        {
            this.userName = userName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the active user.
        /// </summary>
        public string ActiveUser => userName;

        /// <summary>
        /// Gets the user's organization.
        /// </summary>
        public long? UserOrganization => 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the roles associated with the user.
        /// </summary>
        /// <returns>A list of roles associated with the user.</returns>
        public List<string> GetRoles()
        {
            return new List<string>();
        }

        /// <summary>
        /// Retrieves a user setting based on the setting name.
        /// </summary>
        /// <param name="settingName">The name of the setting to retrieve.</param>
        /// <returns>The value of the user setting.</returns>
        public string GetUserSetting(string settingName)
        {
            if (settingName == "DefaultParty")
                return userName;
            return "";
        }

        /// <summary>
        /// Checks if the user has a specific role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns><c>true</c> if the user has the specified role; otherwise, <c>false</c>.</returns>
        public bool HasRole(string role)
        {
            return role == "CTZN";
        }

        /// <summary>
        /// Checks if a specific task is allowed for the user.
        /// </summary>
        /// <param name="task">The task to check.</param>
        /// <returns><c>true</c> if the task is allowed; otherwise, <c>false</c>.</returns>
        public bool IsTaskAllowed(string task)
        {
            return false;
        }

        /// <summary>
        /// Checks if a specific task is allowed for the user with a specific operation authorization.
        /// </summary>
        /// <param name="task">The task to check.</param>
        /// <param name="operationAuthorization">The operation authorization to check.</param>
        /// <returns><c>true</c> if the task is allowed; otherwise, <c>false</c>.</returns>
        public bool IsTaskAllowed(string task, OperationAuthorizationEnum operationAuthorization)
        {
            return false;
        }

        /// <summary>
        /// Checks if a specific task is allowed for the user with a specific operation code.
        /// </summary>
        /// <param name="task">The task to check.</param>
        /// <param name="operationCode">The operation code to check.</param>
        /// <returns><c>true</c> if the task is allowed; otherwise, <c>false</c>.</returns>
        public bool IsTaskAllowed(string task, string operationCode)
        {
            return false;
        }

        #endregion
    }
}