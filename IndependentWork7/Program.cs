using System;

namespace IndependentWork7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи Cache<TKey, TValue> (FIFO) ===");

            Cache<string, string> cache = new Cache<string, string>(3);

            Console.WriteLine("Додаємо елементи: Key1, Key2, Key3");
            cache.Add("Key1", "Значення 1");
            cache.Add("Key2", "Значення 2");
            cache.Add("Key3", "Значення 3");

            Console.WriteLine($"Поточна кількість елементів: {cache.Count}");
            Console.WriteLine($"Key1 є в кеші: {cache.ContainsKey("Key1")}");

            Console.WriteLine("\nДодаємо Key4 (кеш повний, Key1 повинен витіснитися за політикою FIFO)...");
            cache.Add("Key4", "Значення 4");

            Console.WriteLine($"Поточна кількість елементів: {cache.Count}");
            Console.WriteLine($"Key1 є в кеші: {cache.ContainsKey("Key1")}");
            Console.WriteLine($"Key4 є в кеші: {cache.ContainsKey("Key4")}");
            Console.WriteLine($"Значення Key4: {cache.Get("Key4")}");

            Console.WriteLine("\nДемонстрація виконана успішно!");
        }
    }
}