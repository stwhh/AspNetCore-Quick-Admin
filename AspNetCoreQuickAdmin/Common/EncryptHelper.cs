using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
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
