export class AppConsts {

    static readonly tenancyNamePlaceHolderInUrl = '{TENANCY_NAME}';

    static remoteServiceBaseUrl: string;
    static remoteServiceBaseUrlFormat: string;
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish
    static appBaseUrlFormat: string;
    static recaptchaSiteKey: string;
    static subscriptionExpireNootifyDayCount: number;
    static readonly phoneNumber_regex = /((09|03|07|08|05)+([0-9]{8})\b)/g;
    
    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly localization = {
        defaultLocalizationSourceName: 'tmss'
    };

    static readonly authorization = {
        encrptedAuthTokenName: 'enc_auth_token'
    };

    static readonly grid = {
        defaultPageSize: 10
    };

    static readonly requestAsset = {
        request: "Request",
        waitManager: "WaitManager",
        adminApproval: "Admin",
        adminApprovaled: "ADMApprovaled",
        adminRejected: "ADMRejected",
        waitAdmin: "WaitADM",
        success: "Success",
        managerApprovaled: "ManagerApproval",
        temManagerApprovaled: "TemManagerApprovaled",
        managerRejected: "ManagerRejected",
        temManagerRejected: "TemManagerRejected",
        waitingApprove: "WaitingApprove"
    };

    static readonly message = {
        deleteSuccess: "SuccessfullyDeleted",
        saveDraftSuccess: "SaveDraftSuccess",
        checkInSuccess: "SuccessfullyCheckIn",
        checkOutSuccess: "SuccessfullyCheckOut",
        confimDelete: "AreYouSure",
        saveSuccess: "SavedSuccessfully",
        confirmApprove: "ConfimApprove",
        confirmSetLearnedSafetySuccess: "ConfirmSetLearnedSafetySuccess",
        confirmSetNotLearnedSafetySuccess: "ConfirmSetNotLearnedSafetySuccess",
        confirmReject: "ConfimReject",
        rejectSuccess: "RejectSuccess",
        rejectError: "RejectError",
        youDontAddAsset: "YouDontAddAsset",
        youDontAddWorker: "YouDontAddWorker",
        youDontAddClient: "YouDontAddClient",
        identityCardErrSafety: "IdentityCardDontMapOnListLearnedSafety",
        historyRequest: "HistoryRequest",
        historyAsset: "HistoryAsset",
        historyEmployees: "HistoryEmployees",
        inManament: "IN",
        outManament: "OUT",
        selectVender: "SelectVender",
        sendSuccessfully: "SendSuccessfully",
        requestInforSuccess: "RequestInfoSuccess",  
        requestInfoError: "RequestInfoError",
        confirmResend: "ConfirmResend",
    };

    static readonly TitleRequest ={
        waitRequest: "WaitRequest",
    }

    static readonly StatusRequestMenu ={
        M_APPROVED: "m_approved",
        M_WATTING: "m_waiting",
        M_REJECTED: "m_rejected",
        U_WAITTINGME: "u_waittingme",
        U_APPROVEDBYME: "u_approvedbyme",
        U_REJECTEDBYME: "u_rejectedbyme",
        IS_DRAFT: "is_draft"
    }

    static readonly StatusRequestInOutMenu ={
        ST_REQUEST_IN: "AssetInManament",
        ST_REQUEST_OUT: "AssetOutManament",
    }

    static readonly KeyStatus = {
        STATUS_CREATE_REQUEST: "CREATE_REQUEST",
        STATUS_MANAGER_APPROVE: "MANAGER_APPROVE",
        STATUS_TEM_MANAGER_REJECT: "TEM_MANAGER_REJECT",
        STATUS_MANAGER_REJECT: "MANAGER_REJECT",
        STATUS_ADM_APPROVE: "ADM_APPROVE",
        STATUS_ADM_REJECT: "ADM_REJECT",
        STATUS_CREATE_DRAFT: "CREATE_DRAFT",
        STATUS_MANAGER_REQUEST_INFO: "MANAGER_REQUEST_INFO",
        STATUS_ADM_REQUEST_INFO: "ADM_REQUEST_INFO"
    }

    static readonly TypeApproveRject = {
        TYPE_APPROVED: "APPROVE",
        TYPE_REJECT: "REJECT",
        TYPE_REQUEST_INFO: "REQUEST_INFO"
    }

    static readonly TypeRequestName = {
       INTERNALREQUEST: "InternalRequest",
       EMPLOYEESVENDERREQUEST: "EmployeesVenderRequest",
       ASSETVENDERREQUEST: "AssetVenderRequest",
       CLIENTREQUEST: "ClientRequest",

       ADDINTERNAL: "AddInternal",
       ADDEMPLOYEESVENDER: "AddEmployeesVender",
       ADDASSETVENDER: "AssetVender",

       CREATEEMPLOYEESVENDER: "CreateEmployeesVender",
       CREATEASSETVENDER: "CreateAssetVender",
       CREATECLIENTREQUEST: "CreateClientRequest",

       INTERNAL: "Internal",
       EMPLOYEESVENDER: "EmployeesVender",
       ASSETVENDER: "AssetVender"
    }

    static readonly MinimumUpgradePaymentAmount = 1;

    /// <summary>
    /// Gets current version of the application.
    /// It's also shown in the web page.
    /// </summary>
    static readonly WebAppGuiVersion = '10.2.0';
}
