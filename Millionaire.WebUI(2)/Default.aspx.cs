using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.Drawing;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Mail;

using Millionaire.GameEngine;

namespace Millionaire.WebUI
{
    internal struct SessionInfo
    {
        public static string FirstLoad = "FirstLoad";
        public static string Game = "Game";
        public static string BackColor = "BackColor";
        public static string ForeColor = "ForeColor";
    }

    public partial class Default : System.Web.UI.Page
    {
        #region                        - Fields

        private GameASPNETForm _game;

        private RadioButton[] _answerButtons;

        #endregion


        #region                        - UI Events Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            bool? firstLoad = (bool?)Session[SessionInfo.FirstLoad];
            if (firstLoad == null)
            {
                tblQuestions.Visible = false;
                btnAnswer.Visible = false;
                tblUserDecision.Visible = false;
                email.Visible = false;

                Session.Add(SessionInfo.BackColor, btnAnswer.BackColor);
                Session.Add(SessionInfo.ForeColor, btnAnswer.ForeColor);
            }
            else
            {
                tblUserDecision.Enabled = true;
                ShowPossibleUserDecisions();
                ShowPrizesProgress();
            }

            _answerButtons = new RadioButton[4];
            _answerButtons[0] = answer0;
            _answerButtons[1] = answer1;
            _answerButtons[2] = answer2;
            _answerButtons[3] = answer3;
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Visible = false;

            tblQuestions.Visible = true;
            btnAnswer.Visible = true;
            tblUserDecision.Visible = true;

            TxtFileQuestionsRepository questions = new TxtFileQuestionsRepository("Questions01.txt");
            _game = new GameASPNETForm(questions);
            _game.Play();

            ShowQuestion(_game.NextQuestion());
            ShowInfo(_game.Current + 1);

            Session.Add(SessionInfo.Game, _game);
            Session.Add(SessionInfo.FirstLoad, false);

            ShowPrizesProgress();
        }

        protected void btnAnswer_Click(object sender, EventArgs e)
        {
            int checkedBtn = CheckedAnswerRadio();

            if (checkedBtn < 0)
                return;

            _game = (GameASPNETForm)Session[SessionInfo.Game];

            if (this.btnAnswer.Text == "Answer")
            {
                DiasbleAnswerRadios();

                if (_game.CheckLastQuestionAnswer(checkedBtn))
                {
                    _answerButtons[checkedBtn].BackColor = Color.Lime;
                    _answerButtons[checkedBtn].ForeColor = Color.White;
                }
                else
                {
                    _answerButtons[checkedBtn].BackColor = Color.Red;
                }

                if (_game.GameStatus == GameStatus.Playing)
                {
                    btnAnswer.Text = "Next question";

                    btnAnswer.BackColor = Color.Lime;
                    btnAnswer.ForeColor = Color.White;

                    ShowInfo(_game.Current);
                    ShowPossibleUserDecisions();
                }
                else
                {
                    if (_game.CurrentPrize == _game.MaxPrize)
                    {
                        welcome.BackColor = Color.Gold;
                        welcome.ForeColor = Color.FromArgb(0x30, 0x30, 0x30);
                        welcome.Text = "Congradulations! You has won your 1000000!";
                    }
                    else if (_game.CurrentPrize > 0)
                    {
                        welcome.BackColor = Color.Lime;
                        welcome.ForeColor = Color.FromArgb(0x60, 0x60, 0x60);
                        welcome.Text = "Game is over. You has won " + _game.CurrentPrize.ToString();
                    }
                    else
                    {
                        welcome.BackColor = Color.Red;
                        welcome.ForeColor = Color.FromArgb(0xC0, 0xC0, 0xC0);
                        welcome.Text = "Unfortunately you has lost. Try it again";
                    }

                    btnAnswer.Visible = false;
                    tblUserDecision.Visible = false;
                    Session.RemoveAll();
                }
            }
            else
            {
                btnAnswer.Text = "Answer";
                UncheckAnswerRadios();
                EnableAnswerRadios();

                ShowInfo(_game.Current + 1);
                ShowQuestion(_game.NextQuestion());

                btnAnswer.BackColor = (Color)Session[SessionInfo.BackColor];
                btnAnswer.ForeColor = (Color)Session[SessionInfo.ForeColor];
            }
        }

        protected void btnUsrHelpOfAudience_Click(object sender, EventArgs e)
        {
            _game = (GameASPNETForm)Session[SessionInfo.Game];
            if (_game != null)
            {
                HelpOfAudienceInfo state = new HelpOfAudienceInfo()
                {
                    CliensScript = this.ClientScript,
                    page = this,
                    question = question,
                    UniqueID = UniqueID
                };
                _game.HelpOfAudience(state);

                ShowPossibleUserDecisions();
            }
        }

        protected void btnUsrHelpOfFriend_Click(object sender, EventArgs e)
        {
            _game = (GameASPNETForm)Session[SessionInfo.Game];
            if (_game != null)
            {
                HelpOfFriendInfo state = new HelpOfFriendInfo()
                {
                    page = this
                };
                _game.HelpOfFriend(state);

                ShowPossibleUserDecisions();
            }
        }

        protected void btnUsrHelpOf5050_Click(object sender, EventArgs e)
        {
            _game = (GameASPNETForm)Session[SessionInfo.Game];
            if (_game != null)
            {
                var incorrect = _game.HelpOf5050();
                
                _answerButtons[incorrect.Item1].Visible = false;
                _answerButtons[incorrect.Item2].Visible = false;

                ShowPossibleUserDecisions();
            }
        }

        protected void btnUsrGetMoney_Click(object sender, EventArgs e)
        {
            _game = (GameASPNETForm)Session[SessionInfo.Game];
            if (_game != null)
            {
                int prize = _game.GetPrize();

                welcome.BackColor = Color.Pink;
                welcome.ForeColor = Color.FromArgb(0x30, 0x30, 0x30);
                welcome.Text = "It's a pity that you won't play longer. You has won " + prize.ToString();

                btnAnswer.Visible = false;
                tblUserDecision.Visible = false;
                Session.RemoveAll();
            }
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            _game = (GameASPNETForm)Session[SessionInfo.Game];
            if (_game != null)
            {
                var fromAddress = new MailAddress("roman.tarasiuk.l@gmail.com", "Роман Тарасюк Account 2");
                var toAddress = new MailAddress(txtFriendEmail.Text, "My best friend");
                string fromPassword = txtPassword.Text;
                string subject = "Help me on 'Who Wants to Be a Millionaire' Game";
                string body = txtEmailMessage.Text;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                HideEmailBlock();
            }

        }

        #endregion


        #region                        - Methods

        public void ShowEmailBlock()
        {
            email.Visible = true;
        }

        public void HideEmailBlock()
        {
            email.Visible = false;
        }
        
        #endregion


        #region                        - Helper methods

        private int CheckedAnswerRadio()
        {
            for (int i = 0; i < 4; i++)
                if (_answerButtons[i].Checked)
                    return i;
            return -1;
        }

        private void UncheckAnswerRadios()
        {
            for (int i = 0; i < 4; i++)
            {
                _answerButtons[i].Checked = false;
                _answerButtons[i].BackColor = (Color)Session[SessionInfo.BackColor];
                _answerButtons[i].ForeColor = (Color)Session[SessionInfo.ForeColor];
            }
        }

        private void EnableAnswerRadios()
        {
            for (int i = 0; i < 4; i++)
            {
                _answerButtons[i].Enabled = true;
                _answerButtons[i].Visible = true;
            }
        }

        private void DiasbleAnswerRadios()
        {
            for (int i = 0; i < 4; i++)
            {
                _answerButtons[i].Enabled = false;
            }
        }

        private void ShowQuestion(MultipleChoiceQuestion q)
        {
            UncheckAnswerRadios();

            question.Text = q.Question;
            answer0.Text = @"<span class=""answerLetter"">A. </span>" + q.Answer0;
            answer1.Text = @"<span class=""answerLetter"">B. </span>" + q.Answer1;
            answer2.Text = @"<span class=""answerLetter"">C. </span>" + q.Answer2;
            answer3.Text = @"<span class=""answerLetter"">D. </span>" + q.Answer3;
        }

        private void ShowInfo(int questionNumber)
        {
            welcome.Text = "Question #" + questionNumber.ToString() + "/" + Game.QuestionsCount
                + " with prize " + _game.CurrentPrize + " (" + _game.CurrentGuaranteedPrize + " garanteed)";

        }

        private void ShowPossibleUserDecisions()
        {
            _game = (GameASPNETForm)Session[SessionInfo.Game];
            if (_game != null)
            {
                btnUsrGetMoney.Enabled = _game.CurrentPrize > 0;
                btnUsrHelpOf5050.Enabled = _game.HelpOf5050Enabled;
                btnUsrHelpOfAudience.Enabled = _game.HelpOfAudienceEnabled;
                btnUsrHelpOfFriend.Enabled = _game.HelpOfFriendEnabled;
            }
        }

        private void ShowPrizesProgress()
        {
            _game = (GameASPNETForm)Session[SessionInfo.Game];
            if (_game != null)
            {
                StringBuilder info = new StringBuilder();
                for (int i = Game.QuestionsCount - 1; i >= 0; i--)
                {
                    int prize = _game.Prizes[i];

                    info.AppendLine(@"<span style=""padding: 10px"">");

                    if (prize == _game.Guaranteed1 || prize == _game.Guaranteed2)
                        info.Append(@"<span style=""color:lime"">");

                    if (i == _game.Current)
                    {
                        info.Append(@"<span style=""color:gold"">&gt </span>");
                        info.Append(@"<span style=""background-color:gold; color:#303030"">");
                    }

                    info.Append(prize);

                    if (i == _game.Current)
                        info.Append(@"</span>");

                    if (prize == _game.Guaranteed1 || prize == _game.Guaranteed2)
                        info.Append("</span>");

                    info.Append(@"</span><br />");

                }

                progress.InnerHtml = info.ToString();
            }
        }

        #endregion
    }
}