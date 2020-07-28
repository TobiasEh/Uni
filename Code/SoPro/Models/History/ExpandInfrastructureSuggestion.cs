namespace Sopro.Models.History
{
    public class ExpandInfrastructureSuggestion : Suggestion
    {
        /// <summary>
        /// Schlägt vor, inwiefern die Infrastruktur erweitert werden muss, damit ausreichend Buchungsanfragen gedeckt sind.
        /// </summary>
        /// <param name="stations">Empfohlene Anzahl nötiger Stationen.</param>
        /// <param name="zones">Emfpohlene Anzahl nötiger Zonen</param>
        public ExpandInfrastructureSuggestion(int stations, int zones) : base(stations, zones)
        {
            suggestion = "Sie brauchen ca. " + zones + " Zonen und " + stations + " Stationen mehr.";
        }
    }
}
