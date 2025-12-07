using DuelingSimulation.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DuelingSimulation.Service
{
    public static class SpellMapper
    {
        public static SpellDto MapToDto(SpellEntity entity)
        {
            if (entity == null) return null;
            return new SpellDto
            {
                Name = entity.Name,
                Damage = entity.Damage,
                Effect = entity.Effect
            };
        }
    }

    public static class WizardMapper
    {
        public static WizardProfileDto MapToProfileDto(WizardEntity entity, IEnumerable<SpellEntity> knownSpells)
        {
            if (entity == null) return null;
            return new WizardProfileDto
            {
                Id = entity.Id,
                Name = entity.Name,
                House = entity.House,
                Rating = entity.Rating,
                KnownSpells = knownSpells.Select(SpellMapper.MapToDto).ToList()
            };
        }
    }

    public static class DuelHistoryMapper
    {
        public static DuelResultDto MapToDto(DuelHistory entity, string winnerName, string loserName)
        {
            if (entity == null) return null;
            return new DuelResultDto
            {
                DuelId = entity.DuelId,
                WinnerName = winnerName,
                LoserName = loserName,
                RatingStake = entity.RatingStake,
                TurnLogSummary = entity.TurnLog
            };
        }
    }
}