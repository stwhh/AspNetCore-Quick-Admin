using System;
using System.Security.Cryptography;
using System.Text;

namespace QuickAdmin.Common
{
    /// <summary>
    /// 加解密类
    /// </summary>
    public static class EncryptHelper
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="aesKey">密钥</param>
        /// <param name="originData">待加密的原始数据</param>
        /// <returns></returns>
        public static string AesEncrypt(string aesKey, string originData)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(aesKey);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(originData);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            //对称加密和分组加密中的四种模式(ECB、CBC、CFB、OFB),这三种的区别，主要来自于密钥的长度，16位密钥 = 128位，24位密钥 = 192位，32位密钥 = 256位
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="aesKey">密钥</param>
        /// <param name="encryptData">待解密的数据</param>
        /// <returns></returns>
        public static string AesDecrypt(string aesKey, string encryptData)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(aesKey);
            byte[] toEncryptArray = Convert.FromBase64String(encryptData);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
