namespace IndependentWork1;

public class Book
{
    private string _title;
    private int _totalPages;
    private int _readPages;

    public string Title => _title;
    public int TotalPages => _totalPages;

    public int ReadPages
    {
        get => _readPages;
        set
        {
            if (value >= 0 && value <= _totalPages)
            {
                _readPages = value;
            }
        }
    }

    public Book(string title, int totalPages, int readPages = 0)
    {
        _title = title;
        _totalPages = totalPages > 0 ? totalPages : 1;
        ReadPages = readPages;
    }

    public double GetReadingProgress()
    {
        return ((double)_readPages / _totalPages) * 100.0;
    }

    public bool IsFinished()
    {
        return _readPages >= _totalPages;
    }
}