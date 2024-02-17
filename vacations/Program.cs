List<DateTime> AllVacations = new List<DateTime>();
List<string> Workers = new List<string>()
{
        "Иванов Иван Иванович",
        "Петров Петр Петрович",
        "Юлина Юлия Юлиановна",
        "Сидоров Сидор Сидорович",
        "Павлов Павел Павлович",
        "Георгиев Георг Георгиевич"
};



class Vacation
{
    enum WorkingDaysOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }
    public const int VacationDays = 28;

    public int[] VacationSize { get; } = [7, 14];
    public DateTime Start { get; } = new DateTime(DateTime.Now.Year, 1, 1);
    public DateTime End { get; } = new DateTime(DateTime.Now.Year, 12, 31);
    public List<DateTime> WorkerVacations = new List<DateTime>();
    public string WorkerName;
    public Random Seed;

    public Vacation(string workerName)
    {
        WorkerName = workerName;
        Seed = new Random();
    }

    private int GetDaysOfYear() => (End - Start).Days;

    private int GetVacationSize() => VacationSize[Seed.Next(2)]; 

    private List<DateTime> GetStartAndEndOfVacation(int range)
    {
        DateTime vacationStart = Start.AddDays(Seed.Next(range));
        DateTime vacationEnd = vacationStart.AddDays(GetVacationSize());
        return new List<DateTime>();
    }
}