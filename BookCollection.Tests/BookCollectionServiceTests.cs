using BookCollection.App.DTO;
using BookCollection.Configuration;
using BookCollection.Domain;
using BookCollection.Domain.Repositories;
using BookCollection.Infrastructure.Repositories;
using BookCollection.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace BookCollection.Tests;

public class BookCollectionServiceTests : IDisposable
{
    private readonly IBookCollectionService _bookCollectionService;
    private readonly IBookCollectionRepository _bookCollectionRepository;
    private readonly IConfiguration _configuration;

    public BookCollectionServiceTests()
    {
        _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Staging.json")
                .Build();
        AppConfiguration.Initialize(_configuration);

        _bookCollectionRepository = new BookCollectionRepository();
        _bookCollectionService = new BookCollectionService(_bookCollectionRepository);
    }


    [Fact]
    public void AddAndGetBook_Ok()
    {
        var bookDTO = new BookDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(bookDTO);

        var result = _bookCollectionService.GetBooks();
        var book = result.Data.FirstOrDefault();
        Assert.NotNull(result.Data);
        Assert.Equal(bookDTO.Author, book.Author);
        Assert.Equal(bookDTO.Title, book.Title);
        Assert.Equal(bookDTO.Year, book.Year);
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);

    }

    [Fact]
    public void GetBook_NotFound()
    {
        var book = new BookDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var result = _bookCollectionService.GetBook(200);
        Assert.Null(result.Data);
        Assert.Equal(OperationStatusCode.NotFound, result.StatusCode);
        Assert.Equal("The requested book does not exist.", result.Message);
    }

    [Fact]
    public void GetBooks_Ok()
    {
        var booksToAdd = new List<BookDTO>(){
            new()
            {
                Author = "Jacob Wal",
                Title = "Some random book",
                Year = 2013
            },
            new()
            {
                Author = "Adam Szym",
                Title = "Next das",
                Year = 2010
            },
            new()
            {
                Author = "Kim Min",
                Title = "Han guk",
                Year = 2019
            },
        };

        foreach (var book in booksToAdd)
        {
            _bookCollectionService.AddBook(book);
        }

        var result = _bookCollectionService.GetBooks();
        Assert.NotNull(result.Data);
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void GetBooks_Empty_Ok()
    {
        var result = _bookCollectionService.GetBooks();
        Assert.Empty(result.Data);
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void DeleteBook_Ok()
    {
        var book = new BookDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var newBook = (_bookCollectionService.GetBooks().Data).FirstOrDefault();

        var result = _bookCollectionService.DeleteBook(newBook.Id);

        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void DeleteBook_NotFound()
    {
        var book = new BookDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);
        var result = _bookCollectionService.DeleteBook(120);

        Assert.Equal(OperationStatusCode.NotFound, result.StatusCode);
        Assert.Equal("The requested book does not exist.", result.Message);
    }

    [Fact]
    public void UpdateBook_Ok()
    {
        var book = new BookDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var newBook = (_bookCollectionService.GetBooks().Data).FirstOrDefault();

        var updatedBook = new BookDTO
        {
            Author = "Pavel Atr",
            Title = "New Book",
            Year = 2013
        };

        var result = _bookCollectionService.UpdateBook(newBook.Id, updatedBook);

        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void UpdateBook_NotFound()
    {
        var book = new BookDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var updatedBook = new BookDTO
        {
            Author = "Pavel Atr",
            Title = "New Book",
            Year = 2013
        };
        var result = _bookCollectionService.UpdateBook(150, updatedBook);


        Assert.Equal(OperationStatusCode.NotFound, result.StatusCode);
    }

    [Theory]
    [InlineData("Some random book", "", true)]
    [InlineData("", "Jacob Wal", true)]
    [InlineData("Some random book", "Jacob Wal", true)]
    [InlineData("Next das", "Jacob Wal", false)]
    [InlineData("", "", true)]
    public void GetBooksByTitleOrAuthor_AuthorAndTitle_Ok(string title, string author, bool boolResult)
    {
        var booksToAdd = new List<BookDTO>(){
        new()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        },
        new()
        {
            Author = "Adam Szym",
            Title = "Next das",
            Year = 2010
        },
        new()
        {
            Author = "Kim Min",
            Title = "Han guk",
            Year = 2019
        },
    };

        foreach (var book in booksToAdd)
        {
            _bookCollectionService.AddBook(book);
        }

        var searchDTO = new BookSearchDTO()
        {
            Title = title,
            Author = author
        };

        var result = _bookCollectionService.GetBooksByTitleOrAuthor(searchDTO);

        if (boolResult)
        {
            Assert.NotEmpty(result.Data);
        }
        else
        {
            Assert.Empty(result.Data);
        }
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }
    public void Dispose()
    {
        if (File.Exists(AppConfiguration.BookCollectionFile))
        {
            File.Delete(AppConfiguration.BookCollectionFile);
        }
    }
}