using BookApp.Models;

namespace BookApp.DAL
{
    public interface IBookRepository : IDisposable
    {
        IEnumerable<Book> GetBooks();

        Book GetBookByID(int id);

        void CreateBook(Book book);

        void UpdateBook(Book book);

        void DeleteBook(int id);

        void Save();
    }
}