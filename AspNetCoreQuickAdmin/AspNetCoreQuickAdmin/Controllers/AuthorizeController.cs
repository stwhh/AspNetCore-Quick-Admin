using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreQuickAdmin.JWTAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.DTO;

namespace AspNetCoreQuickAdmin.Controllers
{
    [Route("api/Authorize")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {

        private JwtSetting _jwtSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_jwtSetting">jwt认证配置</param>
        public AuthorizeController(IOptions<JwtSetting> _jwtSetting)
        {
            _jwtSettings = _jwtSetting.Value;
        }

        [Route("Token")]
        [HttpPost]
        public IActionResult Token([FromBody] LoginInInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!(input.UserName == "stwhh" && input.Password == "123456")) //todo-stwhh 判断账号密码是否正确
            {
                return BadRequest();
            }

            var claim = new Claim[]
            {
                new Claim("name", "stwhh"), //ClaimTypes.Name
                new Claim("userNo", "0001"), //这里可自定义申明信息，生成的token登录后可以获取到Claim信息
                new Claim("role", "admin"),
                new Claim("email", "admin@qq.com")
            };

            //对称秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            //签名证书(秘钥，加密算法)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claim,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinute), //有效期
                signingCredentials: creds);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return Ok(new {token = handler.WriteToken(token)});
        }
    }
}