using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Millionaire.GameEngine
{
    public class TxtFileQuestionsRepository : IRepository
    {
        private bool _isLoaded = false;
        private List<MultipleChoiceQuestion> _questions = new List<MultipleChoiceQuestion>();

        public int Count
        {
            get 
            {
                if(!_isLoaded)
                    throw new InvalidOperationException("Repository is not loaded");
                return _questions.Count;
            }
        }

        public MultipleChoiceQuestion GetQuestion(int index)
        {
            if (!_isLoaded)
                throw new InvalidOperationException("Repository is not loaded");

            return _questions[index];
        }

        public TxtFileQuestionsRepository()
        {

        }

        public TxtFileQuestionsRepository(string path)
        {
            Load(path);
        }

        public bool Load(string path)
        {
            _isLoaded = false;
            _questions.Clear();

            //try
            //{
                using (StreamReader reader = new StreamReader(path))
                {
                    while(!reader.EndOfStream)
                    {
                        string Q = reader.ReadLine();
                        string A0 = reader.ReadLine();
                        string A1 = reader.ReadLine();
                        string A2 = reader.ReadLine();
                        string A3 = reader.ReadLine();
                        int right = int.Parse(reader.ReadLine());
                        _questions.Add(new MultipleChoiceQuestion(Q, A0, A1, A2, A3, right));
                    }
                }
                
                _isLoaded = true;
                return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }
    }
}
