using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Bookify.Web.Core.Mapping
{
    public class Mappingprofile:Profile
    {
        public Mappingprofile()
        {
            //Category
            //map from Category to CategoryViewModel
            CreateMap<Category, CategoryViewModel>();
            //.ForMember(destination=> destination.CategoryName,opt=>opt.MapFrom(sourceMember => sourceMember.Name)); // in case the property not same name in Category and CategoryViewModel
            //destination is CategoryViewModel will get it value from source(Category)

            //map from CategoryFormViewModel to Category OR convert Category to CategoryFormViewModel
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();//ReverseMap inseated of ( CreateMap<Category, CategoryFormViewModel>();) // 

            CreateMap<Category, SelectListItem>()
                .ForMember(destListItem => destListItem.Value, options => options.MapFrom(sourceCategory => sourceCategory.Id))
                .ForMember(destListItem => destListItem.Text, options => options.MapFrom(sourceCategory => sourceCategory.Name));

           
            //Author
            CreateMap<Author, AuthorViewModel>(); //source:Author, destination:AuthorViewModel
            CreateMap<AuthorFormViewModel, Author>().ReverseMap();

            CreateMap<Author, SelectListItem>()
              .ForMember(destListItem => destListItem.Value, options => options.MapFrom(sourceAuthor => sourceAuthor.Id))
              .ForMember(destListItem => destListItem.Text, options => options.MapFrom(sourceAuthor => sourceAuthor.Name));

            //Book
            CreateMap<BookFormViewModel, Book>()
                .ReverseMap()
                .ForMember(dest=>dest.Categories,opt=>opt.Ignore()); //this is because there are two the same name with (Categories) in Category and CategoryFormViewModel with different type

            CreateMap<Book, BookViewModel>()
              .ForMember(destBookViewModel => destBookViewModel.Author, options => options.MapFrom(sourceBook => sourceBook.Author!.Name))
              .ForMember(destBookViewModel => destBookViewModel.Categories, options => options.MapFrom(sourceBook => sourceBook.Categories.Select(category => category.Category!.Name).ToList()));

            CreateMap<BookCopy, BookCopyViewModel>()
                   .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book!.Title));

            CreateMap<BookCopy, BookCopyFormViewModel>();

            //Users
            CreateMap<ApplicationUser, UserViewModel>();  //source:ApplicationUser,dest:UserViewModel
            CreateMap<UserFormViewModel, ApplicationUser>()
                .ForMember(dest => dest.NormalizedEmail, operation => operation.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedUserName, operation => operation.MapFrom(src => src.Username.ToUpper()))
                .ReverseMap();  //source:UserFormViewModel,dest:ApplicationUser => used in edit not in create action

            //first way to map IdentityRole to SelectListItem(1)
            CreateMap<IdentityRole, SelectListItem>() //source :identityRolr , dest :SelectListItem
                .ForMember(destListItem =>destListItem.Value, options => options.MapFrom(sourceIdentityRole=> sourceIdentityRole.Name))
                .ForMember(destListItem =>destListItem.Text, options => options.MapFrom(sourceIdentityRole=> sourceIdentityRole.Name));
               
            CreateMap<IEnumerable<SelectListItem>, UserFormViewModel>() //source:SelectListItem,dest:UserFormViewModel
                .ForMember(destBookViewModel => destBookViewModel.Roles, options => options.MapFrom(sourceBook => sourceBook.Select(category => category.Value).ToList()));

            CreateMap<UserFormViewModel, UserViewModel>();//source:UserFormViewModel,dest:ApplicationUser
            #region Note (don't use automapper with ApplicationUser as Destination => only use new ApplicationUser => mostly in create action
            // CreateMap<UserFormViewModel, ApplicationUser>(); //source:UserFormViewModel,dest:ApplicationUser
            // var user = _mapper.Map<ApplicationUser>(user); //do not do that
            //here we can not use automapper with ApplicationUser as Destination with UserFormViewModel
            //because automapper will initialize Id (in ApplicationUser) with null
            //but when using new keyword (in new ApplicationUser) ,it(new) will initialize Id (in ApplicationUser) with (new Guid)
            #endregion
        }
    }
}
