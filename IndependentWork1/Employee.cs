namespace IndependentWork1;

public class Employee
{
    private string _fullName;
    private double _monthlySalary;

    public string FullName
    {
        get { return _fullName; }
    }

    public double MonthlySalary
    {
        get { return _monthlySalary; }
        private set
        {
            if (value >= 0)
                _monthlySalary = value;
        }
    }

    public Employee(string fullName, double monthlySalary)
    {
        _fullName = fullName;
        _monthlySalary = monthlySalary >= 0 ? monthlySalary : 0;
    }

    public double CalculateAnnualIncome(double bonusPercentage)
    {
        double baseAnnual = _monthlySalary * 12;
        double bonus = baseAnnual * (bonusPercentage / 100.0);
        return baseAnnual + bonus;
    }
}