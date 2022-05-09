using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using Webapi.Application.GenreOperations.Commands.CreateGenre;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
        public CreateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var genre = new Genre { Name = "WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = new CreateGenreModel { Name = genre.Name };

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap türü zaten mevcut");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeCreated()
        {
            CreateGenreCommand command = new CreateGenreCommand(_context);
            var model = new CreateGenreModel { Name = "WhenValidInputsAreGiven_Genre_ShouldBeCreated" };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var genre = _context.Genres.SingleOrDefault(g => g.Name == model.Name);
            genre.Should().NotBeNull();
            genre.IsActive.Should().Be(true);
        }
    }
}