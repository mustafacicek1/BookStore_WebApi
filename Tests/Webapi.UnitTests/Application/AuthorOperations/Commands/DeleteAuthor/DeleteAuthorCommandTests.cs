using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using Webapi.Application.AuthorOperations.Commands.DeleteAuthor;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int authorId = 500;
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = authorId;

            //act&assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar bulunamadı");
        }

        [Fact]
        public void WhenAuthorAlreadyHasPublishedBook_InvalidOperationException_ShouldBeReturn()
        {
            var author = new Author { Name = "TestAuthor", DateOfBirth = new DateTime(1750, 01, 01) };
            _context.Authors.Add(author);

            var book = new Book { AuthorId = author.Id };
            _context.Books.Add(book);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = author.Id;

            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazarın yayında kitabı bulunuyor!");
        }

        [Fact]
        public void WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted()
        {
            var model = new Author { Name = "WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted" };
            _context.Authors.Add(model);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = model.Id;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var author = _context.Authors.SingleOrDefault(g => g.Id == model.Id);
            author.Should().BeNull();
        }
    }
}