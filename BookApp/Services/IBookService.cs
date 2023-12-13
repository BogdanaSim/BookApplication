using BookApp.Models;

namespace BookApp.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetBooks();

        Book GetBookByID(int id);

        void CreateBook(Book book);

        void UpdateBook(Book book);

        void DeleteBook(int id);

        void Save();
    }
}