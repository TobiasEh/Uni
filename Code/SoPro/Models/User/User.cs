namespace Sopro.Models.User
{
    /// <summary>
    /// Beschreibt den Benutzer.
    /// </summary>
    public struct User
    {
        public string email { get; set; }
        public UserType usertype { get; set; }
    }
}
