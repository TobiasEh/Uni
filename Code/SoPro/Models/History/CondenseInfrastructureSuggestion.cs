namespace Sopro.Models.History
{
    public class CondenseInfrastructureSuggestion : Suggestion
    {
        public CondenseInfrastructureSuggestion(int stations, int zones) : base(stations, zones)
        {
            suggestion = "Sie brauchen ca. " + zones + " Zonen und " + stations + " Stationen weniger.";
        }
    }
}
