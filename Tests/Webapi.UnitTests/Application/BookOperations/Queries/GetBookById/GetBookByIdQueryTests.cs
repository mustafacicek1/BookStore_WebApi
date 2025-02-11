using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using Webapi.Application.BookOperations.Queries.GetBookById;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.BookOperations.Queries.GetBookById
{
    public class GetBookByIdQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBookByIdQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int bookId = 500;
            GetBookByIdQuery query = new GetBookByIdQuery(_context, _mapper);
            query.BookId = bookId;

            //act&assert
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Bulunamadı!");
        }

        [Fact]
        public void WhenExistBookIdIsGiven_Book_ShouldBeShown()
        {
            //arrange
            var model = new Book
            {
                AuthorId = 1,
                GenreId = 1,
                PageCount = 100,
                Title = "WhenExistBookIdIsGiven_Book_ShouldBeShown",
                PublishDate = DateTime.Now.AddYears(-2)
            };
            _context.Books.Add(model);
            _context.SaveChanges();

            GetBookByIdQuery query = new GetBookByIdQuery(_context, _mapper);
            query.BookId = model.Id;

            //act
            FluentActions.Invoking(() => query.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(b => b.Id == model.Id);
            book.Should().NotBeNull();
        }
    }
}