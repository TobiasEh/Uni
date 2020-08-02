namespace Sopro.Models.History
{
    public class Suggestion
    {
        /// <summary>
        /// Ein Verbesserungsvorschlag für ein Szenario.
        /// </summary>
        public string suggestion { get; set; }

        public Suggestion(int stations, int zones)
        {
            suggestion = "";
        }

        public Suggestion()
        {
            suggestion = "";
        }
    }
}
