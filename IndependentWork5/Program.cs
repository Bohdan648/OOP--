using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IndependentWork5
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"[Товар]: {Name}, Ціна: {Price} грн";
        }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"[Клієнт]: {Name} ({Email})";
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }

        public Order(int orderId, decimal totalAmount)
        {
            OrderId = orderId;
            TotalAmount = totalAmount;
        }

        public override string ToString()
        {
            return $"[Замовлення №{OrderId}]: Сума = {TotalAmount} грн";
        }
    }

    public class MyCustomCollection : IEnumerable<string>
    {
        private readonly List<string> _items = new List<string>();

        public void Add(string item)
        {
            _items.Add(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        public static void ProcessStream(Stream stream)
        {
            Console.WriteLine($"\n--- Обробка потоку типу: {stream.GetType().Name} ---");
            Console.WriteLine($"Чи підтримує читання: {stream.CanRead}");
            Console.WriteLine($"Довжина потоку: {stream.Length} байтів");

            byte[] buffer = new byte[stream.Length];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Прочитано байтів: {bytesRead}");
            Console.WriteLine($"Вміст потоку: \"{content}\"");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== 1. Поліморфізм через System.Object ===");
            List<object> objects = new List<object>
            {
                new Product("Ноутбук", 35000m),
                new Customer("Богдан", "bohdan@example.com"),
                new Order(101, 35000m)
            };

            foreach (object obj in objects)
            {
                Console.WriteLine(obj.ToString());
            }

            Console.WriteLine("\n=== 2. Поліморфізм через IEnumerable<T> ===");
            MyCustomCollection myCollection = new MyCustomCollection
            {
                "Перший елемент",
                "Другий елемент",
                "Третій елемент"
            };

            foreach (string item in myCollection)
            {
                Console.WriteLine($"Елемент колекції: {item}");
            }

            Console.WriteLine("\n=== 3. Поліморфізм через абстрактний клас Stream ===");

            string testFilePath = "test_file.txt";
            File.WriteAllText(testFilePath, "Тестові дані для FileStream");

            using (FileStream fileStream = File.OpenRead(testFilePath))
            {
                ProcessStream(fileStream);
            }

            byte[] memoryData = Encoding.UTF8.GetBytes("Тестові дані для MemoryStream");
            using (MemoryStream memoryStream = new MemoryStream(memoryData))
            {
                ProcessStream(memoryStream);
            }

            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }

            Console.WriteLine("\nВсі сценарії успішно виконані!");
        }
    }
}