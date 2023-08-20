﻿using System.Security.Claims;

namespace JourneyMate.Application.Common.Interfaces;
public interface ITokenService
{
	string GenerateAccessToken(Guid userId, string userEmail, string userName);
	string GenerateAccessTokenFromClaims(IEnumerable<Claim> claims);
	string GenerateRefreshToken();
	ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	DateTime GetRefreshExpiryDate();
}