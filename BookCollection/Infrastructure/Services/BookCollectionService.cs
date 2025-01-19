using Serilog;
using System.Text.Json;
using BookCollection.Domain.Models;
using BookCollection.App.DTO;
using BookCollection.Domain;
using BookCollection.Configuration;

namespace BookCollection.Infrastructure.Services;

public interface IBookCollectionService
{
    OperationResultData<List<Book>> GetBooks();
    OperationResultData<Book> GetBook(int id);
    OperationResult AddBook(BookAddDTO newBook);
    OperationResult UpdateBook(BookDTO updatedBook);
    OperationResult DeleteBook(int id);
    OperationResultData<List<Book>> GetBooksByTitleOrAuthor(BookSearchDTO bookSearch);

}

public class BookCollectionService : IBookCollectionService
{
    //_dbcontext for now json
    public static int CurrentBookId = 1; // Just for this example with DB Guid.NewGuid()

    public OperationResultData<List<Book>> GetBooks()
    {
        try
        {
            if (File.Exists(AppConfigurationConstants.BookCollectionFile))
            {
                var fileText = File.ReadAllText(AppConfigurationConstants.BookCollectionFile);
                var booksList = JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];

                return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.Ok, Data = booksList };
            }
            return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.Ok, Data = [] };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }

    public OperationResultData<Book> GetBook(int id)
    {
        try
        {
            if (File.Exists(AppConfigurationConstants.BookCollectionFile))
            {
                var fileText = File.ReadAllText(AppConfigurationConstants.BookCollectionFile);
                var booksList = JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];
                var bookToReturn = booksList.FirstOrDefault(book => book.Id == id);

                return new OperationResultData<Book> { StatusCode = OperationStatusCode.Ok, Data = bookToReturn };
            }
            return new OperationResultData<Book> { StatusCode = OperationStatusCode.InternalError, Message = "A general error occurred while processing the request" };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResultData<Book> { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }

    public OperationResult AddBook(BookAddDTO newBook)
    {
        try
        {
            List<Book> booksList;

            var book = new Book
            {
                Id = CurrentBookId++,
                Title = newBook.Title,
                Author = newBook.Author,
                Year = newBook.Year,
            };

            if (File.Exists(AppConfigurationConstants.BookCollectionFile))
            {
                var fileText = File.ReadAllText(AppConfigurationConstants.BookCollectionFile);
                booksList = JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];
            }
            else
            {
                booksList = [];
            }

            booksList.Add(book);

            var updatedBooksJson = JsonSerializer.Serialize(booksList);

            File.WriteAllText(AppConfigurationConstants.BookCollectionFile, updatedBooksJson);
            return new OperationResult { StatusCode = OperationStatusCode.Ok };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResult { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }

    public OperationResult UpdateBook(BookDTO updatedBook)
    {
        try
        {
            if (File.Exists(AppConfigurationConstants.BookCollectionFile))
            {
                var fileText = File.ReadAllText(AppConfigurationConstants.BookCollectionFile);
                var booksList = JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];

                var bookToUpdate = booksList.FirstOrDefault(book => book.Id == updatedBook.Id);
                if (bookToUpdate != null)
                {
                    bookToUpdate.Title = updatedBook.Title;
                    bookToUpdate.Author = updatedBook.Author;
                    bookToUpdate.Year = updatedBook.Year;
                    var updatedBooksJson = JsonSerializer.Serialize(booksList);

                    File.WriteAllText(AppConfigurationConstants.BookCollectionFile, updatedBooksJson);
                    return new OperationResult { StatusCode = OperationStatusCode.Ok };
                }
                return new OperationResult { StatusCode = OperationStatusCode.NotFound, Message = "The requested book does not exist" };
            }
            return new OperationResult { StatusCode = OperationStatusCode.InternalError, Message = "No data exist" };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating the book.");
            return new OperationResult { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }


    public OperationResult DeleteBook(int id)
    {
        try
        {
            if (File.Exists(AppConfigurationConstants.BookCollectionFile))
            {
                var fileText = File.ReadAllText(AppConfigurationConstants.BookCollectionFile);
                var booksList = JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];

                var bookToRemove = booksList.FirstOrDefault(book => book.Id == id);
                if (bookToRemove != null)
                {
                    booksList.Remove(bookToRemove);

                    var updatedBooksJson = JsonSerializer.Serialize(booksList);

                    File.WriteAllText(AppConfigurationConstants.BookCollectionFile, updatedBooksJson);
                    return new OperationResult { StatusCode = OperationStatusCode.Ok };
                }
                return new OperationResult { StatusCode = OperationStatusCode.NotFound, Message = "The requested book does not exist" };
            }
            return new OperationResult { StatusCode = OperationStatusCode.InternalError, Message = "No data exist" };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResult { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }

    public OperationResultData<List<Book>> GetBooksByTitleOrAuthor(BookSearchDTO bookSearch)
    {
        try
        {
            if (File.Exists(AppConfigurationConstants.BookCollectionFile))
            {
                var fileText = File.ReadAllText(AppConfigurationConstants.BookCollectionFile);
                var booksList = JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];
                var sortedBooks = booksList.Where(book => book.Title.Equals(bookSearch.Title) || book.Author.Equals(bookSearch.Author)).ToList();
                return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.Ok, Data = sortedBooks };
            }
            return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.Ok, Data = [] };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }
}
