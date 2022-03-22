namespace GStatsFaker.Model
{
    public class Respone
    {
        
        public int Code { get; set; }
        public string Desc { get; set; } = string.Empty;

        public Respone()
        {
            
        }

        public Respone(int Code, string Desc)
        {
            this.Desc = Desc;
            this.Code = Code;
        }
    }
}
