
using System.Security.Claims;

namespace Bookify.Web.Controllers
{ //Hello
    [Authorize(Roles = AppRoles.Archive)]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
             
            var authors = _context.Authors.AsNoTracking().ToList();
            //map authors to IEnumerable<AuthorViewModel>>
            var authorsViewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(authors);
            return View(authorsViewModel);
            #region without automapper
            //var authors = _context.Authors
            //   .Select(c => new AuthorViewModel
            //   {
            //       Id = c.Id,
            //       Name = c.Name,
            //       IsDeleted = c.IsDeleted,
            //       CreatedOn = c.CreatedOn,
            //       LastUpdatedOn = c.LastUpdatedOn,
            //   })
            //   .AsNoTracking()
            //   .ToList();
            //return View(authors);
            #endregion
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);
            //convert model(AuthorFormViewModel) to Author
            var author = _mapper.Map<Author>(model);
                author.CreatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            #region mapping without automapper
            //var author = new Author { Name = model.Name };
            #endregion
            _context.Add(author);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Saved Successfully: " + author.Name;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);

            if (author is null)
                return NotFound();
            var viewModel = _mapper.Map<AuthorFormViewModel>(author);
            return View("Form", viewModel);
            #region mapping without automapper
            //var viewModel = new AuthorFormViewModel
            //{
            //    Id = id,
            //    Name = author.Name
            //};
            #endregion

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var author = _context.Authors.Find(model.Id);

            if (author is null)
                return NotFound();

            author = _mapper.Map(model, author); //(specially in Edit) not _mapper.Map<Author>(model); ..error
            #region mapping without automapper
            //author.Name = model.Name;
            #endregion
            author.LastUpdatedOn = DateTime.Now;
            author.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value; ;
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Saved Successfully: " + author.Name;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int Id)
        {

            var author = _context.Authors.Find(Id);
            if (author is null)
                return NotFound();
            author.IsDeleted = !author.IsDeleted;
            author.LastUpdatedOn = DateTime.Now;
            author.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();
            return Ok(author.LastUpdatedOn.ToString());
        }
        public IActionResult AllowAuthor(AuthorFormViewModel model)
        {
            var author = _context.Authors.SingleOrDefault(c => c.Name == model.Name);
            var isAllowed = author is null || author.Id.Equals(model.Id);

            return Json(isAllowed);
        }

    }
}
