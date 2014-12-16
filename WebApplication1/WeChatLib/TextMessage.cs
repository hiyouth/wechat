using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatLib
{
    public class TextMessage
    {
        public string touser { get; set; }
        public string toparty { get; set; }
        public string totag { get; set; }
        public string msgtype { get; set; }
        public string agentid { get; set; }

        public TextContent text { get; set; }
        public string safe;
    }

    public class TextContent
    {
        public string content { get; set; }
    }
}