using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatLib
{
    public class DepartMems
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }

        public List<UserID> userlist { get; set; }
    }

    public class UserID
    {
        public string userid{get;set;}
        public string name{get;set;}
    }
}