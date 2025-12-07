namespace DuelingSimulation.Service
{
    public class SpellDto
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public string Effect { get; set; }
    }

    public class WizardProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string House { get; set; }
        public int Rating { get; set; }
        public List<SpellDto> KnownSpells { get; set; }
    }

    public class DuelResultDto
    {
        public string DuelId { get; set; }
        public string WinnerName { get; set; }
        public string LoserName { get; set; }
        public int RatingStake { get; set; }
        public string TurnLogSummary { get; set; }
    }
}