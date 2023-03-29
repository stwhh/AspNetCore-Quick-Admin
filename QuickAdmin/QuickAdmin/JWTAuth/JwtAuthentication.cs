using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace QuickAdmin.JWTAuth;

public static class JwtAuthentication
{

	public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services,
		IConfiguration configuration)
	{
		var jwtSetting = configuration.GetSection("JwtSettings");

		return services.AddAuthentication(options =>
		{
			//认证middleware配置
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(o =>
		{
			o.TokenValidationParameters = new TokenValidationParameters
			{
				//Token颁发机构
				ValidIssuer = jwtSetting["Issuer"],
				//颁发给谁
				ValidAudience = jwtSetting["Audience"],
				//这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["SecretKey"])),
				//是否验证key
				ValidateIssuerSigningKey = true,
				//是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
				ValidateLifetime = true,
				//允许的服务器时间偏移量
				ClockSkew = TimeSpan.Zero
			};
		});
	}
}