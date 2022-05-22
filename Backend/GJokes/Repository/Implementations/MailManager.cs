using GStatsFaker.Repository.Interfaces;
using System.Net;
using System.Net.Mail;

namespace GStatsFaker.Repository.Implementations
{
    public class MailManager : IMailManager
    {
        public SmtpClient Client { get; set; } = default!;
        private string Mail { get; set; } = string.Empty;

        private string CodeMailBody { get; set; } = default!;

        public MailManager()
        {
            InitConnection(Config.Mail, Config.MailPassword);
        }

        public void InitConnection(string Mail, string Password)
        {
            Client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(Mail, Password),
                EnableSsl = true,
            };
            this.Mail = Mail;
        }

        public void SendMail(string Empfänger, string Betreff, string body)
        {
            MailMessage MM = new MailMessage(Mail, Empfänger, Betreff, body);
            MM.IsBodyHtml = true;

            Client.Send(MM);
        }

        public void SendCodeMail(string Empfänger, string Code)
        {
            string str = "In this Mail is the verification Token for your Mail. You need to copy it and paste it on our site in the right textfield";

            string CodeHTML = "";
            for (int i = 0; i < Code.Length; i++)
            {
                CodeHTML += $@"<h2 class='Hallo' style=' background: rgba(0, 0, 0, 0.123);
                        border-radius: .4rem;
                        display: inline;
                        padding-left: 8px;
                        padding-right: 8px;
                        padding-bottom: 4px;
                        margin-left: 2px;
                        margin-right: 2px;
                        padding-top: 4px;'
                        >{Code[i]}</h2>";
            }

            string myText =
                @"
<html>
                <link rel='preconnect' href='https://fonts.googleapis.com'>
                <link rel = 'preconnect'' href = 'https://fonts.gstatic.com' crossorigin >
                <link href = 'https://fonts.googleapis.com/css2?family=Roboto:wght@300&display=swap' rel = 'stylesheet' >
                        " + @$"
                <div class='Red'>
                    <div class='dings' style='border: solid;
                        padding: 2rem;
                        width: 600px;
                        margin: auto;
                        display: block;
                        margin-top: .1rem;
                        border-radius: 1rem;
                        background: rgb(252, 255, 255);
                        box-shadow: 0px 0px 10rem rgb(255, 255, 255);
                        border-width: .1rem;
                        font-family: "+"\"Roboto\""+$@", sans-serif;'>
                        <h1 class=''>Dear User,</h1>
                        <p>
                            {str}
                        </p>
                        <div class='d' stlye='margin-top: 30px;
                        text-align: center;
                        margin-bottom: 30px;'>
                            {CodeHTML}
                        </div>
                        <p>with best regards<br> the GJokes Team</p>
                        <small style='font-size: 12px; '>If you don't know why you got this Email, just ignore it.</smallA>
                       </div>
                </div>

                </html>";

            SendMail(Empfänger, "GJokes Verifikation Token", myText);
        }
    }
}