using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using WeiXinApi.Util;
using WeiXinApi.Model;
using System.Net;
using System.Web.UI;
namespace WeiXinApi
{
    /// <summary>
    /// weixinapi 的摘要说明
    /// </summary>
    public class weixinapi : IHttpHandler
    {
        //公众号上接口字段
        string sToken = "key";
        /// <summary>
        /// AppId 要与 微信公共平台 上的 AppId 保持一致
        /// </summary>
        string sAppId = "wx3fd9b86495ce9609";
        /// <summary>
        /// 加密用 
        /// </summary>
        string AESKey = "j8XRu1TG0egoKhdKngbGI886R9ooPMIhoUZPOdhp9GL";
        string GlobalpostXmlStr = string.Empty;
        string RequestLocation = String.Empty;
        

        public void ProcessRequest(HttpContext context)
        {
            wxmessage wxGlobal = new wxmessage();
            FunctionAll fuc = new FunctionAll();
            XmlDocument doc = new XmlDocument();
            try
            {
                fuc.MyMenu();
                Stream stream = context.Request.InputStream;
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, (int)stream.Length);
                string postXmlStr = System.Text.Encoding.UTF8.GetString(byteArray);
                if (!string.IsNullOrEmpty(postXmlStr))
                {
                    doc.LoadXml(postXmlStr);
                    XmlElement root = doc.DocumentElement;
                    wxGlobal = fuc.GetWxMessage(root,wxGlobal);
                    switch (wxGlobal.MsgType) 
                    {
                        case "text"://文本
                            wxGlobal.Content = doc.SelectSingleNode("xml").SelectSingleNode("Content").InnerText;
                            break;
                        case "event"://文本
                            wxGlobal.EventName = doc.SelectSingleNode("xml").SelectSingleNode("Event").InnerText;
                            wxGlobal.EventKey = doc.SelectSingleNode("xml").SelectSingleNode("EventKey").InnerText;
                            break;
                        case "location"://文本
                            wxGlobal.Location_X = doc.SelectSingleNode("xml").SelectSingleNode("Location_X").InnerText;
                            wxGlobal.Location_Y = doc.SelectSingleNode("xml").SelectSingleNode("Location_Y").InnerText;
                            break;
                    }
                    string result="";
                    string requestContent = "";
                    //var rootElement = doc.DocumentElement;
                    if (wxGlobal.MsgType == null)
                    {
                        return;
                    }
                    else
                    {
                        //获取用户发来的信息
                        switch (wxGlobal.MsgType)
                        {
                                
                            case "text"://文本
                                requestContent = WeiXinXML.CreateTextMsg(doc, wxGlobal.Content);
                                LogHelper.WriteLog(requestContent);
                                result = WeiXinXML.CreateTextMsg(doc, TuLing.GetTulingMsg(wxGlobal.Content));
                                break;
                            case "location"://文本
                                //LogHelper.WriteXMLLog(doc);
                                result = WeiXinXML.ReArticle(wxGlobal.FromUserName, wxGlobal.ToUserName, "您附近的XXX", "XXXXXXXX", "http://119.29.20.29/image/test.jpg", "http://m.amap.com/around/?locations="+wxGlobal.Location_Y+""+wxGlobal.Location_X+"&keywords=疾控中心&defaultIndex=3&defaultView=map&searchRadius=5000&key=33fe5b1e0fc0023eb1cd28b392d5e70f");
                                break;
                            case "event":
                                switch (wxGlobal.EventName)
                                {
                                    case "subscribe": //订阅
                                        result = WeiXinXML.subscribeRes(doc);
                                        break;
                                    case "unsubscribe": //取消订阅
                                        break;
                                    case "CLICK":

                                        if (wxGlobal.EventKey == "HELLO")
                                            result = WeiXinXML.CreateTextMsg(doc, "微医app - 原名 挂号网，用手机挂号，十分方便！更有医生咨询、智能分诊、院外候诊、病历管理等强大功能。\r\n"+
　　"预约挂号 聚合全国超过900家重点医院的预约挂号资源\r\n"+
  "咨询医生 支持医患之间随时随地图文、语音、视频方式的沟通交流\r\n" +
  "智能分诊 根据分诊自测系统分析疾病类型，提供就诊建议\r\n" +
  "院外候诊 时间自由可控，不再无谓浪费\r\n" +
  "病历管理 病历信息统一管理，个人健康及时监测\r\n" +
  "贴心服务 医疗支付、报告提取、医院地图\r\n" +
  "权威保障 国家卫计委（原卫生部）指定的全国健康咨询及就医指导平台\r\n" +
　　"[微医] 目前用户量大，有些不足我们正加班加点的努力完善，希望大家用宽容的心给 [微医] 一点好评，给我们一点激励，让 [微医] 和大家的健康诊疗共成长。");
                                        if (wxGlobal.EventKey == "myprofile")
                                            result = WeiXinXML.CreateTextMsg(doc, "嘻嘻爸爸就是我！");
                                        if (wxGlobal.EventKey == "jkzx")
                                            result = WeiXinXML.ReArticle(wxGlobal.FromUserName, wxGlobal.ToUserName, "点击图片查看您附近的疾控中心", @"测试测试测试测试", "http://119.29.20.29/image/navi.jpg", "http://m.amap.com/around/?locations=&keywords=疾控中心&defaultIndex=3&defaultView=map&searchRadius=5000&key=33fe5b1e0fc0023eb1cd28b392d5e70f");

                                        break;
                                }
                                
                                break;
                        }
                        context.Response.Write(result);
                        context.Response.Flush();
                        LogHelper.WriteLog("系统回复的明文"+result);
                    }///消息加密
                    if (!string.IsNullOrWhiteSpace(sAppId))
                    {
                        WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, AESKey, sAppId);
                        string sEncryptMsg = ""; //xml格式的密文
                        string timestamp = context.Request["timestamp"];
                        string nonce = context.Request["nonce"];
                        int ret = wxcpt.EncryptMsg(result, timestamp, nonce, ref sEncryptMsg);
                        if (ret != 0)
                        {
                            LogHelper.WriteLog("加密失败，错误码：" + ret);
                            return;
                        }

                        context.Response.Write(sEncryptMsg);
                        context.Response.Flush();
                        LogHelper.WriteLog("系统回复的加密文:" + sEncryptMsg);
                    }
                }
                else
                {
                    valid(context);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);

            }
        }
        public void valid(HttpContext context)
        {
            var echostr = context.Request["echoStr"].ToString();
            if (checkSignature(context) && !string.IsNullOrEmpty(echostr))
            {
                context.Response.Write(echostr);
                context.Response.Flush();       //推送...不然微信平台无法验证token
            }
        }

        /// <summary>
        /// 第一步，公众号开发者验证方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool checkSignature(HttpContext context)
        {
            var signature = context.Request["signature"].ToString();
            var timestamp = context.Request["timestamp"].ToString();
            var nonce = context.Request["nonce"].ToString();
            var token = "key";
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}

   