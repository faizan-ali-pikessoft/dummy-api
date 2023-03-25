using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rest_Api
{
    public class TokenProvider
    {                
        public UserInfo LoginUser(UserInfo user)
        {
            var userinfo = new UserInfo();
            try
            {                
                userinfo.UserId = Convert.ToInt32(user.UserId);
                userinfo.Email = "fazimalik37@gmail.com";
                var secretBytes = Encoding.UTF8.GetBytes("SAMPLE_REST_API-2374-OFFKDI940NG7:56753253-TYUW-5769-0921-KFIROX29ZOXV");
                var key = new SymmetricSecurityKey(secretBytes);
                var algorithm = SecurityAlgorithms.HmacSha256;

                var signingCredentials = new SigningCredentials(key, algorithm);
                const string Audiance = "http://localhost:4015/";
                const string Issuer = Audiance;                
                //Generate Token for user
                var JWToken = new JwtSecurityToken(
                    Issuer,
                    Audiance,
                    claims: GetUserClaims(userinfo),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddHours(24)).DateTime,
                    //expires: new DateTimeOffset(DateTime.Now.AddMinutes(5)).DateTime, // comment for staging server
                    signingCredentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                userinfo.Token = token;
                return userinfo;
            }
            catch (Exception ex)
            {
                userinfo = null;
                return userinfo;
            }                       
        }
        private IEnumerable<Claim> GetUserClaims(UserInfo user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            //_claim = new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName);
            //claims.Add(_claim);
            _claim = new Claim("USERID", user.UserId.ToString());
            claims.Add(_claim);
            _claim = new Claim("EMAIL", user.Email);
            claims.Add(_claim);

            return claims.AsEnumerable<Claim>();
        }
    }
}
