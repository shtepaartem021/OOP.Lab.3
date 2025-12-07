using DuelingSimulation.Data;
using DuelingSimulation.Repository;
using DuelingSimulation.Service;
using DuelingSimulation.Service.Base;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.WriteLine("Симуляція Дуелей Поттеріани");
        Console.WriteLine("============================\n");

        var context = new HogwartsDbContext();

        var wizardRepo = new WizardRepository(context);
        var historyRepo = new DuelHistoryRepository(context);

        var spellRepo = new SpellRepository(context);
        var wsRepo = new WizardSpellRepository(context);

        var spellService = new SpellService(spellRepo);

        var wizardService = new WizardService(wizardRepo, wsRepo, spellService, context);

        var duelService = new DuelService(wizardRepo, historyRepo, wizardService, context);

        int harryId = 1;
        int dracoId = 2;
        int hermioneId = 3;

        Console.WriteLine("--- Дуель 1: Стандартна (Гаррі vs Драко) ---");
        var duel1 = duelService.ConductDuel(harryId, dracoId, "Standard");
        Console.WriteLine($"\nРезультат Дуелі #{duel1.DuelId}: {duel1.WinnerName} ПЕРЕМІГ {duel1.LoserName} (Ставка: {duel1.RatingStake})\n");

        Console.WriteLine("--- Дуель 2: Кубок Дому (Гаррі vs Герміона - обидва Грифіндор) ---");
        var duel2 = duelService.ConductDuel(harryId, hermioneId, "HouseCup");
        Console.WriteLine($"\nРезультат Дуелі #{duel2.DuelId}: {duel2.WinnerName} ПЕРЕМІГ {duel2.LoserName} (Ставка: {duel2.RatingStake})\n");

        Console.WriteLine("--- Дуель 3: Стандартна (Драко vs Герміона) ---");
        var duel3 = duelService.ConductDuel(dracoId, hermioneId, "Standard");
        Console.WriteLine($"\nРезультат Дуелі #{duel3.DuelId}: {duel3.WinnerName} ПЕРЕМІГ {duel3.LoserName} (Ставка: {duel3.RatingStake})\n");

        Console.WriteLine("\n====================================");
        Console.WriteLine("Звіти про історію дуелей та Рейтинг");
        Console.WriteLine("====================================\n");

        var harryProfile = wizardService.GetWizardProfile(harryId);
        Console.WriteLine($"\nІсторія дуелей чарівника {harryProfile.Name} (Рейтинг: {harryProfile.Rating}):");
        foreach (var duel in duelService.GetWizardDuelHistory(harryId))
        {
            Console.WriteLine($"Дуель #{duel.DuelId} (Ставка: {duel.RatingStake}): {duel.WinnerName} переміг {duel.LoserName}");
        }

        var dracoProfile = wizardService.GetWizardProfile(dracoId);
        Console.WriteLine($"\nІсторія дуелей чарівника {dracoProfile.Name} (Рейтинг: {dracoProfile.Rating}):");
        foreach (var duel in duelService.GetWizardDuelHistory(dracoId))
        {
            Console.WriteLine($"Дуель #{duel.DuelId} (Ставка: {duel.RatingStake}): {duel.WinnerName} переміг {duel.LoserName}");
        }

        var hermioneProfile = wizardService.GetWizardProfile(hermioneId);
        Console.WriteLine($"\nІсторія дуелей чарівника {hermioneProfile.Name} (Рейтинг: {hermioneProfile.Rating}):");
        foreach (var duel in duelService.GetWizardDuelHistory(hermioneId))
        {
            Console.WriteLine($"Дуель #{duel.DuelId} (Ставка: {duel.RatingStake}): {duel.WinnerName} переміг {duel.LoserName}");
        }
    }
}
