using Serilog;
using BookCollection.App.DTO;
using BookCollection.Domain;
using BookCollection.Domain.Repositories;

namespace BookCollection.Infrastructure.Services;

public class BookCollectionService : IBookCollectionService
{
    private readonly IBookCollectionRepository _bookCollectionRepository;
    public static int CurrentBookId = 1;

   
    public BookCollectionService(IBookCollectionRepository bookCollectionRepository)
    {
        _bookCollectionRepository = bookCollectionRepository;
        var booksList = _bookCollectionRepository.ReadFile();
        if (booksList.Any())
        {
            CurrentBookId = booksList.Max(book => book.Id) + 1;
        }
    }


    public OperationResultData<List<Book>> GetBooks()
    {
        try
        {
            var booksList = _bookCollectionRepository.ReadFile();
            return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.Ok, Data = booksList };
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
            var booksList = _bookCollectionRepository.ReadFile();
            var bookToReturn = booksList.FirstOrDefault(book => book.Id == id);

            if (bookToReturn != null)
            {
                return new OperationResultData<Book> { StatusCode = OperationStatusCode.Ok, Data = bookToReturn };
            }
            return new OperationResultData<Book> { StatusCode = OperationStatusCode.NotFound, Message = "The requested book does not exist." };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResultData<Book> { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }

    public OperationResult AddBook(BookDTO newBook)
    {
        try
        {
            var book = new Book
            {
                Id = CurrentBookId++,
                Title = newBook.Title,
                Author = newBook.Author,
                Year = newBook.Year,
            };

            var booksList = _bookCollectionRepository.ReadFile();
            booksList.Add(book);
            _bookCollectionRepository.WriteToFile(booksList);

            return new OperationResult { StatusCode = OperationStatusCode.Ok };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResult { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }

    public OperationResult UpdateBook(int id, BookDTO updatedBook)
    {
        try
        {
            var booksList = _bookCollectionRepository.ReadFile();

            var bookToUpdate = booksList.FirstOrDefault(book => book.Id == id);

            if (bookToUpdate != null)
            {
                bookToUpdate.Title = updatedBook.Title;
                bookToUpdate.Author = updatedBook.Author;
                bookToUpdate.Year = updatedBook.Year;

                _bookCollectionRepository.WriteToFile(booksList);

                return new OperationResult { StatusCode = OperationStatusCode.Ok };
            }
            return new OperationResult { StatusCode = OperationStatusCode.NotFound, Message = "The requested book does not exist." };

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
            var booksList = _bookCollectionRepository.ReadFile();

            var bookToRemove = booksList.FirstOrDefault(book => book.Id == id);
            if (bookToRemove != null)
            {
                booksList.Remove(bookToRemove);
                _bookCollectionRepository.WriteToFile(booksList);

                return new OperationResult { StatusCode = OperationStatusCode.Ok };
            }
            return new OperationResult { StatusCode = OperationStatusCode.NotFound, Message = "The requested book does not exist." };
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
            var booksList = _bookCollectionRepository.ReadFile();
            var sortedBooks = new List<Book>();

            sortedBooks = booksList.Where(book =>
                (string.IsNullOrWhiteSpace(bookSearch.Title) || book.Title.Equals(bookSearch.Title)) &&
                (string.IsNullOrWhiteSpace(bookSearch.Author) || book.Author.Equals(bookSearch.Author))
            ).ToList();

            return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.Ok, Data = sortedBooks };
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new OperationResultData<List<Book>> { StatusCode = OperationStatusCode.InternalError, Message = ex.Message };
        }
    }
}
