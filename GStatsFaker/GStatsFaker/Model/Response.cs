namespace GStatsFaker.Model
{
    public class Response
    {
        
        public int Code { get; set; }
        public string Desc { get; set; } = string.Empty;

        public Response()
        {
            
        }

        public Response(int Code, string Desc)
        {
            this.Desc = Desc;
            this.Code = Code;
        }
    }
}
