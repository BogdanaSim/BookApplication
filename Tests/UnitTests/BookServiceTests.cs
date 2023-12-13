using BookApp.DAL;
using BookApp.Models;
using BookApp.Services;

namespace Tests.UnitTests
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IBookRepository> _bookRepository;
        private BookService _bookService;

        [SetUp]
        public void Setup()
        {
            _bookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_bookRepository.Object);
            SetupRepository();
        }

        [Test]
        public void ShouldGetBookById()
        {
            var book = _bookService.GetBookByID(1);
            book.Should().BeEquivalentTo(new Book { Id = 1, Authors = "Alfred Igon", Title = "The Splintered Flame", Price = (decimal)100.10, ReleaseDate = new DateTime(2021, 10, 13) });
        }

        [Test]
        public void ShouldGetBookAllBooks()
        {
            var result = _bookService.GetBooks();
            var books = new List<Book>
            {
                new Book { Id = 1, Authors="Alfred Igon", Title = "The Splintered Flame", Price = (decimal)100.10, ReleaseDate = new DateTime(2021,10,13)},
                new Book { Id = 2, Authors="Oran Alf", Title = "The Dreamer's Hunter", Price = (decimal)321.9, ReleaseDate = new DateTime(2019,3,27)},
            };
            result.Should().BeEquivalentTo(books);
        }

        [Test]
        public void ShouldCreateBook()
        {
            _bookService.CreateBook(new Book { Id = 3, Authors = "Theodoard Paltiel", Title = "Truth in the Tale", Price = (decimal)50.8, ReleaseDate = new DateTime(2022, 8, 1) });
            _bookRepository.Verify(r => r.CreateBook(It.IsAny<Book>()));
        }

        [Test]
        public void ShouldUpdateBook()
        {
            var book = new Book { Id = 2, Authors = "Theodoard Paltiel", Title = "Truth in the Tale", Price = (decimal)50.8, ReleaseDate = new DateTime(2022, 8, 1) };
            _bookService.UpdateBook(book);
            _bookRepository.Verify(r => r.UpdateBook(It.IsAny<Book>()));
        }

        [Test]
        public void ShouldDeleteBook()
        {
            _bookService.DeleteBook(1);
            _bookRepository.Verify(r => r.DeleteBook(It.IsAny<int>()));
        }

        [Test]
        public void ShouldSaveChanges()
        {
            _bookService.Save();
            _bookRepository.Verify(r => r.Save());
        }

        private void SetupRepository()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Authors="Alfred Igon", Title = "The Splintered Flame", Price = (decimal)100.10, ReleaseDate = new DateTime(2021,10,13)},
                new Book { Id = 2, Authors="Oran Alf", Title = "The Dreamer's Hunter", Price = (decimal)321.9, ReleaseDate = new DateTime(2019,3,27)},
            };

            _bookRepository.Setup(r => r.GetBooks()).Returns(books);
            _bookRepository.Setup(r => r.GetBookByID(1)).Returns(books[0]);
            _bookRepository.Setup(r => r.GetBookByID(2)).Returns(books[1]);
        }
    }
}