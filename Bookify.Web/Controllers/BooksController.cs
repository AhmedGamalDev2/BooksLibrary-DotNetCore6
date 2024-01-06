using Bookify.Web.Core.Consts;
using Bookify.Web.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace Bookify.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".webp" };
        private int _maxAllowedSize = 2097152;

        public BooksController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IOptions<CloudinarySettings> cloudinary)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            var account = new Account // account on cloudinary
            { //map data from cloudinarysetting to cloudinary account 
                Cloud = cloudinary.Value.Cloud,
                ApiKey = cloudinary.Value.ApiKey,
                ApiSecret = cloudinary.Value.ApiSecret

            };
            _cloudinary = new Cloudinary(account);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetBooks()
        {
            var skip = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);

            var searchValue = Request.Form["search[value]"];

            var sortColumnIndex = Request.Form["order[0][column]"];
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            IQueryable<Book> books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .ThenInclude(c => c.Category);

            if (!string.IsNullOrEmpty(searchValue))
                books = books.Where(b => b.Title.Contains(searchValue) || b.Author!.Name.Contains(searchValue));

            books = books.OrderBy($"{sortColumn} {sortColumnDirection}");

            var data = books.Skip(skip).Take(pageSize).ToList();

            var mappedData = _mapper.Map<IEnumerable<BookViewModel>>(data);

            var recordsTotal = books.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = mappedData };

            return Ok(jsonData);
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(book => book.Author)
                  .Include(b => b.Copies)
                .Include(b=>b.Categories)
                .ThenInclude(s=>s.Category) // this Category that is inside BookCategory (ThenInclude is very usefull)
                .SingleOrDefault(b => b.Id == id );
           
            if(book is null)
            {
                return NotFound();
            }
            //mapping
            var viewModel = _mapper.Map<BookViewModel>(book);//source:book,destination:BookViewModel
            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = PopulateViewModel(); // initialize authors and categories
            return View("Form", viewModel);
            #region Instead of PopulateViewModel()
            //var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            //var categories = _context.Categories.Where(c => !c.IsDeleted).OrderBy(c => c.Name).ToList();
            //var viewModel = new BookFormViewModel
            //{
            //    Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors),
            //    Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories),
            //};
            #endregion
            #region mapping without automapper
            //var authors = _context.Authors.Where(a => !a.IsDeleted)
            //    .Select(a => new SelectListItem{ Value = a.Id.ToString(),Text = a.Name,})
            //    .OrderBy(c => c.Text).ToList();
            //var categories = _context.Categories.Where(c => !c.IsDeleted)
            //    .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name, })
            //   .OrderBy(c => c.Text).ToList();
            //var viewModel = new BookFormViewModel
            //{
            //    Authors = authors,
            //    Categories =  categories
            //};
            // you can make it viewData[] = new SelectList()
            #endregion
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var returnModel = PopulateViewModel(model);
                return View("Form", returnModel);
            }
            var book = _mapper.Map<Book>(model); // Image cannot map with automapper
            //deal with image
            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtenstions);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }
                //save image in database and in application
                var imageName = $"{Guid.NewGuid()}{extension}"; // change name of image in database and in wwwroot

                #region Save image in local server, this deals with out resource (Operating System)
                    #region Save original image
                    var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", imageName); //this save image in wwwroot/images/books ,, (_webHostEnvironment.WebRootPath) => get path until wwwroot
                    using var stream = System.IO.File.Create(path); // save that this Iformfile(image) in that path
                    await model.Image.CopyToAsync(stream); // copy model.Image in stream that its role save it in path
                    stream.Dispose();//stop this stream to i can start another stream
                    //book.ImageUrl =imageName;//this save image Name in database(ImageUrl)
                    book.ImageUrl = $"/images/books/{imageName}";//this save imageUrl in database(ImageUrl)
                    #endregion
                    #region Save thumbnail image 
                    //how to create and generate thumbnail with backage SixLabors.ImageSharp.Web
                    using var image = Image.Load(model.Image.OpenReadStream());
                    var ratio = (float)image.Width / 200;
                    var height = image.Height / ratio;
                    image.Mutate(i => i.Resize(width: 200, height: (int)height)); // to resize the image
                    var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books/thumb", imageName); //this save image in wwwroot/images/books ,, (_webHostEnvironment.WebRootPath) => get path until wwwroot
                    image.Save(thumbPath); //to save thumbnail in local server
                    book.ImageThumbnailUrl = $"/images/books/thumb/{imageName}";//this save imageThumbnailUrl in database(ThumbnailUrl)

                    #endregion
                #endregion
                #region Save image in Cloudninary server instead of local server
                //using var stream = model.Image.OpenReadStream();// create stream for image
                //var imageParams = new ImageUploadParams
                //{
                //    File = new FileDescription(imageName, stream),
                //    UseFilename = true // make file name on cloudinary is the same name uploaded file(imageName )
                //};
                //var result = await _cloudinary.UploadAsync(imageParams);
                //book.ImageUrl = result.SecureUrl.ToString();
                //book.ImageThumbnailUrl = GetThumbnailUrl(book.ImageUrl);//thumbnail image with small size
                //book.ImagePublicId = result.PublicId;
                #endregion
            }

            foreach (var item in model.SelectedCategoryIds)
            { //fill BookCategory table
                book.Categories.Add(new BookCategory { CategoryId = item });
            }

            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details),new {id= book.Id});// here(id field) must equal and be the same Details parameter(int id)
        }

        public IActionResult Edit(int id)
        {
            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }
            var model = _mapper.Map<BookFormViewModel>(book);
            var viewModel = PopulateViewModel(model);
            viewModel.SelectedCategoryIds = book.Categories.Select(c => c.CategoryId).ToList();
            return View("Form", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormViewModel model)
        {
            /*
             * Note, don't delete that
             * image on server means => image in path wwwroot
             * image on database means=> image in book.ImageUrl(Sql)
             */
            //string imagePublicId = null; //Cloudinary
            if (!ModelState.IsValid)
            {
                var returnModel = PopulateViewModel(model);
                return View("Form", returnModel);
            }
            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == model.Id);
            if (book is null)
            {
                return NotFound();
            }

            //deal with image
            //in case model.Image not null and (there is old image or there is not old image)
            if (model.Image is not null)
            {
                // in case there is old image
                if (!string.IsNullOrEmpty(book.ImageUrl)) //check if ImageUrl(in database) has value
                {
                    #region deal with local server
                    //var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", book.ImageUrl); //on the server, this before thumbnail image
                        #region Deal with original image
                        var oldImagePath = $"{_webHostEnvironment.WebRootPath}{book.ImageUrl}"; //on the server
                        if (System.IO.File.Exists(oldImagePath))// is there file in that path ,if yes delete it 
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        #endregion
                        #region Deal with thumbnail image
                        var oldThumbPath = $"{_webHostEnvironment.WebRootPath}{book.ImageThumbnailUrl}"; //on the server
                        if (System.IO.File.Exists(oldThumbPath))//delete thumbnail image// is there file in that path ,if yes delete it 
                        {
                            System.IO.File.Delete(oldThumbPath);
                        }
                        #endregion
                    #endregion
                    #region Deal with cloudinary
                    // Delete the old image from Cloudinary
                    //await _cloudinary.DeleteResourcesAsync(book.ImagePublicId);
                    #endregion
                }
                // in case model.Image  is new image(there is not old image)
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtenstions);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }
                //save image in database and in application(server)
                var imageName = $"{Guid.NewGuid()}{extension}"; // change name of image in database and in wwwroot(on server)(Guid name)

                #region Save image in local server, this deals with out resource (Operating System)
                    #region Save original image
                    var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", imageName); //this save image in wwwroot/images/books ,, (_webHostEnvironment.WebRootPath) => get path until wwwroot
                    using var stream = System.IO.File.Create(path); // save that this Iformfile(image) in that path or create file in this path
                    await model.Image.CopyToAsync(stream); // created file in that path put this data(model.Image) in it
                    //model.ImageUrl = imageName;//this map in automapper && this save image in database(old)
                    model.ImageUrl = $"/images/books/{imageName}";//this map in automapper && this save imageUrl in database

                    #endregion
                    #region Save thumbnail image
                    //how to create and generate thumbnail with backage imagesharp
                    using var image = Image.Load(model.Image.OpenReadStream());
                    var ratio = (float)image.Width / 200;
                    var height = image.Height / ratio;
                    image.Mutate(i => i.Resize(width: 200, height: (int)height)); // to resize the image
                    var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books/thumb", imageName); //this save image in wwwroot/images/books ,, (_webHostEnvironment.WebRootPath) => get path until wwwroot
                    image.Save(thumbPath); //to save thumbnail in local server
                    model.ImageThumbnailUrl = $"/images/books/thumb/{imageName}";//this save imageThumbnailUrl in database(ThumbnailUrl)

                    #endregion
                #endregion
                #region Save image in Cloudninary server instead of local server
                //// Save image in Cloudinary server
                //using var stream = model.Image.OpenReadStream();// create stream for image
                //var imageParams = new ImageUploadParams
                //{
                //    File = new FileDescription(imageName, stream),
                //    UseFilename = true // make file name on cloudinary is the same name uploaded file(imageName )
                //};
                //var result = await _cloudinary.UploadAsync(imageParams);
                //book.ImageUrl = result.SecureUrl.ToString();
                //book.ImageThumbnailUrl = GetThumbnailUrl(book.ImageUrl);//thumbnail image with small size
                //model.ImageUrl = result.SecureUrl.ToString();
                //imagePublicId = result.PublicId;
                #endregion
            }
            //in case model.Image is null
            else if (!string.IsNullOrEmpty(book.ImageUrl))//model.Image is null & 
            {
                model.ImageUrl = book.ImageUrl;
                model.ImageThumbnailUrl = book.ImageThumbnailUrl; //by save thumbnail  image
            }
            book = _mapper.Map(model, book); 
            book.LastUpdatedOn = DateTime.Now;
            //book.ImagePublicId = imagePublicId; //cloudinary
            foreach (var item in model.SelectedCategoryIds)
            { //fill BookCategory table
                // here in foreach ordering wiht automapper is must هنا الترتيب لازم وعدم الترتيب يحدث مشكلة
                book.Categories.Add(new BookCategory { CategoryId = item });
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Details),new {id=book.Id}); // here(id field) must equal and be the same Details parameter(int id)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var book = _context.Books.Find(id);

            if (book is null)
                return NotFound();

            book.IsDeleted = !book.IsDeleted;
            book.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            return Ok();
        }
        public IActionResult AllowBook(BookFormViewModel model)
        {
            var book = _context.Books.SingleOrDefault(b => b.Title == model.Title && b.AuthorId == model.AuthorId);
            var isAllowed = book is null || book.Id.Equals(model.Id);

            return Json(isAllowed);
        }
         
        public BookFormViewModel PopulateViewModel(BookFormViewModel? model = null)
        {
            BookFormViewModel viewModel = model is null ? new BookFormViewModel() : model; // in case create(get&post) and edit(get&post)
            var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = _context.Categories.Where(c => !c.IsDeleted).OrderBy(c => c.Name).ToList();

            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            return viewModel;
        }
        private string GetThumbnailUrl(string Url)
        {
            var separator = "image/upload/";
            var UrlParts = Url.Split(separator);
            var thumbnailUrl = $"{UrlParts[0]}{separator}c_thumb,w_200,g_face/{UrlParts[1]}";
            return thumbnailUrl;
            #region more Details
            //https://res.cloudinary.com/bookifyabc/image/upload/v1702743649/npucw6wdntg1beeqohin.jpg
            //https://res.cloudinary.com/bookifyabc/image/upload/c_thumb,w_200,g_face/v1702743649/npucw6wdntg1beeqohin.jpg
            //after split
            //UrlParts[0] = https://res.cloudinary.com/bookifyabc/
            //UrlParts[1] = c_thumb,w_200,g_face/v1702743649/npucw6wdntg1beeqohin.jpg
            // (image/upload/) => this separator is deleted
            //(c_thumb,w_200,g_face/) => this is difference between orignal image and thumbnail image
            #endregion
        }
        private string GetPublicId(string imageUrl)
        {
            var separator = "image/upload/";
            var urlParts = imageUrl.Split(separator);
            if (urlParts.Length > 1)
            {
                var publicIdWithExtension = urlParts[1].Split("/")[1];
                return publicIdWithExtension.Split(".")[0];
            }
            return string.Empty;
        }
    }

}











