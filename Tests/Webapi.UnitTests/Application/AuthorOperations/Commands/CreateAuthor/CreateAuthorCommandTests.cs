using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using Webapi.Application.AuthorOperations.Commands.CreateAuthor;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public CreateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var author = new Author { Name = "WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn" };
            _context.Authors.Add(author);
            _context.SaveChanges();

            CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
            command.Model = new CreateAuthorModel { Name = author.Name };

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar zaten mevcut");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeCreated()
        {
            CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
            var model = new CreateAuthorModel { Name = "WhenValidInputsAreGiven_Author_ShouldBeCreated", DateOfBirth = new DateTime(1970, 10, 05) };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var author = _context.Authors.SingleOrDefault(g => g.Name == model.Name);
            author.Should().NotBeNull();
            author.DateOfBirth.Should().Be(model.DateOfBirth);
        }
    }
}