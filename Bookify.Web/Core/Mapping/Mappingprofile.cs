﻿using AutoMapper;
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
            CreateMap<Author, AuthorViewModel>();
            CreateMap<AuthorFormViewModel, Author>().ReverseMap();

            CreateMap<Author, SelectListItem>()
              .ForMember(destListItem => destListItem.Value, options => options.MapFrom(sourceAuthor => sourceAuthor.Id))
              .ForMember(destListItem => destListItem.Text, options => options.MapFrom(sourceAuthor => sourceAuthor.Name));

            //Book
            CreateMap<BookFormViewModel, Book>();

        }
    }
}
