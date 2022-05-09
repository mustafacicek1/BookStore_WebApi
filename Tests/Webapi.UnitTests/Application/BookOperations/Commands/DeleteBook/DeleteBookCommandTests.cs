using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using Webapi.Application.BookOperations.Commands.DeleteBook;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int bookId = 500;
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = bookId;

            //act&assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap bulunamadı");
        }

        [Fact]
        public void WhenExistBookIdIsGiven_Book_ShouldBeDeleted()
        {
            //arrange
            var model = new Book
            {
                AuthorId = 1,
                GenreId = 1,
                PageCount = 100,
                Title = "WhenExistBookIdIsGiven_Book_ShouldBeDeleted",
                PublishDate = DateTime.Now.AddYears(-2)
            };
            _context.Books.Add(model);
            _context.SaveChanges();

            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = model.Id;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(b => b.Id == model.Id);
            book.Should().BeNull();
        }
    }
}