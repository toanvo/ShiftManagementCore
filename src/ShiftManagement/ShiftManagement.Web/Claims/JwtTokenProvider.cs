using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

namespace ShiftManagement.Web.Claims
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using ShiftManagement.Domain;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class JwtTokenProvider
    {
        private static readonly string PrivateKey = "!@$#!ASA+-%DASa2454619Af";

        private readonly RequestDelegate _next;
        private int _tokenExpiration;
        private SigningCredentials _signingCredentials;
        private string _issuer;
        private string _tokenEndPoint;

        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly IConfiguration _config;
        private readonly ILogger<JwtTokenProvider> _logger;

        public static SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));

        public JwtTokenProvider(RequestDelegate next, UserManager<Employee> userManager, SignInManager<Employee> signInManager, IConfiguration configuration)
        {
            _next = next;
            _signingCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            
            Initialize();
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Path.Equals(_tokenEndPoint, StringComparison.Ordinal))
            {
                return _next(httpContext);
            }
            
            if (httpContext.Request.Method.Equals("POST") && httpContext.Request.HasFormContentType)
            {
                return CreateToken(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad request.");
            }
        }

        private void Initialize()
        {
            _tokenExpiration = int.Parse(_config["Tokens:Expires"]);
            _issuer = _config["Tokens:Issuer"];
            _tokenEndPoint = _config["Tokens:Endpoint"];
        }

        private async Task CreateToken(HttpContext httpContext)
        {
            try
            {
                var formCollection = await httpContext.Request.ReadFormAsync();
                formCollection.TryGetValue("username", out StringValues username);
                formCollection.TryGetValue("password", out StringValues password);
                
                var isSucceed = await VerifyUserName(username.ToString(), password.ToString());

                if (isSucceed)
                {
                    var encodedToken = BuildEncodedToken(_issuer, username);

                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(encodedToken);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is an exception happened while creating JWT: {ex}");
            }

            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Invalid username or password.");
        }

        private async Task<bool> VerifyUserName(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null && username.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(username);
            }

            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        private string BuildEncodedToken(string issuer, string username)
        {
            DateTime now = DateTime.UtcNow;
            var claims = GetClaims(now, issuer, username);

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_tokenExpiration),
                signingCredentials: _signingCredentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var jwt = new
            {
                access_token = encodedToken,
                expiration = token.ValidTo
            };
            
            return JsonConvert.SerializeObject(jwt);
        }

        private static Claim[] GetClaims(DateTime now, string issuer, string userName)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            return claims;
        }
    }
}
