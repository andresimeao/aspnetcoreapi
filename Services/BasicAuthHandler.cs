using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Api.Data;
using Api.Models;
using System.Security.Claims;

namespace Api.Services
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ApiContext _context;


        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ApiContext context) : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Por favor insira autorização no Header!");
            }
            try
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                var credentials = Encoding.UTF8.GetString(bytes).Split(new[] { ':' }, 2);

                var username = credentials[0];
                var password = credentials[1];

                User user = _context.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();

                if (user == null)
                {
                    return AuthenticateResult.Fail("username ou password incorreto !");
                }
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (System.Exception)
            {

                return AuthenticateResult.Fail("Ocorreu um erro !");
            }

        }

    }
}