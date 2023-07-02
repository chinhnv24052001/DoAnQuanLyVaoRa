using System;

namespace tmss
{
    /// <summary>
    /// Some consts used in the application.
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// Default page size for paged requests.
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// Maximum allowed page size for paged requests.
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";

        public const int ResizedMaxProfilPictureBytesUserFriendlyValue = 1024;

        public const int MaxProfilPictureBytesUserFriendlyValue = 5;

        public const string TokenValidityKey = "token_validity_key";
        public const string RefreshTokenValidityKey = "refresh_token_validity_key";
        public const string SecurityStampKey = "AspNet.Identity.SecurityStamp";

        public const string TokenType = "token_type";

        public static string UserIdentifier = "user_identifier";

        public const string ThemeDefault = "default";
        public const string Theme2 = "theme2";
        public const string Theme3 = "theme3";
        public const string Theme4 = "theme4";
        public const string Theme5 = "theme5";
        public const string Theme6 = "theme6";
        public const string Theme7 = "theme7";
        public const string Theme8 = "theme8";
        public const string Theme9 = "theme9";
        public const string Theme10 = "theme10";
        public const string Theme11 = "theme11";

        public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(1);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);

        public const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:sszzz";

        public const string TYPE_APPROVED = "APPROVE";
        public const string TYPE_REJECT = "REJECT";
        public const string TYPE_REQUEST_INFO = "REQUEST_INFO";

        public const string R_TYPE_MENU_WAITTING = "watting";
        public const string R_TYPE_MENU_APPROVED = "approved";
        public const string R_TYPE_MENU_RERECTED = "rejected";
        public const string R_TYPE_MENU_DRAFT = "draft";
        public const string R_TYPE_MENU_OTHER = "other";


        public const string M_APPROVED= "m_approved";
        public const string M_WATTING = "m_waiting";
        public const string M_REJECTED = "m_rejected";
        public const string U_WAITTINGME = "u_waittingme";
        public const string U_APPROVEDBYME = "u_approvedbyme";
        public const string U_REJECTEDBYME = "u_rejectedbyme";
        public const string IS_DRAFT = "is_draft";
        public const string U_OTHER_REQUEST = "u_other_request";
        public const string M_OTHER_REQUEST = "m_other_request";

        public const string STATUS_CREATE_REQUEST = "CREATE_REQUEST";
        public const string STATUS_CREATE_DRAFT = "CREATE_DRAFT";
        public const string STATUS_WAIT_TEM_MANAGER = "WAIT_TEM_MANAGER";
        public const string STATUS_TEM_MANAGER_APPROVAL = "TEM_MANAGER_APPROVAL";
        public const string STATUS_TEM_MANAGER_REJECT = "TEM_MANAGER_REJECT";
        public const string STATUS_MANAGER_APPROVE = "MANAGER_APPROVE";
        public const string STATUS_MANAGER_REJECT = "MANAGER_REJECT";
        public const string STATUS_ADM_APPROVE = "ADM_APPROVE";
        public const string STATUS_ADM_REJECT = "ADM_REJECT";
        public const string STATUS_MANAGER_REQUEST_INFO = "MANAGER_REQUEST_INFO";
        public const string STATUS_ADM_REQUEST_INFO = "ADM_REQUEST_INFO";

        public const string STATUS_WAIT_GUARD = "WAIT_GUARD";
        public const string STATUS_BROUGHT_OUT = "BROUGHT_OUT";
        public const string STATUS_BROUGHT_IN = "BROUGHT_IN";
        public const string STATUS_WENT_OUT = "WENT_OUT";
        public const string STATUS_GOT_IN = "GOT_IN";

        public const string ROLE_ADM = "ADM";

        public const string VENDER_ASSET_RQ3="VENDER-ASSET-RQ3";
        public const string CLIENT_RQ4= "CLIENT-RQ4";
        public const string VENDER_EMPLOYEES_RQ2= "NGƯỜI VÀO LÀM VIỆC TẠI TMV";
        public const string INTERNAL_RQ1= "INTERNAL-RQ1";

        public const string COL_ERR_TITLE_REQUEST = "TitleRequest";
        public const string COL_ERR_VENDER_NAME = "VenderName";
        public const string COL_ERR_PERSION_INCHANGE_OF_SUB = "PersonInChargeOfSubName";
        public const string COL_ERR_LIVE_MONITOR_NAME = "LiveMonitorName";
        public const string COL_ERR_LIVE_MONITOR_DEPARTMENT = "LiveMonitorDepartment";
        public const string COL_ERR_WHERE_TO_WORK = "WhereToWork";
        public const string COL_ERR_WHERE_TO_BRING = "WhereToBring";
        public const string COL_ERR_START_DATE = "StartRequestDate";
        public const string COL_ERR_END_DATE = "EndRequestDate";
        public const string COL_ERR_LIST_WORKER = "ListWorker";
        public const string COL_ERR_LIST_ASSET = "ListAsset";
        public const string COL_ERR_LIST_CLIENT = "ListClient";

        public const string COL_ERR_TRADE_UNION_ORGANAGATION = "TradeUnionOrganization";
        public const string COL_ERR_DEPARTMENT_CLIENT = "DepartmentClient";
        public const string YOU_HAVE_NOT_ENTERED_THE_VALUE = "YouHaveNotEnteredTheValue";
        public const string INPUT_IS_INCORECT = "InputIsIncorrect";
    }
}
