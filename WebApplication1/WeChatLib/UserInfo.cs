using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatLib
{
    public class UserInfo
    {
        public int errcode{get;set;}
        public string errmsg{get;set;}
        public string userid{get;set;}

        public string name{get;set;}
        public string department{get;set;}

        public string position{get;set;}

        public string mobile{get;set;}

        public string email{get;set;}

        public string weixinid{get;set;}

        public string avatar{get;set;}

        public int status{get;set;}

        public Extattr extattr { get; set; }
    }

    public class Extattr
    {
        public List<Attr> attrs{get;set;}
    }

    public class Attr
    {
        public string name{get;set;}
        public string value{get;set;}
    }
}