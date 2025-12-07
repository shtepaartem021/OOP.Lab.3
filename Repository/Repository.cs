using DuelingSimulation.Data;
using DuelingSimulation.Entities;
using DuelingSimulation.Repository.Base;
using System.Collections.Generic;
using System.Linq;

namespace DuelingSimulation.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly HogwartsDbContext _context;
        protected readonly List<T> _dbSet;

        public BaseRepository(HogwartsDbContext context, List<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public T GetById(int id) => _dbSet.OfType<dynamic>().FirstOrDefault(e => e.Id == id);
        public IEnumerable<T> GetAll() => _dbSet;
        public void Add(T entity) => _dbSet.Add(entity);
        public void Update(T entity) { /* Оновлення по посиланню */ }
        public void Delete(T entity) => _dbSet.Remove(entity);
    }

    public class WizardRepository : BaseRepository<WizardEntity>, IWizardRepository
    {
        public WizardRepository(HogwartsDbContext context) : base(context, context.Wizards) { }
    }

    public class SpellRepository : BaseRepository<SpellEntity>, ISpellRepository
    {
        public SpellRepository(HogwartsDbContext context) : base(context, context.Spells) { }
    }

    public class WizardSpellRepository : BaseRepository<WizardSpell>, IWizardSpellRepository
    {
        public WizardSpellRepository(HogwartsDbContext context) : base(context, context.WizardSpells) { }

        public IEnumerable<int> GetSpellIdsForWizard(int wizardId)
        {
            return _dbSet.Where(ws => ws.WizardId == wizardId)
                         .Select(ws => ws.SpellId)
                         .ToList();
        }
    }

    public class DuelHistoryRepository : BaseRepository<DuelHistory>, IDuelHistoryRepository
    {
        public DuelHistoryRepository(HogwartsDbContext context) : base(context, context.DuelHistories) { }

        public IEnumerable<DuelHistory> GetHistoryByWizardId(int wizardId)
        {
            return _dbSet.Where(d => d.WinnerId == wizardId || d.LoserId == wizardId);
        }
    }
}