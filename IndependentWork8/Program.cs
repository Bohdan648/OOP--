using System;
using System.Collections.Generic;

namespace IndependentWork8
{
    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; } = string.Empty;
    }

    public class FileMetadataCache
    {
        private readonly Dictionary<string, FileMetadata> _cache = new Dictionary<string, FileMetadata>(StringComparer.OrdinalIgnoreCase);

        public void Add(string key, FileMetadata metadata)
        {
            _cache[key] = metadata;
        }

        public FileMetadata? Get(string key)
        {
            return _cache.TryGetValue(key, out var metadata) ? metadata : null;
        }

        public bool Remove(string key)
        {
            return _cache.Remove(key);
        }

        public bool Contains(string key)
        {
            return _cache.ContainsKey(key);
        }
    }

    public class TagManager
    {
        private readonly Dictionary<string, HashSet<string>> _articleTags = new Dictionary<string, HashSet<string>>();

        public void AddTag(string articleId, string tag)
        {
            if (!_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            _articleTags[articleId].Add(tag);
        }

        public void RemoveTag(string articleId, string tag)
        {
            if (_articleTags.TryGetValue(articleId, out var tags))
            {
                tags.Remove(tag);
            }
        }

        public HashSet<string> GetAllUniqueTags()
        {
            var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var tags in _articleTags.Values)
            {
                allTags.UnionWith(tags);
            }
            return allTags;
        }

        public HashSet<string> FindCommonTags(string articleId1, string articleId2)
        {
            if (_articleTags.TryGetValue(articleId1, out var tags1) && _articleTags.TryGetValue(articleId2, out var tags2))
            {
                var common = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
                common.IntersectWith(tags2);
                return common;
            }
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class SearchIndex
    {
        private readonly Dictionary<string, HashSet<int>> _invertedIndex = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);

        public void AddDocument(int docId, string content)
        {
            var words = content.Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                var cleanedWord = word.ToLowerInvariant();
                if (!_invertedIndex.ContainsKey(cleanedWord))
                {
                    _invertedIndex[cleanedWord] = new HashSet<int>();
                }
                _invertedIndex[cleanedWord].Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            var cleanedWord = word.ToLowerInvariant();
            return _invertedIndex.TryGetValue(cleanedWord, out var docs) ? new HashSet<int>(docs) : new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words == null || words.Length == 0) return new HashSet<int>();

            HashSet<int>? result = null;

            foreach (var word in words)
            {
                var docs = Search(word);
                if (result == null)
                {
                    result = new HashSet<int>(docs);
                }
                else
                {
                    result.IntersectWith(docs);
                }
            }

            return result ?? new HashSet<int>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Сценарій 1: Кеш метаданих (Dictionary) ===");
            var cache = new FileMetadataCache();
            cache.Add("doc1.pdf", new FileMetadata { FileName = "doc1.pdf", Size = 1024, LastModified = DateTime.Now, Hash = "ABC123" });

            if (cache.Contains("DOC1.PDF"))
            {
                var meta = cache.Get("doc1.pdf");
                Console.WriteLine($"Знайдено файл: {meta?.FileName}, Розмір: {meta?.Size} байт, Хеш: {meta?.Hash}");
            }

            Console.WriteLine("\n=== Сценарій 2: Унікальні теги (HashSet) ===");
            var tagManager = new TagManager();
            tagManager.AddTag("art1", "C#");
            tagManager.AddTag("art1", ".NET");
            tagManager.AddTag("art1", "OOP");

            tagManager.AddTag("art2", "C#");
            tagManager.AddTag("art2", "LINQ");

            Console.WriteLine("Усі унікальні теги: " + string.Join(", ", tagManager.GetAllUniqueTags()));
            Console.WriteLine("Спільні теги для art1 та art2: " + string.Join(", ", tagManager.FindCommonTags("art1", "art2")));

            Console.WriteLine("\n=== Сценарій 3: Індекс для пошуку (Dictionary + HashSet) ===");
            var index = new SearchIndex();
            index.AddDocument(1, "Основи об'єктно-орієнтованого програмування на мові C#");
            index.AddDocument(2, "Вивчення колекцій Dictionary та HashSet у .NET");
            index.AddDocument(3, "Розробка консольних застосувань на C#");

            Console.WriteLine("Документи за словом 'C#': " + string.Join(", ", index.Search("C#")));
            Console.WriteLine("Документи за словами 'C#' та 'програмування': " + string.Join(", ", index.SearchMultiple("C#", "програмування")));
        }
    }
}