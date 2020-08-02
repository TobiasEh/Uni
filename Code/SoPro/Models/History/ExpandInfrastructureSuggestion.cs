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
            if (zones == 1)
                suggestion = "Sie werden wahrscheinlich eine neue Zone benötigen. Insgesamt brauchen sie ca. " + stations + " Stationen mehr.";
            else
                suggestion = "Insgesamt brauchen sie ca. " + stations + " Stationen mehr.";
        }
    }
}
