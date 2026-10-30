using System;
using System.Collections.Generic;

namespace lab9_self_v1
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public static class CollectionUtils
    {
        public static IEnumerable<TResult> Map<TSource, TResult>(
            IEnumerable<TSource> collection,
            Func<TSource, TResult> mapper)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            var result = new List<TResult>();
            foreach (var item in collection)
            {
                result.Add(mapper(item));
            }
            return result;
        }

        public static T MaxBy<T, TKey>(
            IEnumerable<T> collection,
            Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            T? maxItem = default;
            TKey? maxKey = default;
            bool first = true;

            foreach (var item in collection)
            {
                TKey key = keySelector(item);
                if (first || key.CompareTo(maxKey!) > 0)
                {
                    maxKey = key;
                    maxItem = item;
                    first = false;
                }
            }

            if (first)
            {
                throw new InvalidOperationException("Колекція порожня.");
            }

            return maxItem!;
        }

        public static IEnumerable<T> Filter<T>(
            IEnumerable<T> collection,
            Predicate<T> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var result = new List<T>();
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static void ForEach<T>(
            IEnumerable<T> collection,
            Action<T> action)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in collection)
            {
                action(item);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. Колекція об'єктів (List<Product>) ===");
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 35000 },
                new Product { Name = "Mouse", Price = 750 },
                new Product { Name = "Keyboard", Price = 2200 },
                new Product { Name = "Monitor", Price = 12000 }
            };

            var productNames = CollectionUtils.Map(products, p => p.Name);
            Console.WriteLine("Назви товарів: " + string.Join(", ", productNames));

            var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
            Console.WriteLine($"Найдорожчий товар: {mostExpensive.Name} ({mostExpensive.Price} грн)");

            var expensiveProducts = CollectionUtils.Filter(products, p => p.Price > 2000);
            Console.WriteLine("Товари дорожчі за 2000 грн:");
            CollectionUtils.ForEach(expensiveProducts, p => Console.WriteLine($" - {p.Name}: {p.Price} грн"));


            Console.WriteLine("\n=== 2. Числова колекція (List<int>) ===");
            var numbers = new List<int> { 5, 12, 3, 8, 25, 42, 1, 19 };

            var squared = CollectionUtils.Map(numbers, x => x * x);
            Console.WriteLine("Квадрати чисел: " + string.Join(", ", squared));

            var maxNumber = CollectionUtils.MaxBy(numbers, x => x);
            Console.WriteLine($"Максимальне число: {maxNumber}");

            var evenNumbers = CollectionUtils.Filter(numbers, x => x % 2 == 0);
            Console.WriteLine("Парні числа:");
            CollectionUtils.ForEach(evenNumbers, x => Console.Write($"{x} "));
            Console.WriteLine();


            Console.WriteLine("\n=== 3. Текстова колекція (List<string>) ===");
            var words = new List<string> { "C#", "NetCore", "Delegates", "Lambda", "Polymorphism" };

            var wordLengths = CollectionUtils.Map(words, w => $"{w} ({w.Length} симв.)");
            Console.WriteLine("Довжини слів: " + string.Join(", ", wordLengths));

            var longestWord = CollectionUtils.MaxBy(words, w => w.Length);
            Console.WriteLine($"Найдовше слово: {longestWord}");

            var longWords = CollectionUtils.Filter(words, w => w.Length > 6);
            Console.WriteLine("Слова довші за 6 символів:");
            CollectionUtils.ForEach(longWords, w => Console.WriteLine($" - {w}"));
        }
    }
}