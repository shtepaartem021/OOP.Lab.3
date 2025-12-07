using DuelingSimulation.Entities;
using System.Collections.Generic;

namespace DuelingSimulation.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public interface IWizardRepository : IRepository<WizardEntity>
    {
        // Додаткові методи CRUD, якщо потрібні, але не складні JOIN-запити.
    }

    public interface ISpellRepository : IRepository<SpellEntity>
    {
        // Наразі достатньо базових CRUD-методів
    }

    public interface IWizardSpellRepository : IRepository<WizardSpell>
    {
        IEnumerable<int> GetSpellIdsForWizard(int wizardId);
    }
    public interface IDuelHistoryRepository : IRepository<DuelHistory>
    {
        IEnumerable<DuelHistory> GetHistoryByWizardId(int wizardId);
    }
}