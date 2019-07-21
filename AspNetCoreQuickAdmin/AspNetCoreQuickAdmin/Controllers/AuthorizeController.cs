using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreQuickAdmin.JWTAuth;
using Common;
using DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.DTO;
using Model.Entities;

namespace AspNetCoreQuickAdmin.Controllers
{
    [Route("api/Authorize")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private JwtSetting _jwtSettings;
        private readonly IQuickAdminRepository<User> _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="jwtSetting">jwt认证配置</param>
        /// <param name="userRepository"></param>
        public AuthorizeController(IConfiguration configuration,
            IOptions<JwtSetting> jwtSetting,
            IQuickAdminRepository<User> userRepository)
        {
            _configuration = configuration;
            _jwtSettings = jwtSetting.Value;
            _userRepository = userRepository;
        }

        [Route("Token")]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] LoginInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userRepository
                .GetEntityAsync(x => x.UserName == input.UserName
                                   && x.Password == EncryptHelper.AesEncrypt(_configuration["EncryptionKey"],input.Password));
            if (user==null)
            {
                return BadRequest();
            }
            //这里可自定义申明信息，生成的token登录后可以获取到Claim信息
            var claim = new Claim[]
            {
                new Claim("id", user.Id), //ClaimTypes.NameIdentifier
                new Claim("name", user.UserName), //ClaimTypes.Name
                new Claim("userNo", "0001"), //todo-stwhh 需要根据自己需要修改
                //new Claim("role", "admin"), //
                //new Claim("email", "admin@qq.com")
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