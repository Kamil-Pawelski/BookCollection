using BookCollection.App.DTO;

namespace BookCollection.Domain.Repositories;

public interface IBookCollectionRepository
{
    List<Book> ReadFile();
    void WriteToFile(List<Book> content);
}
