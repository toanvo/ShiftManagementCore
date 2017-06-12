namespace ShiftManagement.Web.Claims
{
    using Microsoft.AspNetCore.Builder;

    public static class JwtExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenProvider>();
        }
    }
}
