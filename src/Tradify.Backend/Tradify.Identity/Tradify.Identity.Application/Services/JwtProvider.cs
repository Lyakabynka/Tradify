﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tradify.Identity.Application.Configurations;
using Tradify.Identity.Domain.Entities;

namespace Tradify.Identity.Application.Services;

public class JwtProvider
{
    private readonly JwtConfiguration _configuration;
    public JwtProvider(IOptions<JwtConfiguration> options) =>
        _configuration = options.Value;

    public string CreateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384Signature);

        var token = new JwtSecurityToken(
            _configuration.Issuer,
            _configuration.Audience,
            claims,
            null,
            DateTime.Now.AddMinutes(_configuration.MinutesToExpiration),
            credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}