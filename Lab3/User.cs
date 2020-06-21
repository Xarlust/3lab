using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class User : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
