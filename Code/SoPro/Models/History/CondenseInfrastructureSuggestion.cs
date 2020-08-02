namespace Sopro.Models.History
{
    public class CondenseInfrastructureSuggestion : Suggestion
    {
        /// <summary>
        /// Schlägt vor, inwiefern die Infrastruktur reduziert werden muss, damit keine Resourcen verschwendet werden.
        /// </summary>
        /// <param name="stations">Empfohlene Anzahl zu entfernender Stationen.</param>
        /// <param name="zones">Emfpohlene Anzahl zu entfernender Zonen</param>
        public CondenseInfrastructureSuggestion(int stations, int zones) : base(stations, zones)
        {
            if (zones == 1)
                suggestion = "Sie können Zonen abbauen. Insgesamt wären ca. " + stations + " Stationen weniger ausreichend.";
            else
                suggestion = "Insgesamt wären ca. " + stations + " Stationen weniger ausreichend.";
        }
    }
}
