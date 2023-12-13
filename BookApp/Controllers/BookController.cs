using BookApp.Models;
using BookApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace BookApp.Controllers
{
    public class BookController : Controller
    {
        private IBookService _bookService;
        private readonly ILogger _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            this._bookService = bookService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book", "GET");
            var books = from book in _bookService.GetBooks()
                        select book;
            return View(books);
        }

        public ViewResult Details(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book/Details/" + id, "GET");
            var book = _bookService.GetBookByID(id);
            return View(book);
        }

        public ActionResult Create()
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book/Create/", "GET");
            ViewBag.Title = "Create";
            return View("Create", new Book());
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book/Create/", "POST");
            try
            {
                if (ModelState.IsValid)
                {
                    _bookService.CreateBook(book);
                    _bookService.Save();
                    _logger.LogTrace("Created new book with Id {id}", book.Id);
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save.");
                _logger.LogError("Error while creating new book : {error}", ex.InnerException);
            }

            return View(book);
        }

        public ActionResult Edit(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book/Edit/" + id, "GET");
            var book = _bookService.GetBookByID(id);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book/Edit/", "POST");
            try
            {
                if (ModelState.IsValid)
                {
                    _bookService.UpdateBook(book);
                    _bookService.Save();
                    _logger.LogTrace("Edited book with Id {id}", book.Id);
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save.");
                _logger.LogError("Error while editing book with Id {id} : {error}", book.Id, ex.InnerException);
            }

            return View(book);
        }

        public ActionResult Delete(int id, bool? saveError)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book/Delete/" + id, "GET");
            if (saveError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save.";
                //_logger.LogError("Error while deleting book with Id {id}", id);
            }
            var book = _bookService.GetBookByID(id);
            return View(book);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/Book/Delete/", "POST");
            try
            {
                var book = _bookService.GetBookByID(id);
                _bookService.DeleteBook(id);
                _bookService.Save();
                _logger.LogTrace("Deleted book with Id {id}", id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Error while deleting book with Id {id} : {error}", id, ex.InnerException);

                return RedirectToAction("Delete",
                    new RouteValueDictionary
                    {
                        {"id",id },
                        {"saveError", true }
                    });
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}