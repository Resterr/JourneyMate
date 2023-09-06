using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JourneyMate.Infrastructure.Security;

internal sealed class TokenService : ITokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _signingKey;
    private readonly TimeSpan _accessTokenExpiry;
    private readonly TimeSpan _refreshTokenExpiry;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityToken = new();
    private readonly IDateTimeService _dateTimeService;

    public TokenService(IOptions<AuthOptions> options, IDateTimeService dateTimeService)
    {
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _signingKey = options.Value.SigningKey;
        _accessTokenExpiry = options.Value.AccessTokenExpiry ?? TimeSpan.FromMinutes(10);
        _refreshTokenExpiry = options.Value.RefreshTokenExpiry ?? TimeSpan.FromHours(24);
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_signingKey)),
                SecurityAlgorithms.HmacSha256);
        _dateTimeService = dateTimeService;
    }

    public string GenerateAccessToken(Guid userId, string userEmail, string userName)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, userEmail),
            new(ClaimTypes.Name, userName)
        };

        var now = _dateTimeService.Now;
        var expires = now.Add(_accessTokenExpiry);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        var token = _jwtSecurityToken.WriteToken(jwt);

        return token;
    }

    public string GenerateAccessTokenFromClaims(IEnumerable<Claim> claims)
    {
        var now = _dateTimeService.Now;
        var expires = now.Add(_accessTokenExpiry);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        var token = _jwtSecurityToken.WriteToken(jwt);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public DateTime GetRefreshExpiryDate()
    {
        var now = _dateTimeService.Now;
        var expires = now.Add(_refreshTokenExpiry);

        return expires;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new BadRequestException("Invalid token.");

            return principal;
        }
        catch
        {
            throw new BadRequestException("Invalid token.");
        }
    }
}
