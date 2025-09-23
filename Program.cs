using static Utils;

internal class Program
{
    private static void Main()
    {
        try
        {
            Console.Clear();
            Console.ResetColor();
            int numTestCases = 3;

            for (int i = 0; i < numTestCases; i++)
            {
                Console.ResetColor();

                HighlightedWriteLine($"Test Case {i + 1}", ConsoleColor.Green);

                Console.WriteLine("");

                Console.WriteLine("1. Get GPA");
                Console.WriteLine("2. Get hourly wage");

                int selection;
                while (true)
                {
                    Console.ResetColor();

                    HighlightedWriteLine("Select a number to proceed: ", ConsoleColor.Yellow);
                    string? input = Console.ReadLine();

                    if (!int.TryParse(input, out selection) || (selection != 1 && selection != 2))
                    {
                        HighlightedWriteLine("Invalid input.", ConsoleColor.Red);
                        continue;
                    }
                    break;
                }

                switch (selection)
                {
                    case 1:
                        {
                            Student student = new("Adrian Seth", "Tabotabo");
                            student.Introduce();
                            Console.WriteLine($"GPA: {student.GPA:F2}");
                            Console.WriteLine(
                                $"Marks: [ {string.Join(", ", student.Marks.Select(m => $"{m.Grade:F2}"))} ]"
                            );
                        }
                        break;

                    case 2:
                        {
                            Worker worker = new("Jeff", "Mondigo");
                            worker.Introduce();
                            Console.WriteLine($"Hourly Wage: {worker.CalculateHourlyWage:F2}");
                        }
                        break;

                    default:
                        {
                            Console.WriteLine("Choice not yet set.");
                        }
                        break;
                }

                Console.WriteLine("");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}

public abstract class Human
{
    private protected string firstName;
    private protected string lastName;

    public virtual void Introduce()
    {
        Console.WriteLine($"First name: {firstName}");
        Console.WriteLine($"Last name: {lastName}");
    }

    public Human(string firstName, string lastName)
    {
        this.firstName = firstName;
        this.lastName = lastName;
    }
}

internal class Student : Human
{
    private List<(float Grade, int Unit)> grades = [];
    private bool isSet;

    public override void Introduce()
    {
        base.Introduce();

        Console.WriteLine(
            $"Grades: [ {string.Join(", ", grades.Select(g => $"({g.Grade:F2}, {g.Unit})"))} ]"
        ); // (Grade1, Unit1), (Grade2, Unit2), ...
    }

    public IReadOnlyList<(float Grade, int Unit)> Marks
    {
        get => grades;
    }

    public void InputGrades()
    {
        Console.Write("Enter 'grade,unit' separated by '|' (pipe): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            HighlightedWriteLine("Empty Input.", ConsoleColor.Red);
            return;
        }

        List<string> inputGrades = [.. input.Split('|')]; // Split the input with " " as delimiter
        List<(float Grade, int Units)> _grades = []; // Buffer

        foreach (string inputGrade in inputGrades)
        {
            Console.ResetColor();

            List<string> parts = [.. inputGrade.Split(',')];

            if (parts.Count != 2)
            {
                HighlightedWriteLine($"Invalid input: {inputGrade}", ConsoleColor.Red);
                return;
            }

            if (!float.TryParse(parts[0], out float Grade))
            {
                HighlightedWriteLine(
                    $"Invalid input: {parts[0]} in {inputGrade}",
                    ConsoleColor.Red
                );
                return;
            }

            if (!int.TryParse(parts[1], out int Units))
            {
                HighlightedWriteLine(
                    $"Invalid input: {parts[1]} in {inputGrade}",
                    ConsoleColor.Red
                );
                return;
            }

            _grades.Add((Grade, Units));
        }

        grades = _grades;
        isSet = true;
    }

    public float GPA
    {
        get
        {
            Console.ResetColor();

            if (!isSet)
            {
                HighlightedWriteLine("Grades not yet set!", ConsoleColor.DarkYellow);
                InputGrades();
            }

            float totalGrade = 0;
            int totalUnits = 0;

            foreach ((float Grade, int Units) in grades)
            {
                totalGrade += Grade * Units;
                totalUnits += Units;
            }

            // Return 0 if totalUnits is 0, else return something
            return totalUnits == 0 ? 0 : totalGrade / totalUnits;
        }
    }

    public Student(string firstName, string lastName, List<(float Grade, int Units)> grade)
        : base(firstName, lastName)
    {
        grades = grade;
        isSet = true;
    }

    public Student(string firstName, string lastName)
        : base(firstName, lastName)
    {
        isSet = false;
    }
}

internal class Worker : Human
{
    private float wage;
    private float hoursWorked;

    bool isSet;

    public override void Introduce()
    {
        base.Introduce();

        Console.WriteLine($"Wage: {wage:F2}");
        Console.WriteLine($"Hours Worked: {hoursWorked:F2}");
        Console.WriteLine($"Hourly Wage: {CalculateHourlyWage:F2}");
    }

    public void InputValues()
    {
        while (true)
        {
            Console.ResetColor();

            Console.Write("Enter wage: ");
            string? input = Console.ReadLine();
            if (!float.TryParse(input, out wage))
            {
                HighlightedWriteLine("Invalid input. Please input a valid wage.", ConsoleColor.Red);
                continue;
            }
            break;
        }

        while (true)
        {
            Console.ResetColor();

            Console.Write("Enter hours worked: ");
            string? input = Console.ReadLine();
            if (!float.TryParse(input, out hoursWorked))
            {
                HighlightedWriteLine(
                    "Invalid input. Please input a valid hours worked.",
                    ConsoleColor.Red
                );
                continue;
            }
            break;
        }

        isSet = true;
    }

    public float CalculateHourlyWage
    {
        get
        {
            if (!isSet)
            {
                HighlightedWriteLine("Wage and Hours Worked not yet set!", ConsoleColor.DarkYellow);
                InputValues();
            }

            // If hoursWorked == 0, return 0
            return hoursWorked == 0 ? 0 : wage / hoursWorked;
        }
    }

    public Worker(string firstName, string lastName, float wage, float hoursWorked)
        : base(firstName, lastName)
    {
        this.wage = wage;
        this.hoursWorked = hoursWorked;
        isSet = true;
    }

    public Worker(string firstName, string lastName)
        : base(firstName, lastName)
    {
        isSet = false;
    }
}

public class Utils
{
    public static void HighlightedWriteLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void HighlightedWrite(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
}
