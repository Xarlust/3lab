using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class User
    {
        private int id;
        private string nickName;
        private int goodAns;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        public int GoodAns
        {
            get { return goodAns; }
            set { goodAns = value; }
        }
    }
}
