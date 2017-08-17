using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinApi.Model
{    
    //海口互联网医院 
    //appid = "wx7c90b55d449081c4";
    //正式开发者密码 1bcf0967b6e4eeeeef1549be48db2c6b
    //加密钥匙VCZuOnYCPVf1pDUg7VPLd8q57WzlJQEfsedHVkzhzhp

    //测试的appid    wx3fd9b86495ce9609     开发者密码6f6a79781e36f7c69e48533f209bc226  密钥j8XRu1TG0egoKhdKngbGI886R9ooPMIhoUZPOdhp9GL
    /// <summary>
    /// 全局设置appid和appsecret密码
    /// </summary>
    public class AppIDCode
    {
        private static string appid = "wx3fd9b86495ce9609";
        private static string appsecret = "6f6a79781e36f7c69e48533f209bc226";
        private static string aesKey = "j8XRu1TG0egoKhdKngbGI886R9ooPMIhoUZPOdhp9GL";
        private static string key = "key";
        /// <summary>
        /// 验证时用的key
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }
        /// <summary>
        /// 加密解密用的key
        /// </summary>
        public static string AesKey
        {
            get { return aesKey; }
            set { aesKey = value; }
        }
        /// <summary>
        /// appid
        /// </summary>
        public static string Appid {
            get { return appid; }
            set { appid = value; } 
        }

        /// <summary>
        /// app密钥
        /// </summary>
        public static string Appsecret
        {
            get { return appsecret; }
            set { appsecret = value; } 
        }
    }
}