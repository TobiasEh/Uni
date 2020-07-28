namespace Sopro.Models.Communication
{
    /// <summary>
    /// Ereignisse zu denen nachrichten an den Benutzer versendet werden,
    /// </summary>
    public static class NotificationEvent
    {
        public const string ACCEPTED = "acceptedBooking";
        public const string DECLINED = "declinedBooking";
        public const string CHECKIN = "checkInBooking";
        public const string CHECKOUT = "checkOutBooking";
    }
}
