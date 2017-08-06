using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using WeiXinApi.Util;
namespace WeiXinApi
{
    /// <summary>
    /// weixinapi 的摘要说明
    /// </summary>
    public class weixinapi : IHttpHandler
    {
        //三个加密字段
        string Token = "key";
        /// <summary>
        /// AppId 要与 微信公共平台 上的 AppId 保持一致
        /// </summary>
        string AppId = "wx4e9ab80a5ee0e214";
        /// <summary>
        /// 加密用 
        /// </summary>
        string AESKey = "UU9ETyPbqgva4UP164HEJipLpCKY47TzFFsM1za7GH5";


        public void ProcessRequest(HttpContext context)
        {
            
            try
            {
                Stream stream = context.Request.InputStream;
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, (int)stream.Length);
                string postXmlStr = System.Text.Encoding.UTF8.GetString(byteArray);
                if (!string.IsNullOrEmpty(postXmlStr))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(postXmlStr);
                    //if (string.IsNullOrWhiteSpace(AppId))
                    //{
                    //    //DataTable dt = ConfigDal.GetConfig(WXMsgUtil.GetFromXML(doc, "ToUserName"));
                    //    //DataRow dr = dt.Rows[0];
                    //    //sToken = dr["Token"].ToString();
                    //    //sAppID = dr["AppID"].ToString();
                    //    //sEncodingAESKey = dr["EncodingAESKey"].ToString();
                      

                    //}
                  
                    //if (!string.IsNullOrWhiteSpace(sAppID))  //没有AppID则不解密(订阅号没有AppID)
                    //{
                    //    //解密
                    //    WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                    //    string signature = context.Request["msg_signature"];
                    //    string timestamp = context.Request["timestamp"];
                    //    string nonce = context.Request["nonce"];
                    //    string stmp = "";
                    //    int ret = wxcpt.DecryptMsg(signature, timestamp, nonce, postXmlStr, ref stmp);
                    //    if (ret == 0)
                    //    {
                    //        doc = new XmlDocument();
                    //        doc.LoadXml(stmp);

                    //        try
                    //        {
                    //            responseMsg(context, doc);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            FileLogger.WriteErrorLog(context, ex.Message);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        FileLogger.WriteErrorLog(context, "解密失败，错误码：" + ret);
                    //    }
                    //}
                    //else
                    //{
                    //    responseMsg(context, doc);
                    //}
                    string result;
                    var rootElement = doc.DocumentElement;
                    if (rootElement == null)
                    {
                        return;
                    }
                    else
                    {
                        //获取用户发来的信息
                        string msgType =WeiXinXML.GetFromXML(doc, "MsgType");
                        switch (msgType)
                        {
                            case "text"://文本
                                string Content = rootElement.SelectSingleNode("Content").InnerText;
                                result = WeiXinXML.CreateTextMsg(doc, Content);
                                LogHelper.WriteLog(result);
                                break;

                            default:
                                break;
                        }
                    }

                  //回复消息
                    responseMsg(context, doc);
                    
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
            var token = "tielizi";
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
       

        public void responseMsg(HttpContext context, XmlDocument xmlDoc)
        {
            string fromUser =WeiXinXML.GetFromXML(xmlDoc, "FromUserName");
            string toUser = WeiXinXML.GetFromXML(xmlDoc, "ToUserName");
            string result = "";
            string msgType = WeiXinXML.GetFromXML(xmlDoc, "MsgType");
            switch (msgType)
            {
                case "event":
                    switch (WeiXinXML.GetFromXML(xmlDoc, "Event"))
                    {
                        case "subscribe": //订阅
                            result = WeiXinXML.subscribeRes(xmlDoc);
                            break;
                        case "unsubscribe": //取消订阅
                            break;
                        case "CLICK":
                           
                            break;
                        default:
                            break;
                    }
                    break;
                case "text":
                    string text = WeiXinXML.GetFromXML(xmlDoc, "Content");
                    if (text !=null)
                    {
                        result = WeiXinXML.CreateTextMsg(xmlDoc, TuLing.GetTulingMsg(text));
                    }
                    else
                    {
                        result = WeiXinXML.CreateTextMsg(xmlDoc, "回复错误！");
                    }
                    
                    break;
                default:
                    break;
            }
            
            /////消息加密
            //if (!string.IsNullOrWhiteSpace(sAppID))
            //{
            //    WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
            //    string sEncryptMsg = ""; //xml格式的密文
            //    string timestamp = context.Request["timestamp"];
            //    string nonce = context.Request["nonce"];
            //    int ret = wxcpt.EncryptMsg(result, timestamp, nonce, ref sEncryptMsg);
            //    if (ret != 0)
            //    {
            //        LogHelper.WriteLog(this.GetType(), context+ "加密失败，错误码：" + ret);
            //        return;
            //    }

            //    context.Response.Write(sEncryptMsg);
            //    context.Response.Flush();
            //}
            //else
            //{ 
               
                context.Response.Write(result);
                context.Response.Flush();
                LogHelper.WriteLog(result);
            //}
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