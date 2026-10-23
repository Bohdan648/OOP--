using System;

namespace IndependentWork1;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== ДЕМОНСТРАЦІЯ РОБОТИ КЛАСІВ ===\n");

        Employee employee = new Employee("Олексій Коваленко", 25000.0);
        double annualIncome = employee.CalculateAnnualIncome(10.0);
        Console.WriteLine($"[Employee] Працівник: {employee.FullName}");
        Console.WriteLine($"[Employee] Оклад: {employee.MonthlySalary} грн/міс");
        Console.WriteLine($"[Employee] Річний дохід з 10% бонусом: {annualIncome:F2} грн\n");

        Rectangle rectangle = new Rectangle(5.5, 10.0);
        Console.WriteLine($"[Rectangle] Прямокутник: {rectangle.Width} x {rectangle.Height}");
        Console.WriteLine($"[Rectangle] Площа: {rectangle.CalculateArea()}");
        Console.WriteLine($"[Rectangle] Периметр: {rectangle.CalculatePerimeter()}\n");

        Book book = new Book("Кобзар", 350, 175);
        Console.WriteLine($"[Book] Назва: '{book.Title}'");
        Console.WriteLine($"[Book] Прочитано: {book.ReadPages} з {book.TotalPages} сторінок");
        Console.WriteLine($"[Book] Прогрес читання: {book.GetReadingProgress():F1}%");
        Console.WriteLine($"[Book] Завершено читання: {(book.IsFinished() ? "Так" : "Ні")}");
    }
}