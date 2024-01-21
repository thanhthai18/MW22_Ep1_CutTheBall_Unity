namespace Runtime.Definition
{
    public class LogicCode
    {
        public const int SUCCESS = 0;
        public const int FAILED = 1;

        public const int INVALID_INPUT_DATA = 5;
        public const int NOT_ENOUGH_RESOURCES = 6;

        public const int PLAYER_ALREADY_LOGIN = 10;
        public const int PLAYER_NOT_LOGIN = 11;
        public const int PLAYER_BAN = 12;

        public const int FEATURE_NOT_UNLOCKED = 30;
        public const int DATA_CONFLICTED = 31;
        public const int MULTIPLE_ACCOUNT_LOGIN = 32;

        // https://github.com/googlesamples/google-signin-unity/blob/master/GoogleSignInPlugin/Assets/GoogleSignIn/GoogleSignInStatusCode.cs
        public const int LOGIN_GOOGLE_FAILED = 1000;
        public const int LOGIN_GOOGLE_API_NOT_CONNECTED = 1001;
        public const int LOGIN_GOOGLE_CANCELED = 1002;
        public const int LOGIN_GOOGLE_INTERRUPTED = 1003;
        public const int LOGIN_GOOGLE_INVALID_ACCOUNT = 1004;
        public const int LOGIN_GOOGLE_TIMEOUT = 1005;
        public const int LOGIN_GOOGLE_DEVELOPER_ERROR = 1006;
        public const int LOGIN_GOOGLE_INTERNAL_ERROR = 1007;
        public const int LOGIN_GOOGLE_NETWORK_ERROR = 1008;
        public const int LOGIN_GOOGLE_ERROR = 1009;
        public const int LOGIN_GOOGLE_UNKNOWN = 1010;

        // https://firebase.google.com/docs/reference/unity/namespace/firebase/auth
        public const int BINDING_FAILED = 1100;
        public const int BINDING_FAILURE = 1101;
        public const int BINDING_INVALID_CUSTOM_TOKEN = 1102;
        public const int BINDING_CUSTOM_TOKEN_MISMATCH = 1103;
        public const int BINDING_INVALID_CREDENTIAL = 1104;
        public const int BINDING_USER_DISABLED = 1105;
        public const int BINDING_ACCOUNT_EXISTS_WITH_DIFFERENT_CREDENTIALS = 1106;
        public const int BINDING_OPERATION_NOT_ALLOWED = 1107;
        public const int BINDING_EMAIL_ALREADY_IN_USE = 1108;
        public const int BINDING_REQUIRES_RECENT_LOGIN = 1109;
        public const int BINDING_CREDENTIAL_ALREADY_IN_USE = 1110; // 0x0000000A
        public const int BINDING_INVALID_EMAIL = 1111; // 0x0000000B
        public const int BINDING_WRONG_PASSWORD = 1112; // 0x0000000C
        public const int BINDING_TOO_MANY_REQUESTS = 1113; // 0x0000000D
        public const int BINDING_USER_NOT_FOUND = 1114; // 0x0000000E
        public const int BINDING_PROVIDER_ALREADY_LINKED = 1115; // 0x0000000F
        public const int BINDING_NO_SUCH_PROVIDER = 1116; // 0x00000010
        public const int BINDING_INVALID_USER_TOKEN = 1117; // 0x00000011
        public const int BINDING_USER_TOKEN_EXPIRED = 1118; // 0x00000012
        public const int BINDING_NETWORK_REQUEST_FAILED = 1119; // 0x00000013
        public const int BINDING_INVALID_API_KEY = 1120; // 0x00000014
        public const int BINDING_APP_NOT_AUTHORIZED = 1121; // 0x00000015
        public const int BINDING_USER_MISMATCH = 1122; // 0x00000016
        public const int BINDING_WEAK_PASSWORD = 1123; // 0x00000017
        public const int BINDING_NO_SIGNED_IN_USER = 1124; // 0x00000018
        public const int BINDING_API_NOT_AVAILABLE = 1125; // 0x00000019
        public const int BINDING_EXPIRED_ACTION_CODE = 1126; // 0x0000001A
        public const int BINDING_INVALID_ACTION_CODE = 1127; // 0x0000001B
        public const int BINDING_INVALID_MESSAGE_PAYLOAD = 1128; // 0x0000001C
        public const int BINDING_INVALID_PHONE_NUMBER = 1129; // 0x0000001D
        public const int BINDING_MISSING_PHONE_NUMBER = 1130; // 0x0000001E
        public const int BINDING_INVALID_RECIPIENT_EMAIL = 1131; // 0x0000001F
        public const int BINDING_INVALID_SENDER = 1132; // 0x00000020
        public const int BINDING_INVALID_VERIFICATION_CODE = 1133; // 0x00000021
        public const int BINDING_INVALID_VERIFICATION_ID = 1134; // 0x00000022
        public const int BINDING_MISSING_VERIFICATION_CODE = 1135; // 0x00000023
        public const int BINDING_MISSING_VERIFICATION_ID = 1136; // 0x00000024
        public const int BINDING_MISSING_EMAIL = 1137; // 0x00000025
        public const int BINDING_MISSING_PASSWORD = 1138; // 0x00000026
        public const int BINDING_QUOTA_EXCEEDED = 1139; // 0x00000027
        public const int BINDING_RETRY_PHONE_AUTH = 1140; // 0x00000028
        public const int BINDING_SESSION_EXPIRED = 1141; // 0x00000029
        public const int BINDING_APP_NOT_VERIFIED = 1142; // 0x0000002A
        public const int BINDING_APP_VERIFICATION_FAILED = 1143; // 0x0000002B
        public const int BINDING_CAPTCHA_CHECK_FAILED = 1144; // 0x0000002C
        public const int BINDING_INVALID_APP_CREDENTIAL = 1145; // 0x0000002D
        public const int BINDING_MISSING_APP_CREDENTIAL = 1146; // 0x0000002E
        public const int BINDING_INVALID_CLIENT_ID = 1147; // 0x0000002F
        public const int BINDING_INVALID_CONTINUE_URI = 1148; // 0x00000030
        public const int BINDING_MISSING_CONTINUE_URI = 1149; // 0x00000031
        public const int BINDING_KEYCHAIN_ERROR = 1150; // 0x00000032
        public const int BINDING_MISSING_APP_TOKEN = 1151; // 0x00000033
        public const int BINDING_MISSING_IOS_BUNDLE_ID = 1152; // 0x00000034
        public const int BINDING_NOTIFICATION_NOT_FORWARDED = 1153; // 0x00000035
        public const int BINDING_UNAUTHORIZED_DOMAIN = 1154; // 0x00000036
        public const int BINDING_WEB_CONTEXT_ALREADY_PRESENTED = 1155; // 0x00000037
        public const int BINDING_WEB_CONTEXT_CANCELLED = 1156; // 0x00000038
        public const int BINDING_DYNAMIC_LINK_NOT_ACTIVATED = 1157; // 0x00000039
        public const int BINDING_CANCELLED = 1158; // 0x0000003A
        public const int BINDING_INVALID_PROVIDER_ID = 1159; // 0x0000003B
        public const int BINDING_WEB_INTERNAL_ERROR = 1160; // 0x0000003C
        public const int BINDING_WEB_STORAGE_UNSUPPORTED = 1161; // 0x0000003D
        public const int BINDING_TENANT_ID_MISMATCH = 1162; // 0x0000003E
        public const int BINDING_UNSUPPORTED_TENANT_OPERATION = 1163; // 0x0000003F
        public const int BINDING_INVALID_LINK_DOMAIN = 1164; // 0x00000040
        public const int BINDING_REJECTED_CREDENTIAL = 1165; // 0x00000041
        public const int BINDING_PHONE_NUMBER_NOT_FOUND = 1166; // 0x00000042
        public const int BINDING_INVALID_TENANT_ID = 1167; // 0x00000043
        public const int BINDING_MISSING_CLIENT_IDENTIFIER = 1168; // 0x00000044
        public const int BINDING_MISSING_MULTI_FACTOR_SESSION = 1169; // 0x00000045
        public const int BINDING_MISSING_MULTI_FACTOR_INFO = 1170; // 0x00000046
        public const int BINDING_INVALID_MULTI_FACTOR_SESSION = 1171; // 0x00000047
        public const int BINDING_MULTI_FACTOR_INFO_NOT_FOUND = 1172; // 0x00000048
        public const int BINDING_ADMIN_RESTRICTED_OPERATION = 1173; // 0x00000049
        public const int BINDING_UNVERIFIED_EMAIL = 1174; // 0x0000004A
        public const int BINDING_SECOND_FACTOR_ALREADY_ENROLLED = 1175; // 0x0000004B
        public const int BINDING_MAXIMUM_SECOND_FACTOR_COUNT_EXCEEDED = 1176; // 0x0000004C
        public const int BINDING_UNSUPPORTED_FIRST_FACTOR = 1177; // 0x0000004D
        public const int BINDING_EMAIL_CHANGE_NEEDS_VERIFICATION = 1178; // 0x0000004E
    }
}