using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using Webapi.Application.AuthorOperations.Commands.UpdateAuthor;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
        public UpdateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int authorId = 500;
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = authorId;

            //act&assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar bulunamadı");
        }

        [Fact]
        public void WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var author1 = new Author { Name = "AuthorName" };
            var author2 = new Author { Name = "TestAuthor" };
            _context.Authors.AddRange(author1, author2);
            _context.SaveChanges();

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = author2.Id;
            command.Model = new UpdateAuthorModel { Name = author1.Name };

            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Aynı isimde bir yazar zaten mevcut");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeUpdated()
        {
            //arrange
            var author = new Author { Name = "WhenValidInputsAreGiven_Author_ShouldBeUpdated", DateOfBirth = new DateTime(1850, 01, 01) };
            _context.Authors.Add(author);
            _context.SaveChanges();

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = author.Id;

            UpdateAuthorModel model = new UpdateAuthorModel
            {
                Name = "UpdatedAuthorName",
                DateOfBirth = new DateTime(1950, 01, 05)
            };
            command.Model = model;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var updatedAuthor = _context.Authors.SingleOrDefault(b => b.Id == author.Id);
            updatedAuthor.Should().NotBeNull();
            updatedAuthor.Name.Should().Be(model.Name);
            updatedAuthor.DateOfBirth.Should().Be(model.DateOfBirth);
        }
    }
}