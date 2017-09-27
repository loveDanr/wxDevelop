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
using System.Xml.Serialization;
using System.Net.Http;
using System.Web.UI;
using System.Configuration;
using System.Web.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WeiXinApi.Util
{


    /// <summary>
    /// 基本上所有的功能性写在这里
    /// </summary>
    public class FunctionAll
    {
        #region 获取post请求数据
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
        #endregion

        #region Post/Get提交调用抓取
        /// <summary>
        /// 给微信消息赋值
        /// </summary>
        /// <param name="root"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public wxmessage GetWxMessage(XmlDocument doc)
        {
            wxmessage wxModel = new wxmessage();
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "FromUserName")))
            {
                wxModel.FromUserName = WeiXinXML.GetFromXML(doc, "FromUserName");
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
        #endregion

        #region 可以通用，获取菜单
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
        #endregion

        #region 自定义菜单方法
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
                               ""name"":""谁简介"",
                               ""key"":""myprofile""
                           },
                           {
                               ""name"":""测试菜单"",
                               ""sub_button"":[
                                {
                                   ""type"":""view"",
                                   ""name"":""全程陪诊"",
                                   ""url"":""http://543av8.natappfree.cc/WebUI/Index.aspx""
                                },
                                {
                                   ""type"":""view"",
                                   ""name"":""全程陪诊H5"",
                                   ""url"":""" + GetCodeUrl("http://543av8.natappfree.cc/vip-c/index.html") + @"""
                                },
                                {
                                   ""type"":""click"",
                                   ""name"":""疾控中心"",
                                   ""key"":""jkzx""
                                }
                           }]
                         }";

            //string filepath = HttpContext.Current.Server.MapPath("menu.txt");
            //StreamReader sr = new StreamReader(filepath, Encoding.Default);
            //string line = sr.ReadToEnd();
            Access_token model = new Access_token();
            model.access_token = HttpRequestUtil.GetAccessToken();//将access_token进行缓存
            //model = GetAccess_token();
            //GetPage("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + model.access_token + "", ""); //删除菜单
            GetPage("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + model.access_token + "", weixin1);
        }
        #endregion

        #region 跳转打开页面授权登录
        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="redirect_uri"></param>
        /// <returns></returns>
        public static string GetCodeUrl(string redirect_uri)
        {
            string result = string.Empty;     
            result =  string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppIDCode.Appid + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect");
            return result;
        }
        #endregion

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

        #region 获取token
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static Access_token GetAccess_token()
        {
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppIDCode.Appid + "&secret=" + AppIDCode.Appsecret;
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
        #endregion

        /// <summary>
        /// 给指定的用户发送模板消息
        /// </summary>
        /// <param name="openId">用户标识openid</param>
        /// <param name="templateId">对应的模板id</param>
        /// <param name="data">对应模板的参数</param>
        /// <param name="url">点击对应消息弹出的地址</param>
        /// <returns>返回json数据包</returns>
        public string SendTemplate(string openId, string token, string templateId, object data, string url)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            var msgData = new
            {
                touser = openId,
                template_id = templateId,
                url = url,
                data = data
            };
            string postData = Jss.Serialize(msgData);
            return Post("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token, postData);
        }


        //Post数据到指定Url，并返回String类型
        public string Post(string Url, string Data)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(Data);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = requestBytes.Length;
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
            string PostJie = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return PostJie;
        }

        #region 用cookie存储登录用户信息的方法
        /// <summary>
        /// 用户登陆成功后，发放表单cookie验证票据并记录用户的相关信息
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="userName">与票证关联的用户名</param>
        /// <param name="Page">页面对象</param>
        /// <param name="expiration">FormsAuthenticationTicket过期时间</param> 
        /// <param name="userInfo">要保存在cookie中用户对象</param>
        public static void UserLoginSetCookie<T>(string userName, HttpContext context, DateTime expiration, T userInfo)
        {
            Configuration conn = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            System.Web.Configuration.AuthenticationSection section = (System.Web.Configuration.AuthenticationSection)conn.SectionGroups.Get("system.web").Sections.Get("authentication");
            expiration = expiration.AddMinutes(section.Forms.Timeout.TotalMinutes);

            //将对象序列化成字符串
            string strUser = JsonHelper.SerializeObject(userInfo);

            // 设置票据Ticket信息
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, expiration, false, strUser);

            string strTicket = FormsAuthentication.Encrypt(ticket);// 加密票据

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strTicket);// 使用新userdata保存cookie
            //cookie.Expires = ticket.Expiration;//将票据的过期时间和Cookie的过期时间同步，避免因两者的不同所产生的矛盾

            context.Response.Cookies.Add(cookie);
        }
        #endregion

        #region 用cookie存储登录用户信息的方法
        /// <summary>
        /// 用户登陆成功后，发放表单cookie验证票据并记录用户的相关信息
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="userName">与票证关联的用户名</param>
        /// <param name="Page">页面对象</param>
        /// <param name="expiration">FormsAuthenticationTicket过期时间</param> 
        /// <param name="userInfo">要保存在cookie中用户对象</param>
        public static void UserLoginSetCookie<T>(string userName, Page page, DateTime expiration, T userInfo)
        {
            Configuration conn = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            System.Web.Configuration.AuthenticationSection section = (System.Web.Configuration.AuthenticationSection)conn.SectionGroups.Get("system.web").Sections.Get("authentication");
            expiration = expiration.AddMinutes(section.Forms.Timeout.TotalMinutes);

            //将对象序列化成字符串
            string strUser = JsonHelper.SerializeObject(userInfo);

            // 设置票据Ticket信息
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, expiration, false, strUser);

            string strTicket = FormsAuthentication.Encrypt(ticket);// 加密票据

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strTicket);// 使用新userdata保存cookie
            //cookie.Expires = ticket.Expiration;//将票据的过期时间和Cookie的过期时间同步，避免因两者的不同所产生的矛盾

            page.Response.Cookies.Add(cookie);
        }
        #endregion



        /// <summary>
        /// 获得保存在cookie中的用户对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="page">页面对象</param>
        /// <returns>保存在cookie中的用户对象</returns>
        public static T GetCookieUserData<T>(Page page)
        {
            if (page != null)
            {
                if (page.User.Identity.IsAuthenticated)//如果没验证通过的话,强制类型转换会出错!~
                {
                    string strUser = ((FormsIdentity)page.User.Identity).Ticket.UserData;
                    return JsonHelper.DeserializeObject<T>(strUser); 
                }
                else
                    return default(T);
            }
            else
                throw new ApplicationException("page == null");
        }

        /// <summary>
        /// 获得保存在cookie中的用户对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="page">页面对象</param>
        /// <returns>保存在cookie中的用户对象</returns>
        public static T GetCookieUserData<T>(HttpContext context)
        {
            if (context != null)
            {
                if (context.User.Identity.IsAuthenticated)//如果没验证通过的话,强制类型转换会出错!~
                {
                    string strUser = ((FormsIdentity)context.User.Identity).Ticket.UserData;
                    return JsonHelper.DeserializeObject<T>(strUser);
                }
                else
                    return default(T);
            }
            else
                throw new ApplicationException("page == null");
        }
    }
}