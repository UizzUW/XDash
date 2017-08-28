using System.Collections.Generic;

namespace XDash.Framework.Models
{
    public class XDash
    {
        public byte[] SerialData { get; set; }
        public List<XDashFile> Files { get; set; }
    }
}
