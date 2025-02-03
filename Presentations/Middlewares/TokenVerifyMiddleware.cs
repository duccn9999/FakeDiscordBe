using System.IdentityModel.Tokens.Jwt;

namespace Presentations.Middlewares
{
    public class TokenVerifyMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenVerifyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token["Bearer ".Length..];
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwtToken = handler.ReadJwtToken(token);
                        var expClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);
                        if(expClaim != null)
                        {
                            // Get the expiration time from the token
                            var expUnix = long.Parse(expClaim.Value);
                            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                            // Check if the token is expired
                            if (expirationDate < DateTime.UtcNow)
                            {
                                Console.WriteLine($"Token expried at {expirationDate}");
                                context.Response.StatusCode = 401; // Unauthorized
                                await context.Response.WriteAsync("Token has expired.");
                                return; // End the pipeline
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    // If an error occurs, assume the token is invalid
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Invalid token.");
                    return; // End the pipeline
                }
            }
            await _next(context);
        }
    }
}
