namespace Sopro.Models.History
{
    public class ExpandInfrastructureSuggestion : Suggestion
    {
        public ExpandInfrastructureSuggestion(int stations, int zones) : base(stations, zones)
        {
            suggestion = "Sie brauchen ca. " + zones + " Zonen und " + stations + " Stationen mehr.";
        }
    }
}
