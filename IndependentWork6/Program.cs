using System;
using System.Collections.Generic;

namespace IndependentWork6
{
    public interface INotifier
    {
        void Send(string message);
    }

    public class EmailNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"[Email] Надіслано листом: {message}");
        }
    }

    public class SmsNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"[SMS] Надіслано через SMS: {message}");
        }
    }

    public class PushNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"[Push] Push-сповіщення: {message}");
        }
    }

    public abstract class FileProcessor
    {
        public void Process(string filePath)
        {
            OpenFile(filePath);
            ProcessContent(filePath);
            CloseFile(filePath);
        }

        protected void OpenFile(string filePath)
        {
            Console.WriteLine($"[Файл] Відкриття файлу: {filePath}");
        }

        protected abstract void ProcessContent(string filePath);

        protected void CloseFile(string filePath)
        {
            Console.WriteLine($"[Файл] Закриття файлу: {filePath}");
        }
    }

    public class TextFileProcessor : FileProcessor
    {
        protected override void ProcessContent(string filePath)
        {
            Console.WriteLine($"[Text] Зчитування та обробка текстових рядків із {filePath}");
        }
    }

    public class XmlFileProcessor : FileProcessor
    {
        protected override void ProcessContent(string filePath)
        {
            Console.WriteLine($"[XML] Парсинг XML-вузлів та атрибутів із {filePath}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Сценарій 1: Система сповіщень (Інтерфейс INotifier) ===");
            List<INotifier> notifiers = new List<INotifier>
            {
                new EmailNotifier(),
                new SmsNotifier(),
                new PushNotifier()
            };

            foreach (INotifier notifier in notifiers)
            {
                notifier.Send("Ваш баланс поповнено!");
            }

            Console.WriteLine("\n=== Сценарій 2: Обробка файлів (Абстрактний клас FileProcessor) ===");
            List<FileProcessor> processors = new List<FileProcessor>
            {
                new TextFileProcessor(),
                new XmlFileProcessor()
            };

            foreach (FileProcessor processor in processors)
            {
                processor.Process("document.dat");
            }
        }
    }
}