using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork11
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal UnitPrice { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string City { get; set; } = "";
    }

    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Генерація тестових даних (60 товарів, 80 клієнтів, 200 продажів)
            var rand = new Random(42);
            var categories = new[] { "Електроніка", "Одяг", "Книги", "Побутова техніка", "Спорт" };
            var cities = new[] { "Київ", "Львів", "Одеса", "Дніпро", "Харків" };

            var products = Enumerable.Range(1, 60).Select(i => new Product
            {
                Id = i,
                Name = $"Товар #{i}",
                Category = categories[rand.Next(categories.Length)],
                UnitPrice = rand.Next(100, 5000)
            }).ToList();

            var customers = Enumerable.Range(1, 80).Select(i => new Customer
            {
                Id = i,
                Name = $"Клієнт #{i}",
                City = cities[rand.Next(cities.Length)]
            }).ToList();

            var sales = Enumerable.Range(1, 200).Select(i => new Sale
            {
                Id = i,
                ProductId = rand.Next(1, 61),
                CustomerId = rand.Next(1, 81),
                Quantity = rand.Next(1, 10),
                SaleDate = DateTime.Now.AddDays(-rand.Next(1, 365))
            }).ToList();

            Console.WriteLine($"Дані завантажено: {products.Count} товарів, {customers.Count} клієнтів, {sales.Count} продажів.\n");

            // Пайплайн 1: ТОП-3 найпопулярніші продукти за категоріями
            var sw = Stopwatch.StartNew();
            var p1 = sales
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalQty = g.Sum(s => s.Quantity) })
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { p.Name, p.Category, s.TotalQty })
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, Top = g.OrderByDescending(x => x.TotalQty).Take(3) });
            sw.Stop();

            Console.WriteLine($"=== 1. ТОП продуктів за категоріями ({sw.ElapsedTicks} ticks) ===");
            foreach (var cat in p1)
            {
                Console.WriteLine($"Категорія: {cat.Category}");
                foreach (var prod in cat.Top)
                    Console.WriteLine($"  - {prod.Name}: {prod.TotalQty} шт.");
            }

            // Пайплайн 2: Загальний дохід за містами клієнтів
            sw.Restart();
            var p2 = sales
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s, p })
                .Join(customers, sp => sp.s.CustomerId, c => c.Id, (sp, c) => new { c.City, Revenue = sp.s.Quantity * sp.p.UnitPrice })
                .GroupBy(x => x.City)
                .Select(g => new { City = g.Key, TotalRevenue = g.Sum(x => x.Revenue) })
                .OrderByDescending(x => x.TotalRevenue);
            sw.Stop();

            Console.WriteLine($"\n=== 2. Дохід за містами ({sw.ElapsedTicks} ticks) ===");
            foreach (var item in p2)
                Console.WriteLine($"  {item.City,-10}: {item.TotalRevenue,10:N0} грн");

            // Пайплайн 3: Продажі за роками та місяцями
            sw.Restart();
            var p3 = sales
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.SaleDate.Year, s.SaleDate.Month, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
                .GroupBy(x => new { x.Year, x.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, TotalUnits = g.Sum(x => x.Quantity), TotalRevenue = g.Sum(x => x.Revenue) })
                .OrderBy(x => x.Year).ThenBy(x => x.Month);
            sw.Stop();

            Console.WriteLine($"\n=== 3. Продажі за місяцями ({sw.ElapsedTicks} ticks) ===");
            foreach (var item in p3)
                Console.WriteLine($"  {item.Year}-{item.Month:D2}: {item.TotalUnits} шт. | {item.TotalRevenue:N0} грн");

            // Пайплайн 4 (Власний): ТОП-5 клієнтів за сумою покупок
            sw.Restart();
            var p4 = sales
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Value = s.Quantity * p.UnitPrice })
                .Join(customers, sp => sp.CustomerId, c => c.Id, (sp, c) => new { c.Name, c.City, sp.Value })
                .GroupBy(x => new { x.Name, x.City })
                .Select(g => new { g.Key.Name, g.Key.City, TotalSpent = g.Sum(x => x.Value) })
                .OrderByDescending(x => x.TotalSpent)
                .Take(5);
            sw.Stop();

            Console.WriteLine($"\n=== 4. ТОП-5 клієнтів ({sw.ElapsedTicks} ticks) ===");
            foreach (var c in p4)
                Console.WriteLine($"  {c.Name} ({c.City}): {c.TotalSpent:N0} грн");
        }
    }
}