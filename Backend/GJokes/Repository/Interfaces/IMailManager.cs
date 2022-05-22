namespace GStatsFaker.Repository.Interfaces
{
    public interface IMailManager
    {
        public void SendMail(string Empfänger, string Betreff, string body);

        public void SendCodeMail(string Empfänger, string Code);
    }
}
