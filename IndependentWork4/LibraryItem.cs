namespace IndependentWork4;

public class LibraryItem
{
    private string _uniqueId;
    private string _title;
    private int _publicationYear;

    public string UniqueId => _uniqueId;
    public string Title => _title;
    public int PublicationYear => _publicationYear;

    public LibraryItem(string uniqueId, string title, int publicationYear)
    {
        _uniqueId = uniqueId;
        _title = title;
        _publicationYear = publicationYear;
    }

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"[ID: {UniqueId}] Назва: \"{Title}\", Рік видання: {PublicationYear}");
    }
}