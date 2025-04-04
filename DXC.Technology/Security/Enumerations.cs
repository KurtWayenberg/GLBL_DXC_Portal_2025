using System;

namespace DXC.Technology.Security
{
    #region Enums

    /// <summary>
    /// Represents the status of a user.
    /// </summary>
    public enum UserStatusEnum
    {
        UNS_Unspecified = 0,
        REG_Registered = 1,
        ACT_Active = 2,
        LCK_Locked = 3,
        NACT_Inactive = 4,
    }


        /// <summary>
        /// List of operations a user can be granted on a certain task
        /// </summary>
        //aka [FlagsAttribute()]
        public enum OperationAuthorizationEnum
        {
            None = 0,
            Create = 1,
            Read = 2,
            Update = 4,
            Delete = 8,
            Administrate = 16,
            Search = 32,
            Print = 64,
            OverviewReport = 128,
            ManagementReport = 256,
            Import = 1024,
            Export = 2048,
            Execute = 4096,
            WorkOnBehalfOf = 8192,
            All = 16383,
            Any = 16383,
        }

    /// <summary>
    /// Standard combination of basic operations in meaningful terms.
    /// </summary>
    public enum BasicOperationAuthorizationEnum
    {
        None = 0,
        Read = 2,
        ReadAndUpdate = 6,
        CreateReadAndUpdate = 7,
        CreateReadUpdateAndDelete = 15,
        Administrate = 31
    }

    /// <summary>
    /// Standard combination of overview operations in meaningful terms.
    /// </summary>
    public enum OverviewOperationAuthorizationEnum
    {
        None = 0,
        Search = 32,
        Print = 64,
        SearchAndPrint = 96,
        SearchPrintAndOverviewReport = 224,
        SearchPrintAndManagementReport = 480
    }

    /// <summary>
    /// Standard combination of execution operations in meaningful terms.
    /// </summary>
    [Flags]
    public enum ExecuteAuthorizationEnum
    {
        None = 0,
        Import = 1024,
        Export = 2048,
        Execute = 4096,
        WorkOnBehalfOf = 8192,
        ImportAndExport = 3072,
        ImportExportAndExecute = 7168,
        ImportExportExecuteAndWorkOnBehalfOf = 15360,
    }

    /// <summary>
    /// Set of Secured Operations. Apart from the "CRUDSPOMIEAX-equivalents, a few deserve special attention:
    /// NotSpecified - the default
    /// None - This item is NOT secured - leave it untouched
    /// Standard - A trick to specify security for all textboxes, buttons, etc at once.
    /// This later gets translated to "Execute" for buttons, "Update" for Textboxes...
    /// </summary>
    public enum SecuredOperationEnum
    {
        NotSpecified = -1,
        Standard = -2,
        None = 0,
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8,
        Administrate = 16,
        Search = 32,
        Print = 64,
        OverviewReport = 128,
        ManagementReport = 256,
        Import = 1024,
        Export = 2048,
        Execute = 4096,
        WorkOnBehalfOf = 8192
    }

    /// <summary>
    /// Represents the type of security token.
    /// </summary>
    public enum SecurityTokenTypeEnum
    {
        NONE_None,

        P_Password,
        PH_PasswordHashed,

        T_Token,
        TH_TokenHashed,
        TD_TokenDate,
        TDH_TokenDateHashed,
        TK_TokenKey,
        TKH_TokenKeyHashed,

        //PDS_PasswordDateSymmetricKey,
        //PDAS_PasswordDateASymmetricKey,
        //PDASS_PasswordDateASymmetricKeySigned,

        PDTS_PasswordDateTimeSymmetricKey,
        //PDTAS_PasswordDateTimeASymmetricKey,
        //PDTASS_PasswordDateTimeASymmetricKeySigned
    }

    /// <summary>
    /// Represents actions a user can perform.
    /// </summary>
    public enum UserActionEnum
    {
        LogOn,
        LogOff,
        Activate,
        ReActivate,
        SendUserConfirmationToken,
        NewUserConfirmationToken,
        SessionTimeOut,
        ConfirmActiveBulletinBoard,
    }

    #endregion
}