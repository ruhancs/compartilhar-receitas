using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace MeuLivroReceitas.Application.Services.Token;

public class TokenController
{
    private const string EmailAlias = "eml";
    private readonly double _expires;
    private readonly string _securityKey;

    public TokenController(double expires, string securityKey)
    {
        _expires = expires;
        _securityKey = securityKey;
    }

    public string generateToken(string email)
    {
        var claims = new List<Claim>
            {
                new Claim(EmailAlias, email),
            };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_expires),
            SigningCredentials = new SigningCredentials(
                SimmetricKey(), SecurityAlgorithms.HmacSha256Signature
                )
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken); 
    }

    public void ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validateParams = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SimmetricKey(),//validar a chave simetrica para criaçao do token
            ClockSkew = new TimeSpan(0),//validar a data de expiraçao
            ValidateIssuer = false,
            ValidateAudience = false
        };

        //se token nao for valido lancara uma exception
        tokenHandler.ValidateToken(token, validateParams, out _);
    }

    private SymmetricSecurityKey SimmetricKey()
    {
        var symmetricKey = Convert.FromBase64String( _securityKey );
        return new SymmetricSecurityKey(symmetricKey);
    }
}
