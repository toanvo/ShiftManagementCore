using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShiftManagement.Domain;

namespace ShiftManagement.Web.Claims
{
    using Microsoft.AspNetCore.Builder;

    public static class JwtExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder, IServiceProvider serviceProvider)
        {           
            return builder.UseMiddleware<JwtTokenProvider>(
                serviceProvider.GetService<UserManager<Employee>>(), 
                serviceProvider.GetService<SignInManager<Employee>>(),
                serviceProvider.GetService<IConfiguration>());
        }
    }
}
