using Sopro.Models.History;

namespace Sopro.Interfaces.ControllerHistory
{
    public interface IEvaluation
    {
        public bool addSuggestion(Suggestion suggestion);
        public bool removeSuggestion(Suggestion suggestion);
    }
}
