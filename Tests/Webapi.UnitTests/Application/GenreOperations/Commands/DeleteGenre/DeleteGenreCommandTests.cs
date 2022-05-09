using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using Webapi.Application.GenreOperations.Commands.DeleteGenre;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int genreId = 500;
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = genreId;

            //act&assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap türü bulunamadı");
        }

        [Fact]
        public void WhenExistGenreIdIsGiven_Genre_ShouldBeDeleted()
        {
            var model = new Genre { Name = "WhenExistGenreIdIsGiven_Genre_ShouldBeDeleted" };
            _context.Genres.Add(model);
            _context.SaveChanges();

            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = model.Id;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var genre = _context.Genres.SingleOrDefault(g => g.Id == model.Id);
            genre.Should().BeNull();
        }
    }
}