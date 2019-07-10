namespace VerticalSliceArchitecture.Core.Exceptions
{
    public static class DomainExceptionCodes
    {
        public static string InvalidLogin => "invalid_user_login";
        public static string InvalidEmail => "invalid_user_email";
        public static string InvalidPassword => "invalid_password";
        public static string InvalidSalt => "invalid_salt";

        public static string InvalidDeviceId => "invalid_device_id";
        public static string InvalidDeviceName => "invalid_device_name";
        public static string InvalidRefreshToken => "invalid_refresh_token";
        public static string InvalidRefreshTokenExpiresDate => "invalid_refresh_token_expires_date";

        public static string InvalidTitle => "invalid_title";
    }
}