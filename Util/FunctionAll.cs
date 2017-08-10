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

namespace WeiXinApi.Util
{
    /// <summary>
    /// 基本上所有的功能性写在这里
    /// </summary>
    public class FunctionAll
    {
        public wxmessage GetWxMessage(XmlElement root,wxmessage model)
        {
            //wxmessage model = new wxmessage();

            model.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            model.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            model.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            model.MsgType = root.SelectSingleNode("MsgType").InnerText;
            if (model.MsgType.Trim().ToLower() == "text")
            {
                model.Content = root.SelectSingleNode("Content").InnerText;
            }
            else if (model.MsgType.Trim().ToLower() == "event")
            {
                model.EventName = root.SelectSingleNode("Event").InnerText;
            }
            return model;


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
                ""url"":""https://www.guahao.com/""
            },
            {
               ""type"":""click"",
               ""name"":""疾控中心"",
               ""key"":""jkzx""
            }]
       }]
 }
";
            Access_token model = new Access_token();
            model = GetAccess_token();
            GetPage("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + model.access_token + "", weixin1);
        }
        /// <summary>
        /// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string IsExistAccess_Token()
        {

            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            string filepath = HttpContext.Current.Server.MapPath("XMLFile.xml");

            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);

            if (DateTime.Now > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();
                xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                xml.Save(filepath);
                Token = mode.access_token;
            }
            return Token;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static Access_token GetAccess_token()
        {
            string appid = "wx3fd9b86495ce9609";
            string secret = "6f6a79781e36f7c69e48533f209bc226";
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
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