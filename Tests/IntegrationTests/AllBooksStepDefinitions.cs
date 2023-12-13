using BookApp.Controllers;
using BookApp.DAL;
using BookApp.Models;
using BookApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Tests.IntegrationTests
{
    [Binding, Scope(Feature = "AllBooks")]
    public class AllBooksStepDefinitions
    {
        private ActionResult result;
        private BookController _bookController;
        private BookService _bookService;
        private Mock<IBookRepository> _bookRepository;
        private List<Book> _books;

        [BeforeScenario]
        public void Setup()
        {
            _books = new List<Book>();
            _bookRepository = new Mock<IBookRepository>();
            var logger = new Mock<ILogger<BookController>>();
            _bookService = new BookService(_bookRepository.Object);
            _bookController = new BookController(_bookService, logger.Object);
            SetupRepository();
        }

        [Given(@"list of books is empty")]
        public void GivenListOfBooksIsEmpty()
        {
            _bookService.GetBooks().Should().BeEmpty();
        }

        [StepArgumentTransformation(@"([in]*valid)")]
        public bool ValidToBool(string value)
        {
            return value == "valid";
        }

        [Given(@"the user creates a (.*) book with these details")]
        public void GivenTheUserCreatesAValidBookWithTheseDetails(bool IsValid, Table table)
        {
            if (IsValid == false)
            {
                _bookController.ModelState.AddModelError("", "Unable to save.");
            }
            var book = table.CreateInstance<Book>();
            result = _bookController.Create(book);
        }

        [Given(@"the user is redirected to the index page")]
        public void GivenTheUserIsRedirectedToTheIndexPage()
        {
            var bookResult = result as RedirectToActionResult;
            bookResult!.ActionName.Should().Be("Index");
        }

        [Then(@"list of books is empty")]
        public void ThenListOfBooksIsEmpty()
        {
            _bookService.GetBooks().Should().BeEmpty();
        }

        [Given(@"the following book is found in the list")]
        public void GivenTheFollowingBookIsFoundInTheList(Table table)
        {
            var book = table.CreateInstance<Book>();
            _bookService.GetBookByID(book.Id).Should().NotBeNull();
        }

        [When(@"the user deletes the book with id (.*)")]
        public void WhenTheUserDeletesTheBookWithId(int id)
        {
            result = _bookController.Delete(id);
        }

        [Then(@"the user is redirected to the index page")]
        public void ThenTheUserIsRedirectedToTheIndexPage()
        {
            var bookResult = result as RedirectToActionResult;
            bookResult!.ActionName.Should().Be("Index");
        }
        private void SetupRepository()
        {
            _bookRepository.Setup(r => r.GetBooks()).Returns(_books);
            _bookRepository.Setup(r => r.GetBookByID(It.IsAny<int>())).Returns<int>(id => _books.SingleOrDefault(b => b.Id == id));
            _bookRepository.Setup(r => r.CreateBook(It.IsAny<Book>())).Callback<Book>(_books.Add);
            _bookRepository.Setup(r => r.UpdateBook(It.IsAny<Book>())).Callback<Book>(book => _books[_books.FindIndex(b => b.Id == book.Id)] = book);
            //_bookRepository.Setup(r => r.DeleteBook(It.IsAny<int>())).Callback<int>(id => _books = _books.Where(b => b.Id != id).ToList());
            _bookRepository.Setup(r => r.DeleteBook(It.IsAny<int>())).Callback<int>(id => _books.Remove(_books.SingleOrDefault(b => b.Id == id)));
        }
    }
}