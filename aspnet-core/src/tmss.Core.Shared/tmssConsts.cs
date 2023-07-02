namespace tmss
{
    public class tmssConsts
    {
        public const string LocalizationSourceName = "tmss";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        public const bool AllowTenantsToChangeEmailSettings = false;

        public const string Currency = "USD";

        public const string CurrencySign = "$";

        public const string AbpApiClientUserAgent = "AbpApiClient";

        //public const string EMAIL_NOREPLY = "noreply@toyotavn.com.vn";

        //public const string EMAIL_SUFFIX = "@toyotavn.com.vn";
        public const string EMAIL_NOREPLY = "noreply@toyotavn.com.vn";

        public const string EMAIL_SUFFIX = "@toyotavn.com.vn";

        public const string PROPERTY_LDAP_MANAGER = "manager";

        public const string PROPERTY_LDAP_DIVISION = "division";

        public const string PROPERTY_LDAP_DEPARTMENT = "department";
        
        //radius
        public const string RADIUS_SERVER_NAME = "192.168.2.112";
        public const string RADIUS_SHARED_SECRET = "System@6868@";
        public const int RADIUS_PORT = 1645;
        public const int TENANT_ID_DEFAULT = 1;
        // Note:
        // Minimum accepted payment amount. If a payment amount is less then that minimum value payment progress will continue without charging payment
        // Even though we can use multiple payment methods, users always can go and use the highest accepted payment amount.
        //For example, you use Stripe and PayPal. Let say that stripe accepts min 5$ and PayPal accepts min 3$. If your payment amount is 4$.
        // User will prefer to use a payment method with the highest accept value which is a Stripe in this case.
        public const decimal MinimumUpgradePaymentAmount = 1M;
    }
}
