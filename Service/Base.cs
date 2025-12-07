using System.Collections.Generic;

namespace DuelingSimulation.Service.Base
{
    public interface IWizardService
    {
        WizardProfileDto GetWizardProfile(int wizardId);
        void UpdateWizardRating(int wizardId, int ratingChange);
        IEnumerable<SpellDto> GetKnownSpellsForWizard(int wizardId); 
    }

    public interface ISpellService
    {
        IEnumerable<SpellDto> GetAllSpells();
        SpellDto GetSpellById(int spellId);
    }

    public interface IDuelService
    {
        DuelResultDto ConductDuel(int wizard1Id, int wizard2Id, string duelType);
        IEnumerable<DuelResultDto> GetWizardDuelHistory(int wizardId);
    }
}