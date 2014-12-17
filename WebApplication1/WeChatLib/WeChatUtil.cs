using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WeChat;
using WeChatLib;

namespace WeChatLib
{
    public class WeChatUtil
    {
        public String GetToken(string corpid,string corpsecret)
        {
            string uri =
             " https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" +
             corpid +
             "&corpsecret=" +
             corpsecret;
            HttpUtil util = new HttpUtil();
            string res = util.SendGet(uri);
            AccessToken at = JsonConvert.DeserializeObject<AccessToken>(res);
            return at.access_token;
        }
    }
}