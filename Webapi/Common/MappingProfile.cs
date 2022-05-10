using AutoMapper;
using Webapi.Application.AuthorOperations.Commands.CreateAuthor;
using Webapi.Application.AuthorOperations.Queries.GetAuthorDetail;
using Webapi.Application.AuthorOperations.Queries.GetAuthors;
using Webapi.Application.BookOperations.Commands.CreateBook;
using Webapi.Application.BookOperations.Queries.GetBookById;
using Webapi.Application.BookOperations.Queries.GetBooks;
using Webapi.Application.GenreOperations.Queries.GetGenreDetail;
using Webapi.Application.GenreOperations.Queries.GetGenres;
using Webapi.Application.UserOperations.Commands.CreateUser;
using Webapi.Entities;

namespace Webapi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookCreateModel, Book>();

            CreateMap<Book, BookViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Name));

            CreateMap<Book, BooksViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Name));

            CreateMap<Genre, GenresViewModel>();

            CreateMap<Genre, GenreDetailViewModel>();

            CreateMap<CreateAuthorModel, Author>();

            CreateMap<Author, AuthorsViewModel>();

            CreateMap<Author, AuthorDetailViewModel>();

            CreateMap<CreateUserModel, User>();
        }
    }
}