using System;
using System.Text;

namespace IndependentWork4;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Система управління бібліотекою ===\n");

        Book book = new Book("B-101", "1984", 1949, "Джордж Орвелл");
        Journal journal = new Journal("J-202", "National Geographic", 2023, 5);
        AudioBook audioBook = new AudioBook("A-303", "Хоббіт", 1937, "Дж. Р. Р. Толкін", "Енді Серкіс");

        Console.WriteLine("--- Книга ---");
        book.DisplayDetails();

        Console.WriteLine("\n--- Журнал ---");
        journal.DisplayDetails();

        Console.WriteLine("\n--- Аудіокнига ---");
        audioBook.DisplayDetails();
    }
}