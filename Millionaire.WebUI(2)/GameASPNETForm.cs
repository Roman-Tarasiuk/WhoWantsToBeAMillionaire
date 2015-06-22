using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;

using Millionaire.GameEngine;

namespace Millionaire.WebUI
{
    public class  HelpOfAudienceInfo
    {
        public ClientScriptManager CliensScript;
        public Default page;
        public string UniqueID;
        public TableCell question;
    }

    public class HelpOfFriendInfo
    {
        public Default page;
    }

    public class GameASPNETForm : Game
    {
        public GameASPNETForm(IRepository questions)
            : base(questions)
        {
        }

        #region                        - Game abstract methods implementation

        protected override void HelpOfFriendImpl(object state)
        {
            //Version 0
            //HelpOfAudienceState State = state as HelpOfAudienceState;

            //if (State != null)
            //{
            //    string message = "This function is not implemented yet."
            //            + @" It is coming soon";
            //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //    sb.Append("<script type = 'text/javascript'>");
            //    sb.Append("window.onload=function(){");
            //    sb.Append("alert('");
            //    sb.Append(message);
            //    sb.Append("')};");
            //    sb.Append("</script>");
            //    State.CliensScript.RegisterClientScriptBlock(State.page.GetType(), "alert", sb.ToString());
            //}

            // Version 1
            //MailMessage mail = new MailMessage("qwe@ukr.net", "roman_tarasiuk@ukr.net");
            //SmtpClient client = new SmtpClient();
            //client.Port = 25;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Host = "smtp.gmail.com";
            //mail.Subject = "Help to your friend in 'Who Wants to Be a Millionaire' Game";
            //mail.Body = "Help me!";
            //client.Send(mail);

            // Version 2
            HelpOfFriendInfo helpInfo = (HelpOfFriendInfo)state;
            helpInfo.page.ShowEmailBlock();
        }

        protected override void HelpOfAudienceImpl(object state)
        {
            HelpOfAudienceInfo State = state as HelpOfAudienceInfo;

            if (State != null)
            {
                string message = "See new tab for information"
                        + " (it may be need to allow popup windows in your browser)";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                State.CliensScript.RegisterClientScriptBlock(State.page.GetType(), "alert", sb.ToString());

                string url = @"https://www.google.com.ua/search?q=" + State.question.Text;
                string script = string.Format("window.open('{0}');", url);
                State.CliensScript.RegisterStartupScript(State.page.GetType(), "newPage" + State.UniqueID, script, true);
            }
        }

        #endregion
    }
}