using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for JsonHelper
/// </summary>
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Xml;
using System.Net;


public class JsonHelper
{
    /// <summary>
    /// 生成Json格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetJson<T>(T obj)
    {
        DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
        using (MemoryStream stream = new MemoryStream())
        {
            json.WriteObject(stream, obj);
            string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
        }
    }
    /// <summary>
    /// 从链接获取返回的json数据
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetJsonFromUrl(string url)
    {
        WebClient wc = new WebClient();
        wc.Credentials = CredentialCache.DefaultCredentials;
        wc.Encoding = Encoding.UTF8;
        string returnText = wc.DownloadString(url);

        if (returnText.Contains("errcode"))
        {
            //可能发生错误
        }
        //Response.Write(returnText);
        return returnText;
    }

    /// <summary>
    /// 获取Json的Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="szJson"></param>
    /// <returns></returns>
    public static T ParseFromJson<T>(string szJson)
    {
        T obj = Activator.CreateInstance<T>();
        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            return (T)serializer.ReadObject(ms);
        }
    }
    /// <summary>
    /// JSON文本转对象,泛型方法
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="jsonText">JSON文本</param>
    /// <returns>指定类型的对象</returns>
    public static T JSONToObject<T>(string jsonText)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        try
        {
            return jss.Deserialize<T>(jsonText);
        }
        catch (Exception ex)
        {
            throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
        }
    }
    /// <summary>
    /// 将JSON文本转换成数据行
    /// </summary>
    /// <param name="jsonText">JSON文本</param>
    /// <returns>数据行的字典</returns>
    public static Dictionary<string, object> DataRowFromJSON(string jsonText)
    {
        return JSONToObject<Dictionary<string, object>>(jsonText);
    }
    /// <summary>
    /// json字符串转换为Xml对象
    /// </summary>
    /// <param name="sJson"></param>
    /// <returns></returns>
    public static XmlDocument Json2Xml(string sJson)
    {
        //XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(sJson), XmlDictionaryReaderQuotas.Max);
        //XmlDocument doc = new XmlDocument();
        //doc.Load(reader);

        JavaScriptSerializer oSerializer = new JavaScriptSerializer();
        Dictionary<string, object> Dic = (Dictionary<string, object>)oSerializer.DeserializeObject(sJson);
        XmlDocument doc = new XmlDocument();
        XmlDeclaration xmlDec;
        xmlDec = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
        doc.InsertBefore(xmlDec, doc.DocumentElement);
        XmlElement nRoot = doc.CreateElement("root");
        doc.AppendChild(nRoot);
        foreach (KeyValuePair<string, object> item in Dic)
        {
            XmlElement element = doc.CreateElement(item.Key);
            KeyValue2Xml(element, item);
            nRoot.AppendChild(element);
        }
        return doc;
    }

    private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> Source)
    {
        object kValue = Source.Value;
        if (kValue.GetType() == typeof(Dictionary<string, object>))
        {
            foreach (KeyValuePair<string, object> item in kValue as Dictionary<string, object>)
            {
                XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                node.AppendChild(element);
            }
        }
        else if (kValue.GetType() == typeof(object[]))
        {
            object[] o = kValue as object[];
            for (int i = 0; i < o.Length; i++)
            {
                XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                KeyValuePair<string, object> item = new KeyValuePair<string, object>("Item", o);
                KeyValue2Xml(xitem, item);
                node.AppendChild(xitem);
            }

        }
        else
        {
            XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
            node.AppendChild(text);
        }
    }
}