using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedIndexDemo
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public override string ToString() => $"[{Id}] {Title}";
    }

    public static class TextProcessor
    {
        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "a", "an", "the", "is", "are", "was", "were", "in", "on", "at", "to", "for", "of", "and", "or"
        };

        public static IEnumerable<string> Tokenize(string text) =>
            Regex.Matches(text.ToLowerInvariant(), @"\w+").Select(m => m.Value);

        public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words) =>
            words.Where(w => !StopWords.Contains(w));
    }

    public class InvertedIndex
    {
        private readonly Dictionary<string, HashSet<int>> _index = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<int, Document> _documents = new();

        public void AddDocument(Document doc)
        {
            _documents[doc.Id] = doc;
            var words = TextProcessor.RemoveStopWords(TextProcessor.Tokenize(doc.Content));
            foreach (var word in words)
            {
                if (!_index.ContainsKey(word))
                    _index[word] = new HashSet<int>();
                _index[word].Add(doc.Id);
            }
        }

        public List<Document> SearchSingleWord(string word)
        {
            string key = word.ToLowerInvariant();
            return _index.TryGetValue(key, out var ids) 
                ? ids.Select(id => _documents[id]).ToList() 
                : new List<Document>();
        }

        public List<Document> SearchMultipleWords(params string[] words)
        {
            HashSet<int>? result = null;
            foreach (var w in words)
            {
                string key = w.ToLowerInvariant();
                if (_index.TryGetValue(key, out var ids))
                {
                    if (result == null) result = new HashSet<int>(ids);
                    else result.IntersectWith(ids);
                }
                else return new List<Document>();
            }
            return result?.Select(id => _documents[id]).ToList() ?? new List<Document>();
        }

        public List<Document> SearchAnyWord(params string[] words)
        {
            var result = new HashSet<int>();
            foreach (var w in words)
            {
                string key = w.ToLowerInvariant();
                if (_index.TryGetValue(key, out var ids))
                    result.UnionWith(ids);
            }
            return result.Select(id => _documents[id]).ToList();
        }

        public int DocumentCount => _documents.Count;
        public int UniqueWords => _index.Count;
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var docs = new List<Document>
            {
                new Document { Id = 1, Title = "Introduction to C#", Content = "C# is a modern object-oriented programming language developed by Microsoft." },
                new Document { Id = 2, Title = "Java Basics", Content = "Java is a popular programming language known for its portability and performance." },
                new Document { Id = 3, Title = "Python Overview", Content = "Python is a high-level programming language with simple syntax." },
                new Document { Id = 4, Title = "C# Advanced Features", Content = "C# supports LINQ, async/await, and many modern programming paradigms." },
                new Document { Id = 5, Title = "Comparing Languages", Content = "C#, Java, and Python are among the most popular programming languages today." }
            };

            var index = new InvertedIndex();
            foreach (var d in docs) index.AddDocument(d);

            Console.WriteLine($"Завантажено документів: {index.DocumentCount}");
            Console.WriteLine($"Унікальних слів: {index.UniqueWords}\n");

            // 1. Пошук AND
            Console.WriteLine("=== Пошук 'programming language' (AND) ===");
            var searchResults = index.SearchMultipleWords("programming", "language");
            foreach (var doc in searchResults)
                Console.WriteLine($"{doc}\n   {doc.Content}");

            // 2. Порівняння продуктивності (на 1000 документів)
            Console.WriteLine("\n=== Порівняння продуктивності (1000 документів) ===");
            var bigDocs = new List<Document>();
            for (int i = 1; i <= 1000; i++)
                bigDocs.Add(new Document { Id = i, Title = $"Doc #{i}", Content = docs[i % 5].Content });

            var sw = Stopwatch.StartNew();
            var linear = bigDocs.Where(d => TextProcessor.Tokenize(d.Content).Contains("programming")).ToList();
            sw.Stop();
            long linearTime = sw.ElapsedTicks;

            var bigIndex = new InvertedIndex();
            foreach (var d in bigDocs) bigIndex.AddDocument(d);

            sw.Restart();
            var indexResult = bigIndex.SearchSingleWord("programming");
            sw.Stop();
            long indexTime = sw.ElapsedTicks;

            Console.WriteLine($"Лінійний пошук: {linearTime} ticks (знайдено {linear.Count})");
            Console.WriteLine($"Inverted Index: {indexTime} ticks (знайдено {indexResult.Count})");
        }
    }
}