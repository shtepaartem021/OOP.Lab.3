namespace DuelingSimulation.Entities
{
    public class WizardEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string House { get; set; }
        public int Health { get; set; }
        public int Rating { get; set; }
    }

    public class SpellEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public string Effect { get; set; }
    }

    public class WizardSpell
    {
        public int Id { get; set; }
        public int WizardId { get; set; }
        public int SpellId { get; set; }
    }
    public class DuelHistory
    {
        public int Id { get; set; }
        public string DuelId { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
        public string TurnLog { get; set; }
        public int RatingStake { get; set; }
    }
}