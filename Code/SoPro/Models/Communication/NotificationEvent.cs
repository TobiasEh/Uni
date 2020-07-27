namespace Sopro.Models.Communication
{
    /// <summary>
    /// Ereignisse zu denen nachrichten an den Benutzer versendet werden,
    /// </summary>
    public static class NotificationEvent
    {
        public static string ACCEPTED = "acceptedBooking";
        public static string DECLINED = "declinedBooking";
        public static string CHECKIN = "checkInBooking";
        public static string CHECKOUT = "checkOutBooking";
    }
}
