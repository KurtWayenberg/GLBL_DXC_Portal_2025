using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DXC.Technology.Security
{
    public class SecurityUserProfile : ISecurityManager
    {
        private UserInfo iUserInfo = new UserInfo();
        private System.Collections.Specialized.StringDictionary iUserSettings = new System.Collections.Specialized.StringDictionary();
        private string[] iUserRoles;
        private PermissionSet iUserPermissionSet = new PermissionSet();

        private InformationOwner iInformationOwner = new InformationOwner();
        private InformationOwner iInformationOwnerOwner = new InformationOwner();

        public UserInfo UserInfo
        {
            get { return iUserInfo; }
            set { iUserInfo = value; }
        }

        public string[] UserRoles
        {
            get { return iUserRoles; }
            set { iUserRoles = value; }
        }

        public System.Collections.Specialized.StringDictionary UserSettings
        {
            get { return iUserSettings; }
            set { iUserSettings = value; }
        }

        public PermissionSet UserPermissionSet
        {
            get { return iUserPermissionSet; }
            set { iUserPermissionSet = value; }
        }

        public InformationOwner InformationOwner
        {
            get
            {
                return iInformationOwner;
            }
            set
            {
                iInformationOwner = value;
            }
        }

        public InformationOwner InformationOwnerOwner
        {
            get
            {
                return iInformationOwnerOwner;
            }
            set
            {
                iInformationOwnerOwner = value;
            }
        }

        public string ActiveUser
        {
            get { return iUserInfo.UserName; }
        }

        public bool HasRole(string role)
        {
            return iUserRoles.Contains(role);
        }

        public List<string> GetRoles()
        {
            return iUserRoles.ToList();
        }

        public bool IsTaskAllowed(string task)
        {
            return this.UserPermissionSet.IsTaskAllowed(task);
        }

        public bool IsTaskAllowed(string task, OperationAuthorizationEnum operationAuthorization)
        {
            return this.UserPermissionSet.IsTaskAllowed(task, operationAuthorization);
        }

        public bool IsTaskAllowed(string task, string operationCode)
        {
            return this.UserPermissionSet.IsTaskAllowed(task, operationCode);
        }

        public string GetUserSetting(string settingName)
        {
            if (this.UserSettings.ContainsKey(settingName))
                return this.UserSettings[settingName];
            else
                return string.Empty;
        }
        public string SetUserSetting(string settingName, string value)
        {
            
            if (this.UserSettings.ContainsKey(settingName))
                return this.UserSettings[settingName];
            else
                return string.Empty;
        }


        public long? UserOrganization
        {
            get {
                string organizationAsString = this.UserSettings["ManagingOrganization"];
                if (!string.IsNullOrEmpty(organizationAsString))
                    return Convert.ToInt64(organizationAsString);
                return new long?();
            }
        }
    }


    public class UserInfo
    {
        #region Primitive Properties

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _userName;


        /// <summary>
        /// Department
        /// </summary>

        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        private string _department;


        /// <summary>
        /// LastName
        /// </summary>

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private string _lastName;

        /// <summary>
        /// FirstName
        /// </summary>

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _firstName;

        /// <summary>
        /// ExternalReferenceAd
        /// </summary>

        public string ExternalReferenceAd
        {
            get { return _externalReferenceAD; }
            set { _externalReferenceAD = value; }
        }

        private string _externalReferenceAD;

        /// <summary>
        /// ExternalReferenceCrm
        /// </summary>

        public string ExternalReferenceCrm
        {
            get { return _externalReferenceCRM; }
            set { _externalReferenceCRM = value; }
        }

        private string _externalReferenceCRM;



        /// <summary>
        /// CreationUser
        /// </summary>
        public string CreationUser
        {
            get { return _creationUser; }
            set { _creationUser = value; }
        }

        private string _creationUser;

        /// <summary>
        /// CreationDate
        /// </summary>

        public Nullable<System.DateTime> CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        private Nullable<System.DateTime> _creationDate;

        /// <summary>
        /// LastPasswordChangeDate
        /// </summary>

        public Nullable<System.DateTime> LastPasswordChangeDate
        {
            get { return _lastPasswordChangeDate; }
            set { _lastPasswordChangeDate = value; }
        }

        private Nullable<System.DateTime> _lastPasswordChangeDate;

        /// <summary>
        /// Street
        /// </summary>

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        private string _street;

        /// <summary>
        /// Housenumber
        /// </summary>

        public string Housenumber
        {
            get { return _housenumber; }
            set { _housenumber = value; }
        }

        private string _housenumber;

        /// <summary>
        /// Box
        /// </summary>

        public string Box
        {
            get { return _box; }
            set { _box = value; }
        }

        private string _box;

        /// <summary>
        /// Zip
        /// </summary>

        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }

        private string _zip;

        /// <summary>
        /// City
        /// </summary>

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _city;

        /// <summary>
        /// Community
        /// </summary>

        public string Community
        {
            get { return _community; }
            set { _community = value; }
        }

        private string _community;

        /// <summary>
        /// Country
        /// </summary>

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        private string _country;

        /// <summary>
        /// Email
        /// </summary>

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _email;

        /// <summary>
        /// Telephone
        /// </summary>

        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }

        private string _telephone;

        /// <summary>
        /// Mobile
        /// </summary>

        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        private string _mobile;



        public byte Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private byte _status;

        /// <summary>
        /// RowVersion
        /// </summary>

        public byte[] RowVersion
        {
            get { return _rowVersion; }
            set { _rowVersion = value; }
        }

        private byte[] _rowVersion;

        #endregion
    }


    [DataContract(IsReference = false)]
    [KnownType(typeof(PermissionSetTask))]
    public class PermissionSet
    {

        #region " Constants "
        public const string CreateOperation = "C_CREATE";
        public const string ReadOperation = "R_READ";
        public const string UpdateOperation = "U_UPDATE";
        public const string DeleteOperation = "D_DELETE";
        public const string AdministrateOperation = "A_ADMINISTRATE";
        public const string SearchOperation = "S_SEARCH";
        public const string PrintOperation = "P_PRINT";
        public const string OverviewReportOperation = "O_OVERVIEW";
        public const string ManagementReportOperation = "M_MANAGEMENT";
        public const string ExecuteOperation = "X_EXECUTE";
        public const string ExportOperation = "E_EXPORT";
        public const string ImportOperation = "I_IMPORT";
        public const string WorkOnBehalfOfOperation = "B_WorkOnBehalfOf";
        #endregion

        private ICollection<PermissionSetTask> iPermissionSetTasks = new List<PermissionSetTask>();
        private Dictionary<string, PermissionSetTask> iPermissionSetTaskCatalog = null;

        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, PermissionSetTask> PermissionSetTaskCatalog
        {
            get
            {
                if ((iPermissionSetTaskCatalog == null) && (iPermissionSetTasks != null))
                    iPermissionSetTaskCatalog = iPermissionSetTasks.ToDictionary(p => p.TaskName, p => p, StringComparer.OrdinalIgnoreCase);
                return iPermissionSetTaskCatalog;
            }
        }

        [DataMember]
        public ICollection<PermissionSetTask> PermissionSetTasks
        {
            get { return iPermissionSetTasks; }
        }

        /// <summary>
        /// Finds a task based on its logical name
        /// </summary>
        /// <param name="task">Logical name of the task to find</param>
        private PermissionSetTask GetTask(string taskName)
        {
            if (PermissionSetTaskCatalog.ContainsKey(taskName))
                return PermissionSetTaskCatalog[taskName];
            return null;
        }

        public void MergePermissionSet(PermissionSet pPermissionSet)
        {
            foreach (var lPermissionTaskToMerge in pPermissionSet.PermissionSetTasks)
            {
                if (!PermissionSetTaskCatalog.ContainsKey(lPermissionTaskToMerge.TaskName))
                {
                    PermissionSetTasks.Add(new PermissionSetTask(pPermissionSet.PermissionSetTaskCatalog[lPermissionTaskToMerge.TaskName]));
                    iPermissionSetTaskCatalog = null; //Clear catalog
                }
                else
                    PermissionSetTaskCatalog[lPermissionTaskToMerge.TaskName].MergePermissionTask(lPermissionTaskToMerge);
            }
        }


        public void AddPermission(string taskName, string pPermission)
        {
            if (!PermissionSetTaskCatalog.Keys.Contains(taskName))
            {
                PermissionSetTasks.Add(new PermissionSetTask(taskName, pPermission)); 
                iPermissionSetTaskCatalog = null; //Clear catalog
            }
            else
                PermissionSetTaskCatalog[taskName].Grant(pPermission);
        }


        public void RegisterRole(string roleName)
        {
            AddPermission("ROLE_" + roleName, PermissionSet.ExecuteOperation);
        }
        public bool HasRole(string roleName)
        {
            return IsTaskAllowed("ROLE_" + roleName, DXC.Technology.Security.OperationAuthorizationEnum.Execute);
        }

        public bool IsTaskAllowed(string task)
        {
            if (task.Contains(",") || task.Contains(";"))
            {
                // Check all values => 1 permission is enough
                foreach (string lTask in task.Split(new char[] { ',', ';' }))
                {
                    if (!string.IsNullOrEmpty(lTask) && this.IsTaskAllowed(lTask)) return true;
                }
                return false;
            }
            else
            {
                return this.prvIsTaskAllowed(task, "R");
            }
        }
        public bool IsTaskAllowed(string taskName, DXC.Technology.Security.OperationAuthorizationEnum operationAuthorization)
        {
            if (taskName.Contains(",") || taskName.Contains(";"))
            {
                // Check all values => 1 permission is enough
                foreach (string lTask in taskName.Split(new char[] { ',', ';' }))
                {
                    if (!string.IsNullOrEmpty(lTask) && this.prvIsTaskAllowed(lTask, operationAuthorization)) return true;
                }
                return false;
            }
            else
            {
                return this.prvIsTaskAllowed(taskName, operationAuthorization);
            }
        }

        public bool IsTaskAllowed(string taskName, string pOperationAuthorizationCode)
        {
            if (taskName.Contains(",") || taskName.Contains(";"))
            {
                // Check all values => 1 permission is enough
                foreach (string lTask in taskName.Split(new char[] { ',', ';' }))
                {
                    if (!string.IsNullOrEmpty(lTask) && this.prvIsTaskAllowed(lTask, pOperationAuthorizationCode)) return true;
                }
                return false;
            }
            else
            {
                return this.prvIsTaskAllowed(taskName, pOperationAuthorizationCode);
            }
        }
        public bool prvIsTaskAllowed(string taskName, DXC.Technology.Security.OperationAuthorizationEnum operationAuthorization)
        {
            return prvIsTaskAllowed(taskName, DXC.Technology.Security.PermissionSetTask.GetOperationAuthorizationCode(operationAuthorization));
        }

        public bool prvIsTaskAllowed(string taskName, string operationCode)
        {
            PermissionSetTask lTask = this.GetTask(taskName);

            // VB.NET return Not aTask Is null AndAlso aTask.IsAuthorized AndAlso aTask.IsAuthorized(OperationCode)
            bool lOK = (lTask == null ? false : lTask.IsAuthorized() == false ? false : lTask.IsAuthorized(operationCode) == false ? false : true);
            return lOK;
        }
    }

    /// <remarks>
    /// A task is the low-granularity levelconcept which programmers use to program their interfaces for. 
    /// For each distinct set of windows and/or groups of items. A task grouping these elements can be defined.
    /// Te enablement of these e.g. GUI elements is then dependent on the tasks set and allowed task
    /// permissions for this user
    /// </remarks>
    [Serializable()]
    public sealed class PermissionSetTask
    {
        #region Declarations
        private string iTask = string.Empty;
        private OperationAuthorizationEnum iOperationAuthorization = OperationAuthorizationEnum.None;

        // TODO : Correct (extra) state + mapping to class behavior ??
        private BasicOperationAuthorizationEnum iBasicOperationAuthorization = BasicOperationAuthorizationEnum.None;
        private ExecuteAuthorizationEnum iExecuteAuthorization = ExecuteAuthorizationEnum.None;
        private OverviewOperationAuthorizationEnum iOverviewOperationAuthorizationEnum = OverviewOperationAuthorizationEnum.None;
        #endregion

        #region Constants
        public const string CreateOperation = "C";
        public const string ReadOperation = "R";
        public const string UpdateOperation = "U";
        public const string DeleteOperation = "D";
        public const string SearchOperation = "S";
        public const string PrintOperation = "P";
        public const string OverviewReportOperation = "O";
        public const string ManagementReportOperation = "M";
        public const string ExportOperation = "E";
        public const string ImportOperation = "I";
        public const string AdministrateOperation = "A";
        public const string WorkOnBehalfOfOperation = "B";

        public const string ExampleAuthorizationString1 = "CRUDASPOMEIX";
        public const string ExampleAuthorizationString2 = "RUPE";


        #endregion

        #region Constructors

        /// <summary>
        /// copies
        /// </summary>
        /// <param name="pPermissionSetTask"></param>
        public PermissionSetTask(PermissionSetTask pPermissionSetTask)
        {
            this.iTask = pPermissionSetTask.TaskName;
            this.iOperationAuthorization = pPermissionSetTask.iOperationAuthorization;
        }

        public void MergePermissionTask(PermissionSetTask pPermissionSetTask)
        {
            this.iOperationAuthorization = iOperationAuthorization | pPermissionSetTask.iOperationAuthorization;
        }
        /// <summary>
        /// Constructor. Create a task of a certain name and specify its operations on Basic, Overview and Execution level
        /// </summary>
        /// <param name="task">Logical name of a task</param>
        /// <param name="pBasicAuthorization">Standard combination of basic operations in meaningfull terms</param>
        /// <param name="pOverviewAuthorization">Standard combination of Overview operations in meaningfull terms</param>
        /// <param name="pExecuteAuthorization">Standard combination of Execute operations in meaningfull terms</param>
        public PermissionSetTask(string task,
            BasicOperationAuthorizationEnum pBasicAuthorization,
            OverviewOperationAuthorizationEnum pOverviewAuthorization,
            ExecuteAuthorizationEnum pExecuteAuthorization)
        {
            this.ConstructorImplementation(task,
                pBasicAuthorization,
                pOverviewAuthorization,
                pExecuteAuthorization);
        }

        /// <summary>
        /// Constructor. Create a task of a certain name and specify its operations on Basic and Overview level
        /// </summary>
        /// <param name="task">Logical name of a task</param>
        /// <param name="pBasicAuthorization">Standard combination of basic operations in meaningfull terms</param>
        /// <param name="pOverviewAuthorization">Standard combination of Overview operations in meaningfull terms</param>
        public PermissionSetTask(string task,
            BasicOperationAuthorizationEnum pBasicAuthorization,
            OverviewOperationAuthorizationEnum pOverviewAuthorization)
        {
            this.ConstructorImplementation(task,
                pBasicAuthorization,
                pOverviewAuthorization,
                ExecuteAuthorizationEnum.None);
        }

        /// <summary>
        /// Constructor. Create a task of a certain name and specify its operations on Basic level
        /// </summary>
        /// <param name="task">Logical name of a task</param>
        /// <param name="pBasicAuthorization">Standard combination of basic operations in meaningfull terms</param>
        public PermissionSetTask(string task,
            BasicOperationAuthorizationEnum pBasicAuthorization)
        {
            this.ConstructorImplementation(task,
                pBasicAuthorization,
                OverviewOperationAuthorizationEnum.None,
                ExecuteAuthorizationEnum.None);
        }

        /// <summary>
        /// Constructor. Create a task of a certain name and specify its operations on Basic, Overview and Execution level
        /// </summary>
        /// <param name="task">Logical name of a task</param>
        /// <param name="pAuthorization">Standard combination of operations in meaningfull terms</param>
        public PermissionSetTask(string task, OperationAuthorizationEnum pAuthorization)
        {
            this.iTask = task;
            this.Grant(pAuthorization);
        }

        /// <summary>
        /// Constructor. Create a task of a certain name and specify its allowed operations using an Authorization string
        /// </summary>
        /// <param name="task">Logical name of a task</param>
        /// <param name="pGrant">Authorization string- A selection of characters from "CRUDSPOMIEAX"</param>
        public PermissionSetTask(string task, string pGrant)
        {
            this.iTask = task;
            this.Grant(pGrant);
        }

        /// <summary>
        /// Constructor Implementation. Create a task of a certain name and specify its operations on Basic, Overview and Execution level
        /// </summary>
        /// <param name="task">Logical name of a task</param>
        /// <param name="pBasicAuthorization">Standard combination of basic operations in meaningfull terms</param>
        /// <param name="pOverviewAuthorization">Standard combination of Overview operations in meaningfull terms</param>
        /// <param name="pExecuteAuthorization">Standard combination of Execute operations in meaningfull terms</param>
        private void ConstructorImplementation(string task,
            BasicOperationAuthorizationEnum pBasicAuthorization,
            OverviewOperationAuthorizationEnum pOverviewAuthorization,
            ExecuteAuthorizationEnum pExecuteAuthorization)
        {
            this.iTask = task;
            this.Grant(pBasicAuthorization);
            this.Grant(pOverviewAuthorization);
            this.Grant(pExecuteAuthorization);
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Returns the logical name of the task
        /// </summary>
        public string TaskName
        {
            get
            {
                return this.iTask;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Grants an Operation to a Task
        /// </summary>
        /// <param name="pAuthorization">Operation to add</param>
        public void Grant(OperationAuthorizationEnum pAuthorization)
        {
            this.iOperationAuthorization |= pAuthorization;
        }

        /// <summary>
        /// Grants a basic Operation (set) to a Task
        /// </summary>
        /// <param name="pBasicAuthorization">Basic Authorization To add</param>
        public void Grant(BasicOperationAuthorizationEnum pBasicAuthorization)
        {
            this.iBasicOperationAuthorization |= pBasicAuthorization;
        }

        /// <summary>
        /// Grants an Overview Operation (set) to a Task
        /// </summary>
        /// <param name="pOverviewAuthorization">Overview Authorization To add</param>
        public void Grant(OverviewOperationAuthorizationEnum pOverviewAuthorization)
        {
            this.iOverviewOperationAuthorizationEnum |= pOverviewAuthorization;
        }

        /// <summary>
        /// Grants an Execute Operation (set) to a Task
        /// </summary>
        /// <param name="pExecuteAuthorization">Execute Authorization To add</param>
        public void Grant(ExecuteAuthorizationEnum pExecuteAuthorization)
        {
            this.iExecuteAuthorization |= pExecuteAuthorization;
        }

        /// <summary>
        /// Grant all operations implied by an Authorization string
        /// </summary>
        /// <param name="pGrant">Authorization string- A selection of characters from "CRUDSPOMIEAX"</param>
        public void Grant(string pGrant)
        {
            if (string.IsNullOrEmpty(pGrant)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("GrantString");

            foreach (Char lGrantCode in pGrant)
            {
                this.iOperationAuthorization |= GetOperationAuthorization(lGrantCode);
            }
        }

        /// <summary>
        /// Grants all rights to this taks
        /// </summary>
        public void GrantAll()
        {
            this.iOperationAuthorization = OperationAuthorizationEnum.All;
        }

        /// <summary>
        /// Revokes an Operation to a Task
        /// </summary>
        /// <param name="pAuthorization">Operation to revoke</param>
        public void Revoke(OperationAuthorizationEnum pAuthorization)
        {
            this.iOperationAuthorization ^= (this.iOperationAuthorization & pAuthorization);
        }

        /// <summary>
        /// Revokes a Basic Operation From a Task
        /// </summary>
        /// <param name="pBasicAuthorization">Operation to revoke</param>
        public void Revoke(BasicOperationAuthorizationEnum pBasicAuthorization)
        {
            this.iBasicOperationAuthorization ^= (this.iBasicOperationAuthorization & pBasicAuthorization);
        }

        /// <summary>
        /// Revokes an Overview Operation From a Task
        /// </summary>
        /// <param name="pOverviewAuthorization">Operation to revoke</param>
        public void Revoke(OverviewOperationAuthorizationEnum pOverviewAuthorization)
        {
            this.iOverviewOperationAuthorizationEnum ^= (this.iOverviewOperationAuthorizationEnum & pOverviewAuthorization);
        }

        /// <summary>
        /// Revokes an Execute Operation From a Task
        /// </summary>
        /// <param name="pExecuteAuthorization">Operation to revoke</param>
        public void Revoke(ExecuteAuthorizationEnum pExecuteAuthorization)
        {
            this.iExecuteAuthorization ^= (this.iExecuteAuthorization & pExecuteAuthorization);
        }

        /// <summary>
        /// Revokes a "CRUDSPOMIEAX"-substring defined set of operations from a Task
        /// </summary>
        /// <param name="pRevoke">"CRUDSPOMIEAX"-substring defined set of operations</param>
        public void Revoke(string pRevoke)
        {
            if (string.IsNullOrEmpty(pRevoke)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("RevokeString");
            foreach (Char lRevokeCode in pRevoke)
            {
                OperationAuthorizationEnum lOperationAuthorization = GetOperationAuthorization(lRevokeCode);

                if ((this.iOperationAuthorization & lOperationAuthorization) == lOperationAuthorization)
                {
                    this.iOperationAuthorization ^= lOperationAuthorization;
                }
            }
        }

        /// <summary>
        /// Revoke all rights from this task
        /// </summary>
        public void RevokeAll()
        {
            this.iOperationAuthorization = OperationAuthorizationEnum.None;
        }

        /// <summary>
        /// Verifies if a task authorizes a certain [set of] operation[s]
        /// </summary>
        /// <param name="pAuthorization">[set of] operation[s]</param>
        /// <returns></returns>
        public bool IsAuthorized(OperationAuthorizationEnum pAuthorization)
        {
            return DXC.Technology.Enumerations.EnumerationHelper.IncludesFlag(this.iOperationAuthorization, pAuthorization);
        }

        /// <summary>
        /// Verifies if a task authorizes a certain [set of] basic operation[s]
        /// </summary>
        /// <param name="pBasicAuthorization">[set of] basic operation[s]</param>
        /// <returns></returns>
        public bool IsAuthorized(BasicOperationAuthorizationEnum pBasicAuthorization)
        {
            return ((this.iBasicOperationAuthorization & pBasicAuthorization) == pBasicAuthorization);
        }

        /// <summary>
        /// Verifies if a task authorizes a certain [set of] overview operation[s]
        /// </summary>
        /// <param name="pOverviewAuthorization">[set of] overview operation[s]</param>
        /// <returns></returns>
        public bool IsAuthorized(OverviewOperationAuthorizationEnum pOverviewAuthorization)
        {
            return (this.iOverviewOperationAuthorizationEnum & pOverviewAuthorization) == pOverviewAuthorization;
        }

        /// <summary>
        /// Verifies if a task authorizes a certain [set of] execute operation[s]
        /// </summary>
        /// <param name="pExecuteAuthorization">[set of] execute operation[s]</param>
        /// <returns></returns>
        public bool IsAuthorized(ExecuteAuthorizationEnum pExecuteAuthorization)
        {
            return (this.iExecuteAuthorization & pExecuteAuthorization) == pExecuteAuthorization ? true : false;
        }

        /// <summary>
        /// Verifies if a task authorizes at least one operation
        /// </summary>
        public bool IsAuthorized()
        {
            return this.iOperationAuthorization != OperationAuthorizationEnum.None;
        }

        /// <summary>
        /// Verifies if a task authorizes a certain [set of] execute operation[s] as specified by a "CRUDSPOMIEAX" substring
        /// </summary>
        /// <param name="pAuthorization">[set of] execute operation[s] specified by a "CRUDSPOMIEAX" substring</param>
        /// <returns></returns>
        public bool IsAuthorized(string pAuthorization)
        {
            if (string.IsNullOrEmpty(pAuthorization)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("AuthorizationString");
            OperationAuthorizationEnum lRequiredOperationAuthorization = OperationAuthorizationEnum.None;

            foreach (Char lAuthorizationCode in pAuthorization)
            {
                lRequiredOperationAuthorization |= GetOperationAuthorization(lAuthorizationCode);
            }
            return (this.iOperationAuthorization & lRequiredOperationAuthorization) == lRequiredOperationAuthorization;
        }
        #endregion


        #region Public Conversions
        /// <summary>
        /// Converts a "CRUDSPOMIEAX" operation authorization character to its corresponding OperationAuthorizationEnum
        /// </summary>
        /// <param name="pAuthorizationCode">q "CRUDSPOMIEAX" operation authorization character to convert</param>
        /// <returns></returns>
        public static OperationAuthorizationEnum GetOperationAuthorization(Char pAuthorizationCode)
        {
            switch (pAuthorizationCode)
            {
                case 'C':
                    return OperationAuthorizationEnum.Create;

                case 'R':
                    return OperationAuthorizationEnum.Read;

                case 'U':
                    return OperationAuthorizationEnum.Update;

                case 'D':
                    return OperationAuthorizationEnum.Delete;

                case 'A':
                    return OperationAuthorizationEnum.Administrate;

                case 'B':
                    return OperationAuthorizationEnum.WorkOnBehalfOf;

                case 'S':
                    return OperationAuthorizationEnum.Search;

                case 'P':
                    return OperationAuthorizationEnum.Print;

                case 'O':
                    return OperationAuthorizationEnum.OverviewReport;

                case 'M':
                    return OperationAuthorizationEnum.ManagementReport;

                case 'I':
                    return OperationAuthorizationEnum.Import;

                case 'E':
                    return OperationAuthorizationEnum.Export;

                case 'X':
                    return OperationAuthorizationEnum.Execute;

                default:
                    return OperationAuthorizationEnum.None;
            }
        }

        /// <summary>
        /// Converts a "CRUDSPOMIEAX" operation authorization character to its corresponding OperationAuthorizationEnum
        /// </summary>
        /// <param name="pAuthorizationCode">q "CRUDSPOMIEAX" operation authorization character to convert</param>
        /// <returns></returns>
        public static string GetOperationAuthorizationCode(OperationAuthorizationEnum pAuthorizationCode)
        {
            switch (pAuthorizationCode)
            {
                case OperationAuthorizationEnum.Create:
                    return "C";

                case OperationAuthorizationEnum.Read:
                    return "R";

                case OperationAuthorizationEnum.Update:
                    return "U";

                case OperationAuthorizationEnum.Delete:
                    return "D";

                case OperationAuthorizationEnum.Administrate:
                    return "A";

                case OperationAuthorizationEnum.WorkOnBehalfOf:
                    return "B";

                case OperationAuthorizationEnum.Search:
                    return "S";

                case OperationAuthorizationEnum.Print:
                    return "P";

                case OperationAuthorizationEnum.OverviewReport:
                    return "O";

                case OperationAuthorizationEnum.ManagementReport:
                    return "M";

                case OperationAuthorizationEnum.Import:
                    return "I";

                case OperationAuthorizationEnum.Export:
                    return "E";

                case OperationAuthorizationEnum.Execute:
                    return "X";

                default:
                    return "";
            }
        }
        #endregion
    }




}
