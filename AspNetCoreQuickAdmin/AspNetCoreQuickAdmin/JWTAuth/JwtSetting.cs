using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreQuickAdmin.JWTAuth
{
    /// <summary>
    /// JWT设置类
    /// </summary>
    public class JwtSetting
    {
        /// <summary>
        /// Token发布人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token给哪些人颁布的
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 加密的key，必须大于16位
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 有效期，
        /// </summary>
        public int ExpiresInMinute { get; set; }
    }
}
