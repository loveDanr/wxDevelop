using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using WeiXinApi.Model;
using WeiXinApi.Util;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Script.Serialization;
public class OAuth_Token
{
    public OAuth_Token()
    {

        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    //access_token	网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
    //expires_in	access_token接口调用凭证超时时间，单位（秒）
    //refresh_token	用户刷新access_token
    //openid	用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
    //scope	用户授权的作用域，使用逗号（,）分隔
    public string access_token { get; set; }
    public string expires_in { get; set; }
    public string refresh_token { get; set; }
    public string openid { get; set; }
    public string scope { get; set; }

}
public class OAuthUser
{
    public OAuthUser()
    { }
    #region 数据库字段
    private string _openID;
    private string _searchText;
    private string _nickname;
    private string _sex;
    private string _province;
    private string _city;
    private string _country;
    private string _headimgUrl;
    private string _privilege;
    #endregion

    #region 字段属性
    /// <summary>
    /// 用户的唯一标识
    /// </summary>
    public string openid
    {
        set { _openID = value; }
        get { return _openID; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string SearchText
    {
        set { _searchText = value; }
        get { return _searchText; }
    }
    /// <summary>
    /// 用户昵称 
    /// </summary>
    public string nickname
    {
        set { _nickname = value; }
        get { return _nickname; }
    }
    /// <summary>
    /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知 
    /// </summary>
    public string sex
    {
        set { _sex = value; }
        get { return _sex; }
    }
    /// <summary>
    /// 用户个人资料填写的省份
    /// </summary>
    public string province
    {
        set { _province = value; }
        get { return _province; }
    }
    /// <summary>
    /// 普通用户个人资料填写的城市 
    /// </summary>
    public string city
    {
        set { _city = value; }
        get { return _city; }
    }
    /// <summary>
    /// 国家，如中国为CN 
    /// </summary>
    public string country
    {
        set { _country = value; }
        get { return _country; }
    }
    /// <summary>
    /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
    /// </summary>
    public string headimgurl
    {
        set { _headimgUrl = value; }
        get { return _headimgUrl; }
    }
    /// <summary>
    /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）其实这个格式称不上JSON，只是个单纯数组
    /// </summary>
    public string privilege
    {
        set { _privilege = value; }
        get { return _privilege; }
    }
    #endregion
}

namespace WeiXinApi.WebUI
{
    public partial class AllListPage : System.Web.UI.Page
    {
        //用户id
        public string openid = "";

        //公众号信息部分
        public string appid = AppIDCode.Appid;
        public string appsecret = AppIDCode.Appsecret;
        public string redirect_uri = System.Web.HttpUtility.UrlEncode("http://eb7dvc.natappfree.cc/WebUI/OAuthRedirectUri.aspx");
        public string scope = "snsapi_userinfo";

        #region 显示页面
        public string accesstoken;
        public string nickname;
        public string sex;
        public string headimgurl;
        public string province;
        public string country;
        public string language;
        public string city;

        public string privilege = "";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString.Get("code")))
                {
                    string Code = Request.QueryString.Get("code");
                    //获得Token
                    OAuth_Token Model = Get_token(Code);
                    //Response.Write(Model.access_token);
                    OAuthUser OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);
                    Response.Write("用户OPENID:" + OAuthUser_Model.openid + "<br>用户昵称:" + OAuthUser_Model.nickname + "<br>性别:" + OAuthUser_Model.sex + "<br>所在省:" + OAuthUser_Model.province + "<br>所在市:" + OAuthUser_Model.city + "<br>所在国家:" + OAuthUser_Model.country + "<br>头像地址:" + OAuthUser_Model.headimgurl + "<br>用户特权信息:" + OAuthUser_Model.privilege);

                }
            }
        }
        //获得Token
        protected OAuth_Token Get_token(string Code)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppIDCode.Appid + "&secret=" + appsecret + "&code=" + Code + "&grant_type=authorization_code");
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }
        //刷新Token
        protected OAuth_Token refresh_token(string REFRESH_TOKEN)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + AppIDCode.Appid + "&grant_type=refresh_token&refresh_token=" + REFRESH_TOKEN);
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }
        //获得用户信息
        protected OAuthUser Get_UserInfo(string REFRESH_TOKEN, string OPENID)
        {
            // Response.Write("获得用户信息REFRESH_TOKEN:" + REFRESH_TOKEN + "||OPENID:" + OPENID);
            string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID + "&lang=zh_CN");
            OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
            return OAuthUser_Model;
        }
        protected string GetJson(string url)
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
        /*
         * 接入入口
         * 开放到微信菜单中调用
         * @param $dir_url 来源url
         * @since 1.0
         * @return void
         */
        /// <summary>
        ///用code换取获取用户信息（包括非关注用户的）
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="Appsecret"></param>
        /// <param name="Code">回调页面带的code参数</param>
        /// <returns>获取用户信息（json格式）</returns>
        public string GetUserInfo(string Appid, string Appsecret, string Code)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Appid, Appsecret, Code);
            string ReText = FunctionAll.WebRequestPostOrGet(url, "");//post/get方法获取信息
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
            if (!DicText.ContainsKey("openid"))
            {
                LogHelper.WriteLog("获取openid失败，错误码：" + DicText["errcode"].ToString());
                return "";
            }
            else
            {
                System.Web.HttpContext.Current.Session["Oauth_Token"] = DicText["access_token"];
                System.Web.HttpContext.Current.Session.Timeout = 7200;
                return FunctionAll.WebRequestPostOrGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + DicText["access_token"] + "&openid=" + DicText["openid"] + "&lang=zh_CN", "");
            }
        }
    }
}