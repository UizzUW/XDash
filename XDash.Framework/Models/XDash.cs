namespace XDash.Framework.Models
{
    public class XDash
    {
        public XDashClient Sender { get; set; }
        public byte[] SerialData { get; set; }
        public XDashFile[] Files { get; set; }
    }
}
