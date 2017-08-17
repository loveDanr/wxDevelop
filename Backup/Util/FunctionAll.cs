using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using WeiXinApi.Util;
using WeiXinApi.Model;

namespace WeiXinApi.Util
{
    /// <summary>
    /// 基本上所有的功能性写在这里
    /// </summary>
    public class FunctionAll
    {
        private readonly wxmessage wx = new wxmessage();

        /// <summary>
        /// 获取消息的各个字段
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public void  GetMsgModel(XmlDocument xmlDoc)
        {
            wx.FromUserName = WeiXinXML.GetFromXML(xmlDoc,"FromUserName");
            wx.ToUserName = WeiXinXML.GetFromXML(xmlDoc, "ToUserName");
            wx.MsgType = WeiXinXML.GetFromXML(xmlDoc, "MsgType");
            wx.EventName = WeiXinXML.GetFromXML(xmlDoc, "EventName");
            wx.Content = WeiXinXML.GetFromXML(xmlDoc, "Content");
            wx.Recognition = WeiXinXML.GetFromXML(xmlDoc, "Recognition");
            wx.MediaId = WeiXinXML.GetFromXML(xmlDoc, "MediaId");
            wx.EventKey = WeiXinXML.GetFromXML(xmlDoc, "EventKey");
            wx.Location_X = WeiXinXML.GetFromXML(xmlDoc, "Location_X");
            wx.Location_Y = WeiXinXML.GetFromXML(xmlDoc, "Location_Y");
            wx.Scale = WeiXinXML.GetFromXML(xmlDoc, "Scale");
            wx.Label = WeiXinXML.GetFromXML(xmlDoc, "Label");
            wx.Latitude = WeiXinXML.GetFromXML(xmlDoc, "Latitude");
            wx.Longitude = WeiXinXML.GetFromXML(xmlDoc, "Longitude");
            wx.Precision = WeiXinXML.GetFromXML(xmlDoc, "Precision");

            
        }

        
        
        /// <summary>  
        /// 调用百度地图，返回坐标信息  
        /// </summary>  
        /// <param name="y">经度</param>  
        /// <param name="x">纬度</param>  
        /// <returns></returns>  
        public string GetMapInfo(string x, string y)
        {
            try
            {
                string res = string.Empty;
                string parame = string.Empty;
                string url = "http://maps.googleapis.com/maps/api/geocode/xml";

                parame = "latlng=" + x + "," + y + "&language=zh-CN&sensor=false";//此key为个人申请  
                res = webRequestPost(url, parame);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(res);

                XmlElement rootElement = doc.DocumentElement;
                string Status = rootElement.SelectSingleNode("status").InnerText;

                if (Status == "OK")
                {
                    //仅获取城市  
                    XmlNodeList xmlResults = rootElement.SelectSingleNode("/GeocodeResponse").ChildNodes;
                    for (int i = 0; i < xmlResults.Count; i++)
                    {
                        XmlNode childNode = xmlResults[i];
                        if (childNode.Name == "status")
                        {
                            continue;
                        }
                        string city = "0";
                        for (int w = 0; w < childNode.ChildNodes.Count; w++)
                        {
                            for (int q = 0; q < childNode.ChildNodes[w].ChildNodes.Count; q++)
                            {
                                XmlNode childeTwo = childNode.ChildNodes[w].ChildNodes[q];
                                if (childeTwo.Name == "long_name")
                                {
                                    city = childeTwo.InnerText;
                                }
                                else if (childeTwo.InnerText == "locality")
                                {
                                    return city;
                                }
                            }
                        }
                        return city;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                //WriteTxt("map异常:" + ex.Message.ToString() + "Struck:" + ex.StackTrace.ToString());  
                return "0";
            }
            return "0";
        }

        /// <summary>  
        /// Post 提交调用抓取  
        /// </summary>  
        /// <param name="url">提交地址</param>  
        /// <param name="param">参数</param>  
        /// <returns>string</returns>  
        public string webRequestPost(string url, string param)
        {
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + "?" + param);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Flush();
            }

            using (WebResponse wr = req.GetResponse())
            {
                //在这里对接收到的页面内容进行处理  
                Stream strm = wr.GetResponseStream();
                StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);

                string line;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line + System.Environment.NewLine);
                }
                sr.Close();
                strm.Close();
                return sb.ToString();
            }
        }  

    }
}