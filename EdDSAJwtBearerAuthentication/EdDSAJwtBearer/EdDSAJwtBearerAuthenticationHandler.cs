using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace EdDSAJwtBearer
{
    public class EdDSAJwtBearerAuthenticationHandler : AuthenticationHandler<EdDSAJwtBearerOptions>
    {
        public EdDSAJwtBearerAuthenticationHandler(IOptionsMonitor<EdDSAJwtBearerOptions> options,
                                                   ILoggerFactory logger,
                                                   UrlEncoder encoder,
                                                   ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult Result = AuthenticateResult.NoResult();

            if (Request.Headers.ContainsKey("Authorization"))
            {
                if (AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"],
                    out AuthenticationHeaderValue HeaderValue))
                {
                    if ("Bearer".Equals(HeaderValue.Scheme, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            string Error;
                            string Token = HeaderValue.Parameter;
                            if (TryGetPayLoadWithTokenValidation(Token, Options,
                                out Dictionary<string, object> PayLoad, out Error))
                            {
                                List<Claim> Claims = PayLoad.Where(c => c.Key != "role")
                                                             .Select(t => new Claim(t.Key, $"{t.Value}")).ToList();

                                if (PayLoad.TryGetValue("role", out object Roles))
                                {
                                    string[] RolesArray = JsonSerializer.Deserialize<string[]>(Roles.ToString());

                                    if (RolesArray != null)
                                    {
                                        foreach (var Role in RolesArray)
                                        {
                                            Claims.Add(new Claim("role", Role.ToString()));
                                        }
                                    }
                                }

                                ClaimsIdentity Identity = new ClaimsIdentity(Claims,
                                    Scheme.Name, "firstName", "role");

                                ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);

                                AuthenticationTicket Ticket;

                                if (Options.SaveToken)
                                {
                                    var Properties = new AuthenticationProperties();
                                    Properties.StoreTokens(new AuthenticationToken[]
                                    {
                                        new AuthenticationToken
                                        {
                                            Name = "access_token", Value = Token
                                        }
                                    });

                                    Ticket = new AuthenticationTicket(Principal, Properties, Scheme.Name);
                                }
                                else
                                {
                                    Ticket = new AuthenticationTicket(Principal, Scheme.Name);
                                }

                                Result = AuthenticateResult.Success(Ticket);
                            }
                            else
                            {
                                Result = AuthenticateResult.Fail(Error);
                            }
                        }
                        catch
                        {
                            Result = AuthenticateResult.Fail(EdDSAJwtBearerErros.InvalidToken);
                        }
                    }
                }
            }

            return Task.FromResult(Result);

        }

        private bool TryGetPayLoadWithTokenValidation(string token, EdDSAJwtBearerOptions options,
                                                      out Dictionary<string, object> payLoad, out string error)
        {
            bool IsValid = false;
            payLoad = default;
            error = string.Empty;

            try
            {
                if (EdDSATokenHandler.GetTryPaylosdFromToken(token, options.PublicSigningKey, out payLoad))
                {
                    IsValid = true;
                    object Value;
                    if (options.ValidateIssuer)
                    {
                        IsValid = payLoad.TryGetValue("iss", out Value);

                        if (IsValid)
                        {
                            IsValid = options.ValidIssuer.Equals(Value.ToString(), StringComparison.OrdinalIgnoreCase);
                        }
                        if (!IsValid)
                        {
                            error = EdDSAJwtBearerErros.InvalidIssuer;
                        }
                    }

                    if (IsValid && options.ValidateAudience)
                    {
                        IsValid = payLoad.TryGetValue("aud", out Value);

                        if (IsValid)
                        {
                            string[] Audiences = Value.ToString().Split(",");
                            IsValid = Audiences.Contains(options.ValidAudience);
                        }

                        if (!IsValid)
                        {
                            error = EdDSAJwtBearerErros.InvalidAudience;
                        }
                    }

                    if (IsValid && options.ValidateLifetime)
                    {
                        IsValid = payLoad.TryGetValue("exp", out Value);
                        if (IsValid)
                        {
                            long ExpirationTime = Convert.ToInt64(Value.ToString());
                            IsValid = ExpirationTime > new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                        }

                        if (!IsValid)
                        {
                            error = EdDSAJwtBearerErros.ExpireToken;
                        }
                    }

                }
                else
                {
                    IsValid = false;
                    error = EdDSAJwtBearerErros.InvalidToken;
                }
            }
            catch
            {
                IsValid = false;
                error = EdDSAJwtBearerErros.InvalidToken;
            }

            return IsValid;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = "Bearer";
            return base.HandleChallengeAsync(properties);
        }
    }
}
