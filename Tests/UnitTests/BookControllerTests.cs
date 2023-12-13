using BookApp.Controllers;
using BookApp.Models;
using BookApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Tests.UnitTests
{
    [TestFixture]
    public class BookControllerTests
    {
        private BookController _bookController;
        private Mock<IBookService> _bookService;

        [SetUp]
        public void Setup()
        {
            _bookService = new Mock<IBookService>();
            var logger = new Mock<ILogger<BookController>>();
            _bookController = new BookController(_bookService.Object,logger.Object);
            SetupService();
        }

        [Test]
        public void WhenAccessingIndex_ShouldReturnAllBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Authors="Alfred Igon", Title = "The Splintered Flame", Price = (decimal)100.10, ReleaseDate = new DateTime(2021,10,13)},
                new Book { Id = 2, Authors="Oran Alf", Title = "The Dreamer's Hunter", Price = (decimal)321.9, ReleaseDate = new DateTime(2019,3,27)},
            };
            var result = _bookController.Index() as ViewResult;
            var booksResult = result!.ViewData.Model as IEnumerable<Book>;
            books.Should().BeEquivalentTo(booksResult!.ToList());
        }

        [Test]
        public void WhenAccessingBookDetailsById_ShouldReturnBookDetailsAsViewResult()
        {
            var book = new Book { Id = 1, Authors = "Alfred Igon", Title = "The Splintered Flame", Price = (decimal)100.10, ReleaseDate = new DateTime(2021, 10, 13) };
            var result = _bookController.Details(1) as ViewResult;
            var bookResult = result.ViewData.Model as Book;
            bookResult.Should().BeEquivalentTo(book);
        }

        [Test]
        public void WhenAccessingCreateBookForm_ShouldDisplayEmptyBookAsViewResult()
        {
            var result = _bookController.Create() as ViewResult;
            var bookResult = result!.ViewData.Model as Book;
            bookResult.Should().BeEquivalentTo(new Book(), options =>
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeSameDateAs(ctx.Expectation))
                .WhenTypeIs<DateTime>());
        }

        [Test]
        public void WhenCreatingValidBook_ShouldCreateBookAndRedirectToIndex()
        {
            var result = _bookController.Create(new Book { Id = 3, Authors = "Theodoard Paltiel", Title = "Truth in the Tale", Price = (decimal)50.8, ReleaseDate = new DateTime(2022, 8, 1) }) as RedirectToActionResult;
            result!.ActionName.Should().Be("Index");
            _bookService.Verify(s => s.CreateBook(It.IsAny<Book>()));
            _bookService.Verify(s => s.Save());
        }

        [Test]
        public void WhenAccessingEditBookForm_ShouldDisplayBookDetailsAsViewResult()
        {
            var result = _bookController.Edit(2) as ViewResult;
            var bookResult = result!.ViewData.Model as Book;
            bookResult.Should().BeEquivalentTo(new Book { Id = 2, Authors = "Oran Alf", Title = "The Dreamer's Hunter", Price = (decimal)321.9, ReleaseDate = new DateTime(2019, 3, 27) }, options =>
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeSameDateAs(ctx.Expectation))
                .WhenTypeIs<DateTime>());
        }

        [Test]
        public void WhenEditingValidBook_ShouldEditBookAndRedirectToIndex()
        {
            var result = _bookController.Edit(new Book { Id = 2, Authors = "Theodoard Paltiel", Title = "Truth in the Tale", Price = (decimal)50.8, ReleaseDate = new DateTime(2022, 8, 1) }) as RedirectToActionResult;
            result!.ActionName.Should().Be("Index");
            _bookService.Verify(s => s.UpdateBook(It.IsAny<Book>()));
            _bookService.Verify(s => s.Save());
        }

        [Test]
        public void WhenAccessingDeleteBookPage_ShouldDisplayBookDetailsAsViewResult()
        {
            var result = _bookController.Delete(2, null) as ViewResult;
            var bookResult = result!.ViewData.Model as Book;
            bookResult.Should().BeEquivalentTo(new Book { Id = 2, Authors = "Oran Alf", Title = "The Dreamer's Hunter", Price = (decimal)321.9, ReleaseDate = new DateTime(2019, 3, 27) }, options =>
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeSameDateAs(ctx.Expectation))
                .WhenTypeIs<DateTime>());
        }

        [Test]
        public void WhenDeletingExistingBook_ShouldDeleteBookAndRedirectToIndex()
        {
            var result = _bookController.Delete(1) as RedirectToActionResult;
            result!.ActionName.Should().Be("Index");
            _bookService.Verify(s => s.DeleteBook(It.IsAny<int>()));
            _bookService.Verify(s => s.Save());
        }

        private void SetupService()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Authors="Alfred Igon", Title = "The Splintered Flame", Price = (decimal)100.10, ReleaseDate = new DateTime(2021,10,13)},
                new Book { Id = 2, Authors="Oran Alf", Title = "The Dreamer's Hunter", Price = (decimal)321.9, ReleaseDate = new DateTime(2019,3,27)},
            };

            _bookService.Setup(r => r.GetBooks()).Returns(books);
            _bookService.Setup(r => r.GetBookByID(1)).Returns(books[0]);
            _bookService.Setup(r => r.GetBookByID(2)).Returns(books[1]);
        }
    }
}