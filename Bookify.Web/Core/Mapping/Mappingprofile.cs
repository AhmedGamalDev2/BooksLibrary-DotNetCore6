using AutoMapper;

namespace Bookify.Web.Core.Mapping
{
    public class Mappingprofile:Profile
    {
        public Mappingprofile()
        {
            //map from Category to CategoryViewModel
            CreateMap<Category, CategoryViewModel>();
                //.ForMember(destination=> destination.CategoryName,opt=>opt.MapFrom(sourceMember => sourceMember.Name)); // in case the property not same name in Category and CategoryViewModel
          
            
            //map from CategoryFormViewModel to Category OR convert Category to CategoryFormViewModel
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();//ReverseMap inseated of ( CreateMap<Category, CategoryFormViewModel>();) // 


        }
    }
}
