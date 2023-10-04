using Microsoft.AspNetCore.Http;

namespace GameStore.Service.Commons.Helpers
{
    public class HttpContextHelper
    {
        public static IHttpContextAccessor Accessor { get; set; }
        public static HttpContext HttpContext => Accessor?.HttpContext;
        public static IHeaderDictionary ResponseHeaders => HttpContext?.Response?.Headers;
        public static long? UserId =>
            long.TryParse(HttpContext?.User?.FindFirst("id")?.Value, out _tempUserId) ? _tempUserId : null;
        public static string UserRole => HttpContext?.User?.FindFirst("role")?.Value;
        public static string FirstName => HttpContext?.User?.FindFirst("FirstName")?.Value;
        public static string LastName => HttpContext?.User?.FindFirst("LastName")?.Value;

        private static long _tempUserId;
    }
}
