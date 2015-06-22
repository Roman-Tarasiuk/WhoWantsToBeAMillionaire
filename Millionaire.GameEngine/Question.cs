using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.GameEngine
{
    public class MultipleChoiceQuestion
    {
        public string Question { get;  set; }
        public string Answer0 { get;  set; }
        public string Answer1 { get;  set; }
        public string Answer2 { get;  set; }
        public string Answer3 { get;  set; }
        public int RightAnswer { get;  set; }

        public MultipleChoiceQuestion()
        {

        }

        public MultipleChoiceQuestion(string question, string answ0, string answ1, string answ2, string answ3, int rightAnsw)
        {
            Question = question;
            Answer0 = answ0;
            Answer1 = answ1;
            Answer2 = answ2;
            Answer3 = answ3;
            RightAnswer = rightAnsw;
        }
    }
}
