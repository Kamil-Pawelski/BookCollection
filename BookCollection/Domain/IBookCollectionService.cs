using BookCollection.App.DTO;

namespace BookCollection.Domain;

public interface IBookCollectionService
{
    OperationResultData<List<Book>> GetBooks();
    OperationResultData<Book> GetBook(int id);
    OperationResult AddBook(BookDTO newBook);
    OperationResult UpdateBook(int id, BookDTO updatedBook);
    OperationResult DeleteBook(int id);
    OperationResultData<List<Book>> GetBooksByTitleOrAuthor(BookSearchDTO bookSearch);

}
