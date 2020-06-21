using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class Category
    {
        private int _id;
        private string name;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
