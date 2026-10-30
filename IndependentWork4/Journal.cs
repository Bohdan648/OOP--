namespace IndependentWork4;

public class Journal : LibraryItem
{
    private int _issueNumber;

    public int IssueNumber => _issueNumber;

    public Journal(string uniqueId, string title, int publicationYear, int issueNumber)
        : base(uniqueId, title, publicationYear)
    {
        _issueNumber = issueNumber;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"   Номер випуску: #{IssueNumber}");
    }
}