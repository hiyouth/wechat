using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeChatLib;

namespace WeChat
{
    //用于将微信企业号同indexer账号进行绑定
    public class WeChatController : Controller
    {
        public ActionResult Index()
        {
            return View("WeChat");
        }

        /// <summary>
        /// 团队接入微信号时第一步，验证团队的corpid以及secret
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        public String VerifyCorp(string corpid, string corpsecret)
        {
            //试用版本微信企业号：corpid=wx0beba9277d2499d0&corpsecret=T1XBGAti9ycGk2Isc6r1ThMMH2MB-cVEC5WCTd3PocLLds7uPqLKrADGsxwjGW1N
            string uri = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret;
            HttpUtil http = new HttpUtil();
            string res = http.SendGet(uri);

            AccessToken at = JsonConvert.DeserializeObject<AccessToken>(res);
            if (!String.IsNullOrEmpty(at.access_token))
            {
                //TODO:将accesstoken存入数据库，同团队关联
                return "Success";
            }
            else
            {
                //  AccessTokenError error = JsonConvert.DeserializeObject<AccessTokenError>(res);
                return res;
            }
        }

        /// <summary>
        /// 第二步同步成员，将微信企业号上的成员同indexer上的成员绑定
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public int SyncMember(string teamId)
        {
            //在第一步中已经将团队id同企业的accesstoken绑定，需要拿到teamId去数据库中寻找

            string access_token = "";
            access_token = "ph7y-PEr9def-ZRt41gWTFTk6QGXV5iCEMIHbcV3DOc";
            //如果没有找到，则返回提示-1表示团队账号还没有同微信企业号绑定
            if (String.IsNullOrEmpty(access_token))
            {
                return -1;
            }
            //如果该团队已经绑定了微信企业号，则取到token
            return GetDepartMem(access_token);
        }

        private int GetDepartMem(string access_token)
        {
            //status=1表示只取关注了企业微信的成员
            string uri = "https://qyapi.weixin.qq.com/cgi-bin/user/simplelist?access_token=" +
             access_token + "&department_id=1&fetch_child=1&status=1";

            HttpUtil http = new HttpUtil();
            string res = http.SendGet(uri);
            if (res == "error")
            {
                return -1;
            }
            DepartMems mems = JsonConvert.DeserializeObject<DepartMems>(res);

            if (mems.errcode != 0)
            {
                return -1;
            }
            else
            {
                HttpUtil httpUitl = new HttpUtil();

                List<UserInfo> userInfos = new List<UserInfo>();
                foreach (var item in mems.userlist)
                {
                    //微信这里只能一次取一个user的资料
                    string getUserInfoUri = "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token=" +
                    access_token + "&userid=" + item.userid;
                    string info = httpUitl.SendGet(getUserInfoUri);
                    if (info != "error")
                    {
                        UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(info);
                        userInfos.Add(userInfo);
                    }
                    else
                    {
                        //如果任何一个成员的资料加载失败，则返回错误。
                        return -1;
                    }
                }
                if (userInfos.Count != 0)
                {
                    //这里对比UserInfo中的email属性同数据库中团队成员的email，匹配则存入
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}