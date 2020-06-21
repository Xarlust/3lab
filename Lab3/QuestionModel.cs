using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class QuestionModel
    {
        private int id;
        private int category;
        private string question;
        private string answer;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Category
        {
            get { return category; }
            set { category = value; }
        }

        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
    }
}
