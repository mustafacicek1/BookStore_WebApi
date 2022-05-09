using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using Webapi.Application.BookOperations.Commands.UpdateBook;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;

        public UpdateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int bookId = 500;
            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = bookId;

            //act&assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            //arrange
            var book = new Book
            {
                AuthorId = 1,
                GenreId = 1,
                PageCount = 100,
                Title = "WhenValidInputsAreGiven_Book_ShouldBeUpdated",
                PublishDate = DateTime.Now.AddYears(-2)
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = book.Id;

            UpdateBookModel updateBookModel = new UpdateBookModel { AuthorId = 2, GenreId = 2, PageCount = 500, PublishDate = new DateTime(1970, 10, 01), Title = "UpdatedTitle" };
            command.Model = updateBookModel;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var updatedBook = _context.Books.SingleOrDefault(b => b.Id == book.Id);
            book.Should().NotBeNull();
            book.AuthorId.Should().Be(updateBookModel.AuthorId);
            book.Title.Should().Be(updateBookModel.Title);
            book.GenreId.Should().Be(updateBookModel.GenreId);
            book.PageCount.Should().Be(updateBookModel.PageCount);
            book.PublishDate.Should().Be(updateBookModel.PublishDate);
        }
    }
}