using GD.Shared.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMapTest.Services
{
    class AuthProvider
    {
        public static Action JwtSetted { get; set; }

        public static DecodedToken UserToken
        {
            get
            {
                var jwt = Preferences.Get("JWT", null);
                var converted = ConvertJwtStringToJwtSecurityToken(jwt);
                var res = DecodeJwt(converted);
                return res;
            }
        }
        public static JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string? jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            return token;
        }

        public static DecodedToken DecodeJwt(JwtSecurityToken token)
        {
            var keyId = token.Header.Kid;
            var audience = token.Audiences.ToList();
            var claims = token.Claims.Select(claim => (claim.Type, claim.Value)).ToList();

            var id = Guid.Parse(claims.FirstOrDefault(c => c.Type == GDUserClaimTypes.Id).Value);
            var email = claims.FirstOrDefault(c => c.Type == GDUserClaimTypes.Email).Value;
            var roles = claims.FirstOrDefault(c => c.Type == GDUserClaimTypes.Roles).Value;
            return new DecodedToken(
                keyId,
                token.Issuer,
                audience,
                claims,
                token.ValidTo,
                token.SignatureAlgorithm,
                token.RawData,
                token.Subject,
                token.ValidFrom,
                token.EncodedHeader,
                token.EncodedPayload,
                roles,
                id,
                email
            );
        }
    }
}
