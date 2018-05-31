using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthorManager.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly ILogger<AuthorController> _log;
   
        public AuthorController(IAuthorRepository authorRepo,
            ILogger<AuthorController> log)
        {
            _authorRepo = authorRepo;
            _log = log;
        }
        // GET: Author
        public ActionResult Index()
        {
            return View(_authorRepo.ListAll());
        }

        // GET: Author/Details/5
        public ActionResult Details(int id)
        {
            _log.LogInformation("Received request for author ID: {id}", id);
            return View(_authorRepo.GetById(id));
        }

        // GET: Author/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author newAuthor, IFormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View(newAuthor);
            }

            try
            {
                _authorRepo.AddAuthor(newAuthor);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(newAuthor);
            }
        }
    }
}