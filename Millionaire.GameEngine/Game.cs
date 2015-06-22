using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.GameEngine
{
    public abstract class Game
    {
        #region                        - Fields

        private IRepository _questions;

        private bool _lastQuestionAnswered = false;
        private bool _lastQuestionAnsweredCorrectly = true;

        private MultipleChoiceQuestion _currentQuestion;

        public const int QuestionsCount = 15;
        private const int _guaranteedIndex1 = 4;
        private const int _guaranteedIndex2 = 9;

        private int[] _prizes =
        {
                100, // 0
                200, // 1
                300, // 2
                500, // 3
               1000, // 4 - guaranteed
               2000, // 5
               4000, // 6
               8000, // 7
              16000, // 8
              32000, // 9 - guaranteed
              64000, // 10
             125000, // 11
             250000, // 12
             500000, // 13
            1000000  // 14
        };

        #endregion


        #region                        - Properties

        /// <summary>
        /// Current question's index
        /// </summary>
        public int Current { get; private set; }
        
        public int CurrentPrize { get; private set; }

        public int[] Prizes
        {
            get
            {
                return (int[])_prizes.Clone();
            }
        }

        public int MaxPrize
        {
            get
            {
                return this._prizes[Game.QuestionsCount - 1];
            }
        }

        public int CurrentGuaranteedPrize
        {
            get
            {
                if (this.Current > _guaranteedIndex2)
                    return this._prizes[_guaranteedIndex2];
                else if (this.Current > _guaranteedIndex1)
                    return this._prizes[_guaranteedIndex1];
                else
                    return 0;
            }
        }

        public int Guaranteed1 { get { return this._prizes[_guaranteedIndex1]; } }
        public int Guaranteed2 { get { return this._prizes[_guaranteedIndex2]; } }

        public bool HelpOfFriendEnabled { get; private set; }
        public bool HelpOfAudienceEnabled { get; private set; }
        public bool HelpOf5050Enabled { get; private set; }

        public GameStatus GameStatus;

        #endregion


        #region                        - Event

        public event EventHandler GameIsEnded;

        #endregion


        #region                        - Constructor

        public Game(IRepository questions)
        {
            if (questions == null)
            {
                throw new NullReferenceException("questions");
            }

            if (questions.Count < QuestionsCount)
            {
                throw new ArgumentException("'questions' must have at least " + QuestionsCount.ToString() + " elements");
            }

            this._questions = questions;
            this.GameStatus = GameStatus.Inialized;
            this.Current = 0;
            this.CurrentPrize = 0;

            this.HelpOf5050Enabled = true;
            this.HelpOfAudienceEnabled = true;
            this.HelpOfFriendEnabled = true;
        }

        #endregion


        #region                        - Methods

        public void Play()
        {
            if (this.GameStatus != GameStatus.Inialized)
            {
                throw new InvalidOperationException("Game is already playing or ended");
            }

            this.GameStatus = GameStatus.Playing;
        }

        public MultipleChoiceQuestion NextQuestion()
        {
            if (this.GameStatus != GameStatus.Playing)
            {
                throw new InvalidOperationException("Game is not playing");
            }

            if (!this._lastQuestionAnsweredCorrectly)
            {
                throw new InvalidOperationException("Previous question not answered or is not correctly answered");
            }

            this._lastQuestionAnswered = false;
            this._currentQuestion = this._questions.GetQuestion(this.Current);
            return this._currentQuestion;
        }

        public bool CheckLastQuestionAnswer(int answerIndex)
        {
            if (this.GameStatus != GameStatus.Playing)
            {
                throw new InvalidOperationException("Game is not playing");
            }

            //if (this._lastQuestionAnswered && !this._lastQuestionAnsweredCorrectly)
            if (this._lastQuestionAnswered)
            {
                //throw new InvalidOperationException("Question is already answered or not asked yet");
                throw new InvalidOperationException("Question is already answered");
            }

            this._lastQuestionAnswered = true;
            this._lastQuestionAnsweredCorrectly = (answerIndex == this._currentQuestion.RightAnswer);

            if (this._lastQuestionAnsweredCorrectly == false)
            {
                this.GameStatus = GameStatus.Ended;

                if (this.Current > _guaranteedIndex2)
                    this.CurrentPrize = this._prizes[_guaranteedIndex2];
                else if (this.Current > _guaranteedIndex1)
                    this.CurrentPrize = this._prizes[_guaranteedIndex1];
                else
                    this.CurrentPrize = 0;
            }
            else
            {
                this.CurrentPrize = this._prizes[this.Current];
                this.Current++;

                if (this.Current == QuestionsCount)
                {
                    this.GameStatus = GameStatus.Ended;
                }
            }

            if (this.GameStatus == GameStatus.Ended)
            {
                OnGameIsEnded();
            }

            return this._lastQuestionAnsweredCorrectly;
        }

        public void HelpOfFriend(Object state)
        {
            this.HelpOfFriendImpl(state);
            this.HelpOfFriendEnabled = false;
        }

        public void HelpOfAudience(Object state)
        {
            this.HelpOfAudienceImpl(state);
            this.HelpOfAudienceEnabled = false;
        }

        /// <summary>
        /// Gets pair of incorrect answers
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> HelpOf5050()
        {
            Random rnd = new Random();
            int answerCount = 4;

            int i1 = -1;
            int i2 = -1;
            
            while(true)
            {
                i1 = rnd.Next(answerCount);
                if (i1 != _currentQuestion.RightAnswer)
                    break;
            }

            while (true)
            {
                i2 = rnd.Next(answerCount);
                if ((i2 != _currentQuestion.RightAnswer) && (i2 != i1))
                    break;
            }

            this.HelpOf5050Enabled = false;

            return new Tuple<int,int>(i1, i2);
        }

        protected abstract void HelpOfFriendImpl(Object state);
        protected abstract void HelpOfAudienceImpl(Object state);

        /// <summary>
        /// Player gets prize and leaves the game
        /// </summary>
        /// <returns></returns>
        public int GetPrize()
        {
            if(this.GameStatus != GameStatus.Playing)
            {
                throw new InvalidOperationException("Game is not playing now");
            }

            this.GameStatus = GameStatus.Ended;
            return this.CurrentPrize;
        }
        
        #endregion


        #region                        - Helper method

        protected void OnGameIsEnded()
        {
            if (this.GameIsEnded != null)
                this.GameIsEnded(this, EventArgs.Empty);
        }

        #endregion
    }

    public enum GameStatus
    {
        Inialized,
        Playing,
        Ended
    }
}
