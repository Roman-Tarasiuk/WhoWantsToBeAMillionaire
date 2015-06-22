using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.GameEngine
{
    public interface IRepository
    {
        int Count { get; }
        MultipleChoiceQuestion GetQuestion(int index);
    }
}
