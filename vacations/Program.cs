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
        int size = totalDays - currentDays > 7 ? v.GetVacationSize() : 7;
        bool vacationCorrect = false;

        DateTime[] startAndEnd = v.GetVacationStartEnd(size);
        List<DateTime> vacationDates = new List<DateTime>();

        while (!vacationCorrect)
        {
            vacationDates = v.GetVacationDatesList(startAndEnd[0], startAndEnd[1]);
            vacationCorrect = CheckIfVacationCorrect(vacationDates);
        }

        workerVacationDates = workerVacationDates.Concat(vacationDates).ToList();

        currentDays += size;

        bool CheckIfVacationCorrect(List<DateTime> list)
        {
            bool firstCheck = true;
            bool secondCheck = true;
            foreach (DateTime vac in list)
            {
                firstCheck = allVacations.Count == 0 || allVacations.All(x => x != vac.AddDays(-3));
                secondCheck = workerVacationDates.Count == 0 || (workerVacationDates.Any(x => x.AddMonths(1) >= vac) && list.Any(x => x.AddMonths(-1) <= vac));
            }

            return firstCheck && secondCheck;
        }
    }

    allVacations = allVacations.Concat(workerVacationDates).ToList();
}

foreach (var item in allVacations)
{
    Console.WriteLine(item);
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