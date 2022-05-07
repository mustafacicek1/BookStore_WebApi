using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Webapi.Common;
using Webapi.DbOperations;

namespace Webapi.Application.BookOperations.Queries.GetBookById
{
    public class GetBookByIdQuery
    {
        public int BookId { get; set; }
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBookByIdQuery(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BookViewModel Handle()
        {
            var book = _context.Books.Include(b => b.Genre).Include(b => b.Author).SingleOrDefault(b => b.Id == BookId);
            if (book is null)
                throw new InvalidOperationException("Kitap Bulunamadı!");

            BookViewModel vm = _mapper.Map<BookViewModel>(book);

            return vm;
        }
    }

    public class BookViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }
    }
}