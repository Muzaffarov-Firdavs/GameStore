using GameStore.Service.Commons.Helpers;

namespace GameStore.Web.Configurations
{
    public static class HttpContextConfiguration
    {
        public static void InitAccessor(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            HttpContextHelper.Accessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        }
    }
}
