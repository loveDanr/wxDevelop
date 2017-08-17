using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

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
       
    }
}