using DuelingSimulation.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DuelingSimulation.Data
{
    public class HogwartsDbContext
    {
        public List<WizardEntity> Wizards { get; set; }
        public List<SpellEntity> Spells { get; set; }
        public List<WizardSpell> WizardSpells { get; set; }
        public List<DuelHistory> DuelHistories { get; set; }

        public HogwartsDbContext()
        {
            Wizards = new List<WizardEntity>();
            Spells = new List<SpellEntity>();
            WizardSpells = new List<WizardSpell>();
            DuelHistories = new List<DuelHistory>();
            SeedData();
        }

        private void SeedData()
        {
            Spells.Add(new SpellEntity { Id = 1, Name = "Експеліармус", Damage = 15, Effect = "Disarming" });
            Spells.Add(new SpellEntity { Id = 2, Name = "Ступефай", Damage = 20, Effect = "Stunning" });
            Spells.Add(new SpellEntity { Id = 3, Name = "Петрифікус Тоталус", Damage = 25, Effect = "Damage" });

            Wizards.Add(new WizardEntity { Id = 1, Name = "Гаррі Поттер", House = "Грифіндор", Health = 100, Rating = 1000 });
            Wizards.Add(new WizardEntity { Id = 2, Name = "Драко Малфой", House = "Слизерин", Health = 100, Rating = 1000 });
            Wizards.Add(new WizardEntity { Id = 3, Name = "Герміона Ґрейнджер", House = "Грифіндор", Health = 100, Rating = 1000 });

            int wsId = 1;
            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 1, SpellId = 1 });
            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 1, SpellId = 2 });

            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 2, SpellId = 3 });
            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 2, SpellId = 2 });
            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 2, SpellId = 1 });

            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 3, SpellId = 1 });
            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 3, SpellId = 3 });
            WizardSpells.Add(new WizardSpell { Id = wsId++, WizardId = 3, SpellId = 2 });
        }

        public void SaveChanges()
        {
            if (DuelHistories.Any())
            {
                int maxId = DuelHistories.Max(d => d.Id);
                foreach (var history in DuelHistories.Where(d => d.Id == 0))
                {
                    history.Id = ++maxId;
                }
            }
        }
    }
}