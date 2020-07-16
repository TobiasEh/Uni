namespace Sopro.Models.History
{
    public interface IEvaluation
    {
        public bool addSuggestion(Suggestion suggestion);
        public bool removeSuggestion(Suggestion suggestion);
    }
}