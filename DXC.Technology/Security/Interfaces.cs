using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Security
{
    #region Interfaces

    /// <summary>
    /// Interface for managing security operations.
    /// </summary>
    public interface ISecurityManager
    {
        #region Public Properties

        /// <summary>
        /// Gets the active user.
        /// </summary>
        string ActiveUser { get; }

        /// <summary>
        /// Gets the user's organization.
        /// </summary>
        long? UserOrganization { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the user has a specific role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the user has the role, otherwise false.</returns>
        bool HasRole(string role);

        /// <summary>
        /// Checks if a task is allowed.
        /// </summary>
        /// <param name="task">The task to check.</param>
        /// <returns>True if the task is allowed, otherwise false.</returns>
        bool IsTaskAllowed(string task);

        /// <summary>
        /// Checks if a task is allowed with a specific operation authorization.
        /// </summary>
        /// <param name="task">The task to check.</param>
        /// <param name="operationAuthorization">The operation authorization to check.</param>
        /// <returns>True if the task is allowed, otherwise false.</returns>
        bool IsTaskAllowed(string task, OperationAuthorizationEnum operationAuthorization);

        /// <summary>
        /// Checks if a task is allowed with a specific operation code.
        /// </summary>
        /// <param name="task">The task to check.</param>
        /// <param name="operationCode">The operation code to check.</param>
        /// <returns>True if the task is allowed, otherwise false.</returns>
        bool IsTaskAllowed(string task, string operationCode);

        /// <summary>
        /// Gets a user setting by name.
        /// </summary>
        /// <param name="settingName">The name of the setting.</param>
        /// <returns>The value of the setting.</returns>
        string GetUserSetting(string settingName);

        /// <summary>
        /// Gets the roles of the user.
        /// </summary>
        /// <returns>A list of roles.</returns>
        List<string> GetRoles();

        #endregion
    }

    /// <summary>
    /// Factory interface for creating and managing security managers.
    /// </summary>
    public interface ISecurityManagerFactory
    {
        #region Public Methods

        /// <summary>
        /// Removes the security manager from the cache for a specific user.
        /// </summary>
        /// <param name="userName">The username to remove from the cache.</param>
        void RemoveSecurityManagerFromCache(string userName);

        /// <summary>
        /// Gets the security manager for a specific user.
        /// </summary>
        /// <param name="userName">The username to get the security manager for.</param>
        /// <returns>The security manager for the user.</returns>
        ISecurityManager GetSecurityManager(string userName);

        /// <summary>
        /// Gets the information owner for a specific user.
        /// </summary>
        /// <param name="userName">The username to get the information owner for.</param>
        /// <returns>The information owner for the user.</returns>
        InformationOwner GetInformationOwner(string userName);

        #endregion
    }

    #endregion

    #region Classes

    /// <summary>
    /// Represents an information owner.
    /// </summary>
    public class InformationOwner
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ID of the information owner.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the code of the information owner.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the reference number of the information owner.
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the information owner.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language of the information owner.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the type of the information owner.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the country of the information owner.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the party name of the information owner.
        /// </summary>
        public string PartyName { get; set; }

        #endregion
    }

    #endregion
}