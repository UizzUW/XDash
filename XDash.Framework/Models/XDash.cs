using System.Collections.Generic;

namespace XDash.Framework.Models
{
    public class XDash
    {
        public byte[] SerialData { get; set; }
        public IEnumerable<XDashFile> Files { get; set; }
    }
}
