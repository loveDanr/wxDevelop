using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using WeiXinApi.Util;
using WeiXinApi.Model;
using System.Text;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace WeiXinApi.Util
{
    
        
    /// <summary>
    /// 基本上所有的功能性写在这里
    /// </summary>
    public class FunctionAll
    {
        /// <summary>
        /// 获取post请求数据
        /// </summary>
        /// <returns></returns>
        private string PostInput()
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            return Encoding.UTF8.GetString(b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public wxmessage GetWxMessage(XmlDocument doc)
        {
            wxmessage wxModel = new wxmessage();
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc,"FromUserName")))
            {
                wxModel.FromUserName = WeiXinXML.GetFromXML(doc,"FromUserName");
            }
            else
            {
                wxModel.FromUserName = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "ToUserName")))
            {
                wxModel.ToUserName = WeiXinXML.GetFromXML(doc, "ToUserName");
            }
            else
            {
                wxModel.ToUserName = string.Empty;
            }
            wxModel.CreateTime = WeiXinXML.GetFromXML(doc, "CreateTime");
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "MsgType")))
            {
                wxModel.MsgType = WeiXinXML.GetFromXML(doc, "MsgType");
            }
            else
            {
                wxModel.MsgType = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Event")))
            {
                wxModel.EventName = WeiXinXML.GetFromXML(doc, "Event");
            }
            else
            {
                wxModel.EventName = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Content")))
            {
                wxModel.Content = WeiXinXML.GetFromXML(doc, "Content");
            }
            else
            {
                wxModel.Content = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Recognition")))
            {
                wxModel.Recognition = WeiXinXML.GetFromXML(doc, "Recognition");
            }
            else
            {
                wxModel.Recognition = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "MediaId")))
            {
                wxModel.MediaId = WeiXinXML.GetFromXML(doc, "MediaId");
            }
            else
            {
                wxModel.MediaId = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "EventKey")))
            {
                wxModel.EventKey = WeiXinXML.GetFromXML(doc, "EventKey");
            }
            else
            {
                wxModel.EventKey = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Location_X")))
            {
                wxModel.Location_X = WeiXinXML.GetFromXML(doc, "Location_X");
            }
            else
            {
                wxModel.Location_X = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Location_Y")))
            {
                wxModel.Location_Y = WeiXinXML.GetFromXML(doc, "Location_Y");
            }
            else
            {
                wxModel.Location_Y = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Scale")))
            {
                wxModel.Scale = WeiXinXML.GetFromXML(doc, "Scale");
            }
            else
            {
                wxModel.Scale = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Label")))
            {
                wxModel.Label = WeiXinXML.GetFromXML(doc, "Label");
            }
            else
            {
                wxModel.Label = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Latitude")))
            {
                wxModel.Latitude = WeiXinXML.GetFromXML(doc, "Latitude");
            }
            else
            {
                wxModel.Latitude = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Longitude")))
            {
                wxModel.Longitude = WeiXinXML.GetFromXML(doc, "Longitude");
            }
            else
            {
                wxModel.Longitude = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Precision")))
            {
                wxModel.Precision = WeiXinXML.GetFromXML(doc, "Precision");
            }
            else
            {
                wxModel.Precision = string.Empty;
            }
            return wxModel;


        }
        /// <summary>
        /// 可以通用，获取菜单
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                return string.Empty;
            }
        }
        /// <summary>
        /// 菜单项目
        /// </summary>
        public void MyMenu()
        {
            string weixin1 = "";
            weixin1 = @" {
     ""button"":[
     {	
          ""type"":""click"",
          ""name"":""你好!"",
          ""key"":""HELLO""
      },
      {
           ""type"":""click"",
           ""name"":""我的简介"",
           ""key"":""myprofile""
      },
      {
           ""name"":""二级菜单"",
           ""sub_button"":[
            {
               ""type"":""view"",
               ""name"":""预约挂号"",
                ""url"":""http://eb7dvc.natappfree.cc/WebUI/OAuthRedirectUri.aspx""
            },
            {
               ""type"":""click"",
               ""name"":""疾控中心"",
               ""key"":""jkzx""
            }]
       }]
 }
"; 
            //string filepath = HttpContext.Current.Server.MapPath("menu.txt");
            //StreamReader sr = new StreamReader(filepath, Encoding.Default);
            //string line = sr.ReadToEnd();
            Access_token model = new Access_token();
            model = GetAccess_token();
            GetPage("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + model.access_token + "", weixin1);
        }
        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="redirect_uri"></param>
        /// <returns></returns>
        public static string GetCodeUrl()
        {
            string redirect_uri = System.Web.HttpUtility.UrlEncode("http://eb7dvc.natappfree.cc/WebUI/AllListPage.aspx");
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state=STATE#wechat_redirect", AppIDCode.Appid, redirect_uri, "snsapi_userinfo");
        }
        #region Post/Get提交调用抓取
        /// <summary>
        /// Post/get 提交调用抓取
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="param">参数</param>
        /// <returns>string</returns>
        public static string WebRequestPostOrGet(string sUrl, string sParam)
        {
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);

            Uri uriurl = new Uri(sUrl);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + (url.IndexOf("?") > -1 ? "" : "?") + param);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bt.Length;

            using (Stream reqStream = req.GetRequestStream())//using 使用可以释放using段内的内存
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Flush();
            }
            try
            {
                using (WebResponse res = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理 

                    Stream resStream = res.GetResponseStream();

                    StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);

                    string resLine;

                    System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();

                    while ((resLine = resStreamReader.ReadLine()) != null)
                    {
                        resStringBuilder.Append(resLine + System.Environment.NewLine);
                    }

                    resStream.Close();
                    resStreamReader.Close();

                    return resStringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;//url错误时候回报错
            }
        }
        #endregion Post/Get提交调用抓取
        ///// <summary>
        ///// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
        ///// </summary>
        ///// <param name="datetime"></param>
        ///// <returns></returns>
        //public static string IsExistAccess_Token()
        //{

        //    string Token = string.Empty;
        //    DateTime YouXRQ;
        //    // 读取XML文件中的数据，并显示出来 ，注意文件路径
        //    ;

        //    StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
        //    XmlDocument xml = new XmlDocument();
        //    xml.Load(str);
        //    str.Close();
        //    str.Dispose();
        //    Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
        //    YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);

        //    if (DateTime.Now > YouXRQ)
        //    {
        //        DateTime _youxrq = DateTime.Now;
        //        Access_token mode = GetAccess_token();
        //        xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
        //        _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
        //        xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
        //        xml.Save(filepath);
        //        Token = mode.access_token;
        //    }
        //    return Token;
        //}
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static Access_token GetAccess_token()
        {
            //string appid = "wx3fd9b86495ce9609";
            //string secret = "6f6a79781e36f7c69e48533f209bc226";
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppIDCode.Appid+ "&secret=" + AppIDCode.Appsecret;
            Access_token mode = new Access_token();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                string content = reader.ReadToEnd();
                //Response.Write(content);
                //在这里对Access_token 赋值
                Access_token token = new Access_token();
                token = JsonHelper.ParseFromJson<Access_token>(content);
                mode.access_token = token.access_token;
                mode.expires_in = token.expires_in;
            }
            return mode;
        }

        

    }
}