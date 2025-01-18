namespace BookCollection.Routes;

public struct BookCollectionRoutes
{
    public const string GetBooks = "books";
    public const string GetBook = "books/{id}";
    public const string AddBook = "books";
    public const string UpdateBook = "books";
    public const string DeleteBook = "books/{id}";
    public const string GetBookByTitleOrAuthor = "books/search";
}
