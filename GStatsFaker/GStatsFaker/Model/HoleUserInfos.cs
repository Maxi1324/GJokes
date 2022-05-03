namespace GStatsFaker.Model
{
    public class HoleUserInfos
    {
        public ConfigInfos ConfigInfos { get; set; } = new ConfigInfos();
        public string RealEmail { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool Blocked { get; set; }
        public bool Verified { get; set; }
    }
}
