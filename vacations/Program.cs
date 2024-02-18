using System.ComponentModel.DataAnnotations;

List<DateTime> allVacations = new List<DateTime>();
List<string> workers = new List<string>()
{
        "Иванов Иван Иванович",
        "Петров Петр Петрович",
        "Юлина Юлия Юлиановна",
        "Сидоров Сидор Сидорович",
        "Павлов Павел Павлович",
        "Георгиев Георг Георгиевич"
};

foreach (string worker in workers)
{

    List<DateTime> workerVacationDates = new List<DateTime>();
    int currentDays = 0;
    Vacation v = new Vacation(worker);
    int totalDays = Vacation.VacationDays;

    while (currentDays < totalDays)
    {
        List<DateTime> vacationDates = new List<DateTime>();
        bool vacationCorrect = false;
        int size = 0;
        DateTime start = new DateTime();
        DateTime end = new DateTime();

        while (!vacationCorrect)
        {
            size = totalDays - currentDays > 7 ? v.GetVacationSize() : 7;

            DateTime[] startAndEnd = v.GetVacationStartEnd(size);
            start = startAndEnd[0];
            end = startAndEnd[1];

            vacationCorrect = CheckIfVacationCorrect(start, end);
        }

        vacationDates = v.GetVacationDatesList(start, end);
        workerVacationDates = workerVacationDates.Concat(vacationDates).ToList();
        allVacations = allVacations.Concat(vacationDates).ToList();

        currentDays += size;

        bool CheckIfVacationCorrect(DateTime start, DateTime end)
        {
            bool firstCheck = true;
            bool secondCheck = true;
            if (allVacations.Count == 0 && workerVacationDates.Count == 0) return true;
            else
            {
                firstCheck = !allVacations.Any(x => x.AddDays(3) >= start && x.AddDays(3) <= end);
                secondCheck = !(workerVacationDates.Any(x => x.AddMonths(1) >= end) && workerVacationDates.Any(x => x.AddMonths(-1) <= start));
            }

            return firstCheck && secondCheck;
        }

    }

    PrintVacations(v.WorkerName, workerVacationDates);

    void PrintVacations(string worker, List<DateTime> vacations)
    {
        Console.WriteLine($"Дни отпуска {worker}:");
        foreach (DateTime vac in vacations)
        {
            Console.WriteLine(vac.ToShortDateString());
        }
        Console.WriteLine();
    }

}

class Vacation
{
    public List<string> Weekends { get; } = new List<string> { "Saturday", "Sunday" };
    public const int VacationDays = 28;
    private int[] VacationSize { get; } = [7, 14];
    private DateTime Start { get; } = new DateTime(DateTime.Now.Year, 1, 1);
    private DateTime End { get; } = new DateTime(DateTime.Now.Year, 12, 31);
    public string WorkerName;
    public Random Seed;

    public Vacation(string workerName)
    {
        WorkerName = workerName;
        Seed = new Random();
    }

    private int GetDaysOfYear() => (End - Start).Days;

    public int GetVacationSize() => VacationSize[Seed.Next(2)];

    public DateTime[] GetVacationStartEnd(int vacationSize)
    {
        int range = GetDaysOfYear();
        DateTime vacationStart;
        DateTime vacationEnd;

        do
        {
            vacationStart = Start.AddDays(Seed.Next(range));
            vacationEnd = vacationStart.AddDays(vacationSize);
        } while (Weekends.Contains(vacationStart.DayOfWeek.ToString()));

        return new DateTime[] { vacationStart, vacationEnd };
    }

    public List<DateTime> GetVacationDatesList(DateTime start, DateTime end)
    {
        List<DateTime> result = new List<DateTime>();
        for (DateTime date = start; date < end; date = date.AddDays(1))
        {
            result.Add(date);
        }
        return result;
    }
}