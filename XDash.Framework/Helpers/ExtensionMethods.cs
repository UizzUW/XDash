namespace XDash.Framework.Helpers
{
    public static class ExtensionMethods
    {
        public static string AsFormattedBytesValue(this ulong byteCount)
        {
            const float iKb = 1024;
            const float iMb = 1048576;
            const float iGb = 1073741824;
            if (byteCount < iKb)
            {
                return $"{byteCount:0.##} B";
            }
            if (byteCount >= iKb && byteCount < iMb)
            {
                return $"{byteCount / iKb:0.##} KB";
            }
            if (byteCount >= iMb && byteCount < iGb)
            {
                return $"{byteCount / iMb:0.##} MB";
            }
            return $"{byteCount / iGb:0.##} GB";
        }
    }
}
