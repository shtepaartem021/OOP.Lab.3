using DuelingSimulation.Data;
using DuelingSimulation.Entities;
using DuelingSimulation.Repository.Base;
using DuelingSimulation.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuelingSimulation.Service
{
    public class SpellService : ISpellService
    {
        private readonly ISpellRepository _spellRepository;

        public SpellService(ISpellRepository spellRepository)
        {
            _spellRepository = spellRepository;
        }

        public IEnumerable<SpellDto> GetAllSpells()
        {
            var spells = _spellRepository.GetAll();
            return spells.Select(SpellMapper.MapToDto).ToList();
        }

        public SpellDto GetSpellById(int spellId)
        {
            var spell = _spellRepository.GetById(spellId);
            return SpellMapper.MapToDto(spell);
        }
    }

    public class WizardService : IWizardService
    {
        private readonly IWizardRepository _wizardRepository;
        private readonly IWizardSpellRepository _wizardSpellRepository;
        private readonly ISpellService _spellService;
        private readonly HogwartsDbContext _context;

        public WizardService(IWizardRepository wizardRepository, IWizardSpellRepository wizardSpellRepository, ISpellService spellService, HogwartsDbContext context)
        {
            _wizardRepository = wizardRepository;
            _wizardSpellRepository = wizardSpellRepository;
            _spellService = spellService;
            _context = context;
        }

        public WizardProfileDto GetWizardProfile(int wizardId)
        {
            var wizard = _wizardRepository.GetById(wizardId);
            if (wizard == null) return null;

            var knownSpellsDto = GetKnownSpellsForWizard(wizardId);

            return WizardMapper.MapToProfileDto(wizard, knownSpellsDto.Select(s => new SpellEntity { Name = s.Name, Damage = s.Damage, Effect = s.Effect }));
        }

        public IEnumerable<SpellDto> GetKnownSpellsForWizard(int wizardId)
        {
            var spellIds = _wizardSpellRepository.GetSpellIdsForWizard(wizardId);

            var knownSpells = new List<SpellDto>();
            foreach (var id in spellIds)
            {
                var spellDto = _spellService.GetSpellById(id);
                if (spellDto != null)
                {
                    knownSpells.Add(spellDto);
                }
            }
            return knownSpells;
        }

        public void UpdateWizardRating(int wizardId, int ratingChange)
        {
            var wizard = _wizardRepository.GetById(wizardId);
            if (wizard != null)
            {
                wizard.Rating += ratingChange;
                _wizardRepository.Update(wizard);
                _context.SaveChanges();
            }
        }
    }

    public class DuelService : IDuelService
    {
        private readonly IWizardRepository _wizardRepository;
        private readonly IDuelHistoryRepository _duelHistoryRepository;
        private readonly IWizardService _wizardService;
        private readonly HogwartsDbContext _context;
        private static int _duelCounter = 1;

        public DuelService(IWizardRepository wizardRepository, IDuelHistoryRepository duelHistoryRepository, IWizardService wizardService, HogwartsDbContext context)
        {
            _wizardRepository = wizardRepository;
            _duelHistoryRepository = duelHistoryRepository;
            _wizardService = wizardService;
            _context = context;
        }

        public DuelResultDto ConductDuel(int wizard1Id, int wizard2Id, string duelType)
        {
            var w1 = _wizardRepository.GetById(wizard1Id);
            var w2 = _wizardRepository.GetById(wizard2Id);

            if (w1 == null || w2 == null) return null;

            var spells1Dto = _wizardService.GetKnownSpellsForWizard(w1.Id).ToList();
            var spells2Dto = _wizardService.GetKnownSpellsForWizard(w2.Id).ToList();

            var spells1 = spells1Dto.Select(s => new SpellEntity { Name = s.Name, Damage = s.Damage, Effect = s.Effect }).ToList();
            var spells2 = spells2Dto.Select(s => new SpellEntity { Name = s.Name, Damage = s.Damage, Effect = s.Effect }).ToList();


            var (winner, loser, log) = SimulateFight(w1, w2, spells1, spells2);


            int stake = duelType == "HouseCup" ? 50 : 25;
            if (duelType == "HouseCup" && w1.House == w2.House)
            {
                stake *= 2;
                log.Add($"[{winner.House}]: Оскільки обидва чарівники з одного дому, ставка подвоюється! ({stake})");
            }

            _wizardService.UpdateWizardRating(winner.Id, stake);
            _wizardService.UpdateWizardRating(loser.Id, -stake);

            var history = new DuelHistory
            {
                DuelId = (_duelCounter++).ToString(),
                WinnerId = winner.Id,
                LoserId = loser.Id,
                RatingStake = stake,
                TurnLog = string.Join("\n  ", log)
            };
            _duelHistoryRepository.Add(history);
            _context.SaveChanges();

            return DuelHistoryMapper.MapToDto(history, winner.Name, loser.Name);
        }

        private (WizardEntity winner, WizardEntity loser, List<string> log) SimulateFight(
            WizardEntity wizard1, WizardEntity wizard2, List<SpellEntity> spells1, List<SpellEntity> spells2)
        {
            var log = new List<string> { $"{wizard1.Name} vs {wizard2.Name} починають." };

            wizard1.Health = 100;
            wizard2.Health = 100;

            var damage1 = spells1.Any() ? spells1.Max(s => s.Damage) : 10;
            var damage2 = spells2.Any() ? spells2.Max(s => s.Damage) : 10;

            if (damage1 > damage2)
            {
                wizard2.Health = 0;
                log.Add($"{wizard1.Name} завдав вирішального удару ({damage1} шкоди).");
                return (wizard1, wizard2, log);
            }
            else
            {
                wizard1.Health = 0;
                log.Add($"{wizard2.Name} завдав вирішального удару ({damage2} шкоди).");
                return (wizard2, wizard1, log);
            }
        }

        public IEnumerable<DuelResultDto> GetWizardDuelHistory(int wizardId)
        {
            var historyEntities = _duelHistoryRepository.GetHistoryByWizardId(wizardId);
            var results = new List<DuelResultDto>();

            foreach (var entity in historyEntities)
            {
                var winner = _wizardRepository.GetById(entity.WinnerId);
                var loser = _wizardRepository.GetById(entity.LoserId);
                results.Add(DuelHistoryMapper.MapToDto(entity, winner.Name, loser.Name));
            }
            return results;
        }
    }
}