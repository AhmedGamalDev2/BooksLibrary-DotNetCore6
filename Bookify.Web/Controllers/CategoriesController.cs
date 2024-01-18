
using Bookify.Web.Filters;

namespace Bookify.Web.Controllers
{ //Hello
    [Authorize(Roles = AppRoles.Archive)]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //TODO: use viewModel
            var categories = _context.Categories.AsNoTracking() .ToList();
            //map categories to IEnumerable<CategoryViewModel>>
            var categoriesViewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return View(categoriesViewModel);
            #region without automapper
            //var categories = _context.Categories
            //   .Select(c => new CategoryViewModel
            //   {
            //       Id = c.Id,
            //       Name = c.Name,
            //       IsDeleted = c.IsDeleted,
            //       CreatedOn = c.CreatedOn,
            //       LastUpdatedOn = c.LastUpdatedOn,
            //   })
            //   .AsNoTracking()
            //   .ToList();
            //return View(categories);
            #endregion
        }

        [HttpGet]
        
        public IActionResult Create()
        {
            return View("Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);
            //convert model(CategoryFormViewModel) to Category
            var category = _mapper.Map<Category>(model);
            category.CreatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            #region mapping without automapper
            //var category = new Category { Name = model.Name };
            #endregion
            _context.Add(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Saved Successfully: " + category.Name;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
       
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is null)
                return NotFound();
            var viewModel = _mapper.Map<CategoryFormViewModel>(category);
            return View("Form", viewModel);
            #region mapping without automapper
            //var viewModel = new CategoryFormViewModel
            //{
            //    Id = id,
            //    Name = category.Name
            //};
            #endregion

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var category = _context.Categories.Find(model.Id);

            if (category is null)
                return NotFound();
            
            category = _mapper.Map(model,category); //(specially in Edit) not _mapper.Map<Category>(model); ..error
            category.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            #region mapping without automapper
            //category.Name = model.Name;
            #endregion
            category.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Saved Successfully: " + category.Name;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int Id)
        {
             
            var category = _context.Categories.Find(Id);
            if(category is null) 
                return NotFound();
            category.IsDeleted  = !category.IsDeleted;
            category.LastUpdatedOn  = DateTime.Now;
            category.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();
            return Ok(category.LastUpdatedOn.ToString());//Ok(category);
        }
        public IActionResult AllowCategory(CategoryFormViewModel model)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Name == model.Name);
            var isAllowed = category is null || category.Id.Equals(model.Id);

            return Json(isAllowed);
        }

    }
}
