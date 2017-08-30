using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
namespace WeiXinApi.Util
{
    public class LogHelper
    {
        
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void WriteLog(string Message)
        {
            try
            {
                string path = "~/Log/" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\n日志信息 :");
                    w.WriteLine("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    w.WriteLine(Message);
                    w.WriteLine("________________________________________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 写txt
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void WriteTXT(string Message)
        {
            try
            {
                string path = "~/" + "accessToken"+ ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(path), FileMode.Open, FileAccess.Write);
                fs.SetLength(0);
                fs.Close();
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine(Message);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// 读取txt
        /// </summary>
        /// <param name="errorMessage"></param>
        public  static string ReadTXT()
        {
            string str = "";
            try
            {
               string path = "~/" + "accessToken" + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamReader w =new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                  str = w.ReadLine();
                  w.Close();
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }
            return str;
        }
        /// <summary>
        /// 写xml日志
        /// </summary>
        /// <param name="XmlDoc"></param>
        public static void WriteXMLLog(XmlDocument XmlDoc)
        {
            try
            {
                string path = "~/Log/" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\n日志信息 :");
                    w.WriteLine("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    w.WriteLine(XmlDoc);
                    w.WriteLine("________________________________________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }
        }
       
    }
}