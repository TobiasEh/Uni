using Sopro.Interfaces.ControllerHistory;
using Sopro.Models.Infrastructure;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.History
{
    /// <summary>
    /// Evaluation zu einem Szenario.
    /// </summary>
    public class Evaluation : IEvaluation
    {
        [Range(0,1)]
        public List<Suggestion> suggestions { get; set; }
        public double bookingSuccessRate{ get; set; }
        public double unneccessaryWorkload { get; set; }
        public double neccessaryWorkload { get; set; }
        [EnumLength(1,typeof(PlugType))]
        public List<double> plugDistributionAccepted { get; set; }
        [EnumLength(1, typeof(PlugType))]
        public List<double> plugDistributionDeclined { get; set; }

        /// <summary>
        /// Füge einen Vorschlag hinzu.
        /// </summary>
        /// <param name="suggestion">Der Vorschlag als Ergebnis der Evaluation.</param>
        /// <returns>Wahrheitswert ob das Hinzufügen erfolgreich war.</returns>
        public bool addSuggestion(Suggestion suggestion)
        {
            if (suggestions.Contains(suggestion))
                return false;

            suggestions.Add(suggestion);
            return suggestions.Contains(suggestion);
        }

        /// <summary>
        /// Entferne einen Vorschlag.
        /// </summary>
        /// <param name="suggestion">Der zu entfernende Vorschlag.</param>
        /// <returns>Wahrheitswert ob das Entfernen des Vorschlags erfolgreich war.</returns>
        public bool removeSuggestion(Suggestion suggestion)
        {
            if (!suggestions.Contains(suggestion))
                return false;

            suggestions.Remove(suggestion);
            return !suggestions.Contains(suggestion);
        }
    }
}