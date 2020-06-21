using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class Answer
    {
        private int id;
        private string text;
        private int questionId;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public int QuestionId
        {
            get { return questionId; }
            set { questionId = value; }
        }
    }
}
