namespace Sopro.Models.History
{
    public class CondenseInfrastructureSuggestion : Suggestion
    {
        public CondenseInfrastructureSuggestion(int stations, int zones) : base(stations, zones)
        {
            suggestion = "Hello, is me machine, u will need " + zones + " zones and " + stations + "stations less";
        }
    }
}
