using System;
using System.Collections.Generic;

public enum LiteratureType
{
    Fiction,       // Художественная
    Methodical,    // Методическая
    Reference,     // Справочная
    Scientific,    // Научная
    Other          // Прочее
}

public class Book : IDisposable
{
    private string _title;
    private string _author;
    private int _publicationYear;
    private LiteratureType _type;
    private bool _disposed = false;

    // Конструктор
    public Book(string title, string author, int publicationYear, LiteratureType type)
    {
        SetTitle(title);
        SetAuthor(author);
        SetPublicationYear(publicationYear);
        SetType(type);
    }

    // Деструктор (финализатор)
    ~Book()
    {
        Dispose(false);
    }

    // Реализация IDisposable
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Освобождение управляемых ресурсов
            }
            // Освобождение неуправляемых ресурсов
            _disposed = true;
        }
    }

    // Методы установки значений с проверкой корректности
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название книги не может быть пустым.");
        _title = title.Trim();
    }

    public void SetAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Имя автора не может быть пустым.");
        _author = author.Trim();
    }

    public void SetPublicationYear(int year)
    {
        int currentYear = DateTime.Now.Year;
        if (year < 0 || year > currentYear)
            throw new ArgumentException($"Год публикации должен быть между 0 и {currentYear}.");
        _publicationYear = year;
    }

    public void SetType(LiteratureType type)
    {
        _type = type;
    }

    // Методы получения значений
    public string GetTitle() => _title;
    public string GetAuthor() => _author;
    public int GetPublicationYear() => _publicationYear;
    public new LiteratureType GetType() => _type;

    // Вычисление возраста книги
    public int GetBookAge()
    {
        return DateTime.Now.Year - _publicationYear;
    }

    // Метод печати
    public void Print()
    {
        Console.WriteLine("Информация о книге:");
        Console.WriteLine($"Название: {_title}");
        Console.WriteLine($"Автор: {_author}");
        Console.WriteLine($"Год выпуска: {_publicationYear}");
        Console.WriteLine($"Возраст книги: {GetBookAge()} лет");
        Console.WriteLine($"Тип литературы: {_type}");
    }
}

public class Library
{
    private List<Book> _books;

    public Library()
    {
        _books = new List<Book>();
    }

    // Добавление книги
    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book), "Книга не может быть null.");
        _books.Add(book);
        Console.WriteLine($"Книга '{book.GetTitle()}' успешно добавлена.");
    }

    // Просмотр всех книг
    public void DisplayAllBooks()
    {
        if (_books.Count == 0)
        {
            Console.WriteLine("Библиотека пуста.");
            return;
        }

        Console.WriteLine("\nСписок всех книг в библиотеке:");
        for (int i = 0; i < _books.Count; i++)
        {
            Console.WriteLine($"\nКнига #{i + 1}:");
            _books[i].Print();
        }
    }

    // Очистка ресурсов
    public void Dispose()
    {
        foreach (var book in _books)
        {
            book.Dispose();
        }
        _books.Clear();
    }
}

// Программа с меню
public class Program
{
    public static void Main()
    {
        Library library = new Library();

        try
        {
            while (true)
            {
                Console.WriteLine("\nМеню библиотеки:");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Просмотреть все книги");
                Console.WriteLine("3. Выйти");
                Console.Write("Выберите действие (1-3): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewBook(library);
                        break;
                    case "2":
                        library.DisplayAllBooks();
                        break;
                    case "3":
                        Console.WriteLine("Программа завершена.");
                        return;
                    default:
                        Console.WriteLine("Ошибка: Неверный выбор. Пожалуйста, выберите 1, 2 или 3.");
                        break;
                }
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            library.Dispose();
        }
    }

    private static void AddNewBook(Library library)
    {
        try
        {
            Console.WriteLine("\nДобавление новой книги:");
            Console.Write("Введите название: ");
            string title = Console.ReadLine();

            Console.Write("Введите автора: ");
            string author = Console.ReadLine();

            Console.Write("Введите год выпуска: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Ошибка: Неверный формат года.");
                return;
            }

            Console.WriteLine("Выберите тип литературы:");
            Console.WriteLine("0 - Fiction (Художественная)");
            Console.WriteLine("1 - Methodical (Методическая)");
            Console.WriteLine("2 - Reference (Справочная)");
            Console.WriteLine("3 - Scientific (Научная)");
            Console.WriteLine("4 - Other (Прочее)");
            Console.Write("Введите номер (0-4): ");
            if (!int.TryParse(Console.ReadLine(), out int typeIndex) || !Enum.IsDefined(typeof(LiteratureType), typeIndex))
            {
                Console.WriteLine("Ошибка: Неверный тип литературы.");
                return;
            }

            Book newBook = new Book(title, author, year, (LiteratureType)typeIndex);
            library.AddBook(newBook);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}