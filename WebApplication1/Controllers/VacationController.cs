using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace WebApplication1.Controllers
{
    public class VacationController : Controller
    {
        // GET: Vacation
        public ActionResult Index()
        {
            return View();
        }

        //微信企业号中添加一个app时，指定一个app调用接口，这个接口主要用于验证企业号对API的调用是否合法
        public long API(string msg_signature, string timestamp, string nonce, string echostr)
        {
            //stoken,sEncodingAESKey应该是由App提供方来提供，用以验证对于app接口的调用确实是来自于企业
            //微信在调用App接口时，会将sToken、sEncodingAESKey和scorpID信息加密后发到App接口来验证
            //具体到indexer中，在indexer中，添加一个“请假App”时，系统需要随机生成一个sToken和一个sEncodingAESKey
            //并将这两个值同团队和这个团队添加的这个请假App绑定，也就是说每个团队下面的请假app都会对应一个sToken和
            //sEncodingAESKey。如果当前团队下面的请假App不存在时，系统需要生成sToken和sEncodingAESKy，并存入数据库，
            //如果用户需要重新绑定时（比如用户在微信企业号管理中心中删除了这个应用）indexer不需要再生成这2个值，只需要读取出来。
            //此外，在添加App时，还需要用户输入一个“应用ID”，这个ID由用户输入，也需要能够保存到数据库中。
            //最后用户需要点击一个按钮“生成自定义菜单”
            string sToken = "318GgVzYOh9KIRlLEDyriTQxRNQSHUUF";
            string sEncodingAESKey = "lpGRJCYqiZF8aSNOPI9q4nPBm4RrEzoVXUk9bgEKjno";
            string sCorpID = "wx0beba9277d2499d0";

            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);
            // string sVerifyMsgSig = HttpUtils.ParseUrl("msg_signature");
            string sVerifyMsgSig = msg_signature;
            // string sVerifyTimeStamp = HttpUtils.ParseUrl("timestamp");
            string sVerifyTimeStamp = timestamp;
            // string sVerifyNonce = HttpUtils.ParseUrl("nonce");
            string sVerifyNonce = nonce;
            // string sVerifyEchoStr = HttpUtils.ParseUrl("echostr");
            string sVerifyEchoStr = echostr;
            int ret = 0;
            string sEchoStr = "";
            ret = wxcpt.VerifyURL(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, sVerifyEchoStr, ref sEchoStr);
            if (ret != 0)
            {
                System.Console.WriteLine("ERR: VerifyURL fail, ret: " + ret);
                return 0;
            }
            return Convert.ToInt64(sEchoStr);
        }

        //这个接口接收用户的回调信息，并做相应的处理后返回
        [HttpPost]
        public string API(string msg_signature, string timestamp, string nonce)
        {

            string sToken = "318GgVzYOh9KIRlLEDyriTQxRNQSHUUF";
            string sEncodingAESKey = "lpGRJCYqiZF8aSNOPI9q4nPBm4RrEzoVXUk9bgEKjno";
            string sCorpID = "wx0beba9277d2499d0";

            string sReqMsgSig = msg_signature;
            // string sReqTimeStamp = HttpUtils.ParseUrl("timestamp");
            string sReqTimeStamp = timestamp;
            // string sReqNonce = HttpUtils.ParseUrl("nonce");
            string sReqNonce = nonce;

            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            string sReqData = Encoding.UTF8.GetString(b);

            // string sReqData = value;
            string sMsg = "";  // 解析之后的明文

            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);
            int ret = 0;
            //解析用户发送到App的消息
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            if (ret != 0)
            {
                System.Console.WriteLine("ERR: Decrypt Fail, ret: " + ret);
                return null;
            }
            // ret==0表示解密成功，sMsg表示解密之后的明文xml串
            // TODO: 对明文的处理
            // For example:
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sMsg);
            XmlNode root = doc.FirstChild;
            string content = root["Content"].InnerText;

            string sRespData = "<xml>" +
                "<ToUserName><![CDATA[MRL]]></ToUserName>" +
            "<FromUserName><![CDATA[wx0beba9277d2499d0]]></FromUserName>" +
            "<CreateTime>1348831860</CreateTime>" +
            "<MsgType><![CDATA[text]]></MsgType>" +
            "<Content><![CDATA[Success is so beautifull]]></Content>" +
            "<MsgId>1234567890123456</MsgId><AgentID>0</AgentID>" +
            "</xml>";
            string sEncryptMsg = ""; //xml格式的密文
            ret = wxcpt.EncryptMsg(sRespData, sReqTimeStamp, sReqNonce, ref sEncryptMsg);
            if (ret != 0)
            {
                System.Console.WriteLine("ERR: EncryptMsg Fail, ret: " + ret);
                return null;
            }
            // TODO:
            // 加密成功，企业需要将加密之后的sEncryptMsg返回
            // HttpUtils.SetResponse(sEncryptMsg);

            return sEncryptMsg;
        }
    }
}