namespace XDash.Framework.Models
{
    public enum XDashSendResponseStatus
    {
        Success,
        TimeoutDuringConnect,
        ErrorDuringConnect,
        TimeoutDuringHandshake,
        ErrorDuringHandshake,
        HandshakeRefused,
        TimeoutDuringTransfer,
        ErrorDuringTransfer
    }
}
