namespace TruckApi.Infrastructure.Cache;

public static class CacheKeys
{
    public static class Auth
    {
        public static string Session(string userId) => $"auth:session:{userId}";
        public static string RefreshToken(string token) => $"auth:refresh:{token}";
        public static string UserTokens(string userId) => $"auth:user_tokens:{userId}";

        public static class Otp
        {
            public static string Code(string whatsapp) => $"auth:otp:code:{whatsapp}";
            public static string Cooldown(string whatsapp) => $"auth:otp:cooldown:{whatsapp}";
        }

        public static class Forgot
        {
            public static string Code(string whatsapp) => $"auth:forgot:code:{whatsapp}";
            public static string Cooldown(string whatsapp) => $"auth:forgot:cooldown:{whatsapp}";
        }
    }
}
