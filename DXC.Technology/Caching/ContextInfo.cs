using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Caching
{
    #region Enums

    /// <summary>
    /// Specifies whether a certain ConnectionSetting is active or not
    /// </summary>
    public enum ConnectionEnum
    {
        Standard = 0,
        Standby = 1,
        Reporting = 2,
        Primary = 3
    }

    /// <summary>
    /// Enumeration dictating the (use case) transaction logging
    /// </summary>
    public enum TransactionDescriptionEnum
    {
        /// <summary>
        /// Log all the information (transaction id, database, isolation level)
        /// </summary>
        Full,

        /// <summary>
        /// Log only the transaction id
        /// </summary>
        IdOnly
    }

    #endregion

    #region Interfaces

    public interface IDbTransactionProvider
    {
        /// <summary>
        /// Gets a new database transaction based on the specified connection type.
        /// </summary>
        /// <param name="useCaseConnectionType">The connection type to use for the transaction.</param>
        /// <returns>A new database transaction.</returns>
        System.Data.IDbTransaction GetNewTransaction(ConnectionEnum useCaseConnectionType);
    }

    public interface IDbContextProvider
    {
        /// <summary>
        /// Gets a new Entity Framework database context.
        /// </summary>
        /// <returns>A new database context.</returns>
        DbContext GetNewDbContext();

        /// <summary>
        /// Executes a SQL client data adapter operation.
        /// </summary>
        /// <param name="dataSet">The dataset to use.</param>
        /// <param name="sqlDataAdapter">The SQL data adapter to use.</param>
        /// <param name="sqlParameters">The SQL parameters to use.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteSqlClientDataAdapter(System.Data.DataSet dataSet, System.Data.SqlClient.SqlDataAdapter sqlDataAdapter, params System.Data.SqlClient.SqlParameter[] sqlParameters);

        /// <summary>
        /// Executes a SQL client data adapter operation using a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="dataSet">The dataset to use.</param>
        /// <param name="sqlDataAdapter">The SQL data adapter to use.</param>
        /// <param name="sqlParameters">The SQL parameters to use.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteSqlClientDataAdapter(string connectionString, System.Data.DataSet dataSet, System.Data.SqlClient.SqlDataAdapter sqlDataAdapter, params System.Data.SqlClient.SqlParameter[] sqlParameters);

        /// <summary>
        /// Updates data rows using a SQL client data adapter.
        /// </summary>
        /// <param name="dataRows">The data rows to update.</param>
        /// <param name="sqlDataAdapter">The SQL data adapter to use.</param>
        /// <returns>The number of rows updated.</returns>
        int UpdateFromSqlClientDataAdapter(System.Data.DataRow[] dataRows, System.Data.SqlClient.SqlDataAdapter sqlDataAdapter);

        /// <summary>
        /// Updates data rows using a SQL client data adapter and a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="dataRows">The data rows to update.</param>
        /// <param name="sqlDataAdapter">The SQL data adapter to use.</param>
        /// <returns>The number of rows updated.</returns>
        int UpdateFromSqlClientDataAdapter(string connectionString, System.Data.DataRow[] dataRows, System.Data.SqlClient.SqlDataAdapter sqlDataAdapter);
    }

    #endregion

    public class TechnologyBasicContextInfo
    {
        #region Static Fields

        /// <summary>
        /// Security Manager Factory
        /// </summary>
        private static DXC.Technology.Security.ISecurityManagerFactory securityManagerFactory = null;

        #endregion

        #region Instance Fields

        /// <summary>
        /// User Name
        /// </summary>
        private string userName;

        /// <summary>
        /// API User Name
        /// </summary>
        private string apiUserName;

        /// <summary>
        /// Security Manager
        /// </summary>
        private DXC.Technology.Security.ISecurityManager securityManager;

        /// <summary>
        /// Information Owner
        /// </summary>
        private DXC.Technology.Security.InformationOwner informationOwner;

        /// <summary>
        /// Affinity
        /// </summary>
        private string affinity = "";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnologyBasicContextInfo"/> class.
        /// </summary>
        public TechnologyBasicContextInfo()
        {
            ContextCreationDateTime = DateTime.Now;
            CurrentCulture = new System.Globalization.CultureInfo("nl-be");
            CurrentUICulture = new System.Globalization.CultureInfo("nl-be");
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the Security Manager Factory.
        /// </summary>
        public static DXC.Technology.Security.ISecurityManagerFactory SecurityManagerFactory
        {
            get => securityManagerFactory;
            set => securityManagerFactory = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                ClearUserProfileRelatedCaches();
            }
        }

        /// <summary>
        /// Gets or sets the API user name.
        /// </summary>
        public string ApiUserName
        {
            get => apiUserName;
            set => apiUserName = value;
        }

        /// <summary>
        /// Gets or sets the context creation date and time.
        /// </summary>
        public DateTime ContextCreationDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the user network address.
        /// </summary>
        public string UserNetworkAddress { get; set; }

        /// <summary>
        /// Gets or sets the correlation GUID.
        /// </summary>
        public string CorrelationGuid { get; set; }

        /// <summary>
        /// Gets or sets the current UI culture.
        /// </summary>
        public System.Globalization.CultureInfo CurrentUICulture { get; set; }

        /// <summary>
        /// Gets or sets the current culture.
        /// </summary>
        public System.Globalization.CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Gets or sets the single sign-on security token.
        /// </summary>
        public string SingleSignOnSecurityToken { get; set; }

        /// <summary>
        /// Gets the localized application root URL.
        /// </summary>
        public string LocalizedApplicationRootUrl => string.Concat(DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationRootUrl, CurrentCulture.TwoLetterISOLanguageName, "/");

        /// <summary>
        /// Gets the unlocalized application root URL.
        /// </summary>
        public string UnlocalizedApplicationRootUrl => DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationRootUrl;

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears user profile-related caches.
        /// </summary>
        public void ClearUserProfileRelatedCaches()
        {
            securityManager = null;
            informationOwner = null;
            SingleSignOnSecurityToken = null;
            affinity = null;
        }

        /// <summary>
        /// Gets the security manager for the current user.
        /// </summary>
        /// <returns>The security manager.</returns>
        public DXC.Technology.Security.ISecurityManager GetSecurityManager()
        {
            if (securityManager == null && !string.IsNullOrEmpty(UserName))
            {
                if (SecurityManagerFactory == null)
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo Security Manager Factory");

                lock (DXC.Technology.Objects.KeyedStringLocks.GetKeyedLock(UserName))
                {
                    if (securityManager == null)
                        securityManager = SecurityManagerFactory.GetSecurityManager(UserName);
                }
            }
            return securityManager;
        }

        /// <summary>
        /// Gets the information owner for the current user.
        /// </summary>
        /// <returns>The information owner.</returns>
        public DXC.Technology.Security.InformationOwner GetInformationOwner()
        {
            if (informationOwner == null)
            {
                if (SecurityManagerFactory == null)
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo Security Manager Factory");

                informationOwner = SecurityManagerFactory.GetInformationOwner(UserName);
                securityManager = SecurityManagerFactory.GetSecurityManager(UserName);
            }
            return informationOwner;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the affinity for the current user.
        /// </summary>
        public string Affinity
        {
            get
            {
                if (string.IsNullOrEmpty(affinity))
                {
                    affinity = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().DefaultAffinity;
                    if (string.IsNullOrEmpty(affinity))
                    {
                        if (SecurityManagerFactory != null && !string.IsNullOrEmpty(UserName))
                        {
                            var sm = GetSecurityManager();
                            if (sm != null)
                            {
                                affinity = sm.GetUserSetting("Affinity");
                            }
                        }
                        if (string.IsNullOrEmpty(affinity))
                            affinity = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().DefaultAffinity;
                    }
                }
                return affinity;
            }
        }

        #endregion
    }

    public class TechnologyServiceContextInfo
    {
        #region Instance Fields

        /// <summary>
        /// Owner Stack
        /// </summary>
        private readonly System.Collections.Stack ownerStack = new System.Collections.Stack();

        /// <summary>
        /// Transaction
        /// </summary>
        private System.Data.IDbTransaction transaction;

        #endregion

        #region Static Fields

        /// <summary>
        /// Context Provider
        /// </summary>
        private static IDbContextProvider contextProvider;

        /// <summary>
        /// Transaction Provider
        /// </summary>
        private static IDbTransactionProvider transactionProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnologyServiceContextInfo"/> class.
        /// </summary>
        public TechnologyServiceContextInfo()
        {
            ServiceTimeStamp = DateTime.Now;
            ServiceGuid = Guid.NewGuid().ToString();
            CommitDisconnectedClient = false;
            IllegalAccessVerified = false;
            UseCaseConnectionType = ConnectionEnum.Standard;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the context provider.
        /// </summary>
        /// <returns>The context provider.</returns>
        public static IDbContextProvider GetContextProvider()
        {
            if (contextProvider == null)
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo Context Provider");
            return contextProvider;
        }

        /// <summary>
        /// Sets the context provider.
        /// </summary>
        /// <param name="value">The context provider to set.</param>
        public static void SetContextProvider(IDbContextProvider value)
        {
            contextProvider = value;
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the transaction provider.
        /// </summary>
        public static IDbTransactionProvider TransactionProvider
        {
            get => transactionProvider;
            set => transactionProvider = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the owner stack.
        /// </summary>
        public System.Collections.Stack OwnerStack => ownerStack;

        /// <summary>
        /// Gets or sets the service timestamp.
        /// </summary>
        public DateTime ServiceTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the service GUID.
        /// </summary>
        public string ServiceGuid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to commit disconnected clients.
        /// </summary>
        public bool CommitDisconnectedClient { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether illegal access has been verified.
        /// </summary>
        public bool IllegalAccessVerified { get; set; }

        /// <summary>
        /// Gets or sets the use case connection type.
        /// </summary>
        public ConnectionEnum UseCaseConnectionType { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the service context information description based on the specified mode.
        /// </summary>
        /// <param name="mode">The transaction description mode.</param>
        /// <returns>The service context information description.</returns>
        public string ServiceContextInfoDescription(TransactionDescriptionEnum mode)
        {
            if (mode == TransactionDescriptionEnum.Full)
            {
                using (var stringWriter = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default))
                {
                    stringWriter.Write("Usecase Depth:");
                    stringWriter.Write(ownerStack.Count);
                    stringWriter.Write(" ");
                    stringWriter.Write("Has data-adapter transaction:");
                    if (transaction != null)
                    {
                        stringWriter.Write(transaction.GetHashCode());
                        stringWriter.Write("(");
                        stringWriter.Write(transaction.Connection.Database);
                        stringWriter.Write(")");
                        stringWriter.Write(" Isolation:");
                        stringWriter.Write(transaction.IsolationLevel.ToString());
                    }
                    else
                        stringWriter.Write("---");
                    return stringWriter.ToString();
                }
            }
            else
            {
                return transaction != null ? transaction.GetHashCode().ToString(DXC.Technology.Utilities.StringFormatProvider.Default) : "0";
            }
        }

        /// <summary>
        /// Gets the transaction for the current context.
        /// </summary>
        /// <returns>The transaction.</returns>
        public System.Data.IDbTransaction GetTransaction()
        {
            if (transaction == null)
            {
                if (transactionProvider == null)
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo Transaction Provider");
                transaction = TransactionProvider.GetNewTransaction(UseCaseConnectionType);
            }

            return transaction;
        }

        /// <summary>
        /// Gets the database connection for the current transaction.
        /// </summary>
        /// <returns>The database connection.</returns>
        public System.Data.IDbConnection Connection()
        {
            if (transaction == null)
            {
                if (TransactionProvider == null)
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo Transaction Provider");
                transaction = TransactionProvider.GetNewTransaction(UseCaseConnectionType);
            }
            return transaction.Connection;
        }

        #endregion
    }
}