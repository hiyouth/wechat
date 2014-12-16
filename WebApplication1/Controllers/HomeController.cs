using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public String GetToken()
        {
            string uri =
             " https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wx0beba9277d2499d0&corpsecret=T1XBGAti9ycGk2Isc6r1ThMMH2MB-cVEC5WCTd3PocLLds7uPqLKrADGsxwjGW1N";
            WebClient wc = new WebClient();
            Console.WriteLine("Sending an HTTP GET request to " + uri);
            Stream st = wc.OpenRead(uri);
            StreamReader sr = new StreamReader(st);
            string res = sr.ReadToEnd();
            sr.Close();
            st.Close();

            Token at = JsonConvert.DeserializeObject<Token>(res);
            return at.access_token;
        }

        public String PostNews()
        {
            HttpWebRequest request = null;
            Uri uri1 = new Uri("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + GetToken());

            request = (HttpWebRequest)WebRequest.Create(uri1);

            request.Method = "POST";

            request.ContentType = "application/json";

            News news = new News()
            {
                articles = new Article[]
                {
                    new Article()
                    {
                        title="岁月如歌",
                        picurl="http://img5.douban.com/view/group_topic/large/public/p3651537.jpg",
                        description= "当你爱上一个人的时候，在你的脑海中，你早就和他/她过完了一辈子" +
   "看完这段我想起了另一句，我曾经惋惜于你的生命之短暂，却忘了你的一季就等于我的一生" +
   "一辈子能有多长？再长也长不过等待。可一辈子的数载光阴，竟可以浓浓一笔徜徉在小小书中，品味着人世的曲折婉转." +
  " 耀一说，他是捡故事的人，捡有拾金不昧之意，也有处于下层之艰辛。王尔德说：我们都生活在阴沟里，" +
  "但其中依然有人在仰望星空。他虽是捡故事，但却用自己平淡的笔触一点一滴的积攒世间的冷暖悲欢。" +
   "我们敌不过生死，却也暖不过平凡" +
   "我们身处一片纷杂的渊薮之中，被忙碌扯破，仰人鼻息，很多时候缺少的是一股暖流去洗清心灵上的污浊" +
  " 当都市的美变成花卉的装饰，当音乐的真变成乐器的噪杂，当情谊的爱变成刻意寒暄的过往，一切的天然都逝去了美的意义，" +
  " 这本书最感人的就是返璞归真，是用平凡的细碎道尽芸芸众生的细水流长。 " +
  " 等不来盼头，热不过初恋，西红柿大妈的虐心故事直戳泪点；等不到从前，冷不过人心，" +
   "明知等不到善良狗狗的回归，却依然内心翻搅；脱离了现实，美不过想象，爱永远是没有尽头的执念" +
   "我喜欢大爷说的月亮像半个括弧，直到失去了爱人，再美的满月终究是个句号；我唏嘘那个倔强的老太，" +
   "在失去的老伴后，盲在心中，红也罢，绿也罢；我感叹缅怀爱人的黎阳，一百种风味的面，二十五分钟的召唤，无论是生抑或死，他的爱永将鲜活。" +
   "这本书的人物，上至老一辈人，下至二十出头，每个故事，都似带着自我伤痛，把最血泪的真实摆置在桌前...",
                     url="http://book.douban.com/review/7118903/"
                    }
                }
            };
            
            

            NewsMessage t = new NewsMessage()
            {
                touser = "@all",
                msgtype = "news",
                agentid = "0",
                news = news
            };
            String text = JsonConvert.SerializeObject(t);


            using (Stream writeStream = request.GetRequestStream())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(text);
                writeStream.Write(bytes, 0, bytes.Length);
                string str = System.Text.Encoding.Default.GetString(bytes);

            }
            request.GetResponse();
            return "hi";
        }

        public String PostData()
        {
         
            HttpWebRequest request=null;
            Uri uri1 = new Uri("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token="+GetToken());

            request = (HttpWebRequest) WebRequest.Create(uri1);

            request.Method = "POST";

            request.ContentType = "application/json";

            
            TextMessage t = new TextMessage()
            {
                touser="@all",
                msgtype="text",
                agentid="0",
                safe="0",
                text=new TextContent()
                {
                    content="当你爱上一个人的时候，在你的脑海中，你早就和他/她过完了一辈子"+
　　 "看完这段我想起了另一句，我曾经惋惜于你的生命之短暂，却忘了你的一季就等于我的一生"+
　　 "一辈子能有多长？再长也长不过等待。可一辈子的数载光阴，竟可以浓浓一笔徜徉在小小书中，品味着人世的曲折婉转."+
　　" 耀一说，他是捡故事的人，捡有拾金不昧之意，也有处于下层之艰辛。王尔德说：我们都生活在阴沟里，"+
  "但其中依然有人在仰望星空。他虽是捡故事，但却用自己平淡的笔触一点一滴的积攒世间的冷暖悲欢。"+
   "我们敌不过生死，却也暖不过平凡"+
   "我们身处一片纷杂的渊薮之中，被忙碌扯破，仰人鼻息，很多时候缺少的是一股暖流去洗清心灵上的污浊"+
  " 当都市的美变成花卉的装饰，当音乐的真变成乐器的噪杂，当情谊的爱变成刻意寒暄的过往，一切的天然都逝去了美的意义，"+
  " 这本书最感人的就是返璞归真，是用平凡的细碎道尽芸芸众生的细水流长。 "+
  " 等不来盼头，热不过初恋，西红柿大妈的虐心故事直戳泪点；等不到从前，冷不过人心，"+
   "明知等不到善良狗狗的回归，却依然内心翻搅；脱离了现实，美不过想象，爱永远是没有尽头的执念"+
   "我喜欢大爷说的月亮像半个括弧，直到失去了爱人，再美的满月终究是个句号；我唏嘘那个倔强的老太，"+
   "在失去的老伴后，盲在心中，红也罢，绿也罢；我感叹缅怀爱人的黎阳，一百种风味的面，二十五分钟的召唤，无论是生抑或死，他的爱永将鲜活。"+
   "这本书的人物，上至老一辈人，下至二十出头，每个故事，都似带着自我伤痛，把最血泪的真实摆置在桌前，"
                }
            };
           String text=JsonConvert.SerializeObject(t);

   //         string postdata ="touser:"
   //'"touser": "UserID1|UserID2|UserID3",
   //"toparty": " PartyID1 | PartyID2 ",
   //"totag": " TagID1 | TagID2 ",
   //"msgtype": "text",
   //"agentid": "1",
   //"text": {
   //    "content": "Holiday Request For Pony(http://xxxxx)"
   //},
   //"safe":"0"'
            

            using (Stream writeStream = request.GetRequestStream())
            {
                 UTF8Encoding encoding = new UTF8Encoding();
                  byte[] bytes = encoding.GetBytes(text);
                  writeStream.Write(bytes, 0, bytes.Length);
                  string str = System.Text.Encoding.Default.GetString(bytes);
                  
            }
            request.GetResponse();
            return "hi";
        }

        public class TextMessage
        {
            public string touser{get;set;}
            public string toparty{get;set;}
            public string totag{get;set;}
            public string msgtype{get;set;}
            public string agentid{get;set;}

            public TextContent text{get;set;}
            public string safe;
        }

        public class Token
        {
            public string access_token { get; set; }
        }

        public class TextContent
        {
            public string content{get;set;}
        }

         public class NewsMessage
        {
            public string touser{get;set;}
            public string toparty{get;set;}
            public string totag{get;set;}
            public string msgtype{get;set;}
            public string agentid{get;set;}

            public News news{get;set;}
        }

         public class News
         {
             public Article[] articles { get; set; }
         }

        public class Article
        {
            public string title{get;set;}
            public string description{get;set;}
            public string url{get;set;}
            public string picurl{get;set;}
        }
    }
}