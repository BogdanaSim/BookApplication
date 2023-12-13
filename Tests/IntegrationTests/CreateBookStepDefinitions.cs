using BookApp.Controllers;
using BookApp.Models;
using BookApp.Services;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Tests.IntegrationTests
{
    [Binding, Scope(Feature = "CreateBook")]
    public class CreateBookStepDefinitions
    {
        private ActionResult result;
        private BookController _bookController;
        private Mock<IBookService> _bookService;

        [BeforeScenario]
        public void Setup()
        {
            _bookService = new Mock<IBookService>();
            var logger = new Mock<ILogger<BookController>>();
            _bookController = new BookController(_bookService.Object, logger.Object);
        }

        [When(@"the user navigates to the create book page")]
        [Given(@"the user is on the create book page")]
        public void WhenTheUserNavigatesToTheCreateBookPage()
        {
            result = _bookController.Create();
        }

        [Then(@"the Create Book view should be displayed")]
        public void ThenTheCreateBookViewShouldBeDisplayed()
        {
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewName.Should().Be("Create");
            viewResult.ViewData["Title"].Should().Be("Create");
        }

        [StepArgumentTransformation(@"([in]*valid)")]
        public bool ValidToBool(string value)
        {
            return value == "valid";
        }

        [When(@"the user creates a (.*) book with these details")]
        public void WhenTheUserCreatesABookWithTheseDetails(bool IsValid, TechTalk.SpecFlow.Table table)
        {
            if (IsValid == false)
            {
                _bookController.ModelState.AddModelError("", "Unable to save.");
            }
            var book = table.CreateInstance<Book>();
            result = _bookController.Create(book);
        }

        [Then(@"the user is redirected to the index page")]
        public void ThenTheUserIsRedirectedToTheIndexPage()
        {
            var bookResult = result as RedirectToActionResult;
            bookResult!.ActionName.Should().Be("Index");
        }

        [Then(@"the book is added to the list")]
        public void ThenTheBookIsAddedToTheList()
        {
            //var expectedBook = table.CreateInstance<Book>();
            //_bookService.Verify(s => s.CreateBook(It.Is<Book>(
            //    b =>
            //        b.Id == expectedBook.Id
            //        && b.Authors == expectedBook.Authors
            //        && b.Price == expectedBook.Price
            //        && b.ReleaseDate == expectedBook.ReleaseDate)));
            _bookService.Verify(s => s.CreateBook(It.IsAny<Book>()));
            _bookService.Verify(s => s.Save());
        }

        [Then(@"the validation errors are displayed")]
        public void ThenTheValidationErrorsAreDisplayed()
        {
            var bookResult = result as ViewResult;
            bookResult!.ViewData.ModelState.IsValid.Should().BeFalse();
            _bookController.ModelState.IsValid!.Should().BeFalse();
        }

    }
}