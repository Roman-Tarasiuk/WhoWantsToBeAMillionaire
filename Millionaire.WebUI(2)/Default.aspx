<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Millionaire.WebUI.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="Images/logo.png">
    <link rel="stylesheet" href="style.css" />
    <title>Who Wants to Be a Millionaire</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <img alt="logo" class="logo-zoom" src="Images/logo.png" />
        </div>
        <div>
            <asp:Label ID="welcome" runat="server" Text="Welcome to 'Who Wants to Be a Millionaire' Game!"></asp:Label>
        </div>
        <div>
            <asp:Button ID="btnStart" runat="server" Text="New game" OnClick="btnStart_Click" Width="87px" />
        </div>
        <div id="questions">
            <div>
                <asp:Table ID="tblQuestions" runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell ID="question" runat="server" ColumnSpan="2"></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="answer" runat="server" HorizontalAlign="Left" Width="50%">
                            <asp:RadioButton GroupName="Answers" ID="answer0" runat="server" />
                        </asp:TableCell>
                        <asp:TableCell CssClass="answer" runat="server" HorizontalAlign="Left">
                            <asp:RadioButton GroupName="Answers" ID="answer1" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="answer" runat="server" HorizontalAlign="Left">
                            <asp:RadioButton GroupName="Answers" ID="answer2" runat="server" />
                        </asp:TableCell>
                        <asp:TableCell CssClass="answer" runat="server" HorizontalAlign="Left">
                            <asp:RadioButton GroupName="Answers" ID="answer3" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div>
                <asp:Button ID="btnAnswer" runat="server" Text="Answer" OnClick="btnAnswer_Click" Font-Size="X-Large" />
            </div>
            <br />
            <div>
                <asp:Table ID="tblUserDecision" runat="server" Width="100%">
                    <asp:TableFooterRow>
                        <asp:TableCell Width="25%">
                            <asp:Button ID="btnUsrGetMoney" runat="server" Text="Get money" Enabled="false" OnClick="btnUsrGetMoney_Click" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Button ID="btnUsrHelpOfFriend" runat="server" Text="Friend's help" OnClick="btnUsrHelpOfFriend_Click" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Button ID="btnUsrHelpOfAudience" runat="server" Text="Help of audience" OnClick="btnUsrHelpOfAudience_Click" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Button ID="btnUsrHelpOf5050" runat="server" Text="50:50" OnClick="btnUsrHelpOf5050_Click" />
                        </asp:TableCell>
                    </asp:TableFooterRow>
                </asp:Table>
            </div>
        </div>
        <div id="progress" runat="server">
        </div>
        <div id="email" runat="server">
            <asp:Label ID="Label1" runat="server" Text="Mail to:"></asp:Label>
            <asp:TextBox ID="txtFriendEmail" runat="server" Width="205px"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Message:"></asp:Label>
            <asp:TextBox ID="txtEmailMessage" runat="server" Width="249px" Text="Enter message here..."></asp:TextBox>
            <br />
            <asp:Button ID="btnSendEmail" runat="server" Text="Send" OnClick="btnSendEmail_Click" />
        </div>
    </form>
</body>
</html>
