namespace IndependentWork4;

public class AudioBook : Book
{
    private string _narrator;

    public string Narrator => _narrator;

    public AudioBook(string uniqueId, string title, int publicationYear, string author, string narrator)
        : base(uniqueId, title, publicationYear, author)
    {
        _narrator = narrator;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"   Оповідач: {Narrator}");
    }
}