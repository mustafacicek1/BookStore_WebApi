using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using Webapi.Application.GenreOperations.Commands.UpdateGenre;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;

        public UpdateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int genreId = 500;
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genreId;

            //act&assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap türü bulunamadı");
        }

        [Fact]
        public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var genre1 = new Genre { Name = "WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn" };
            var genre2 = new Genre { Name = "TestGenre" };
            _context.Genres.AddRange(genre1, genre2);
            _context.SaveChanges();

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genre2.Id;
            command.Model = new UpdateGenreModel { Name = genre1.Name };

            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Aynı isimli bir kitap türü zaten mevcut");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeUpdated()
        {
            //arrange
            var genre = new Genre { Name = "WhenValidInputsAreGiven_Genre_ShouldBeUpdated" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genre.Id;

            UpdateGenreModel model = new UpdateGenreModel
            {
                Name = "UpdatedGenreName",
                IsActive = false
            };
            command.Model = model;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var updatedGenre = _context.Genres.SingleOrDefault(b => b.Id == genre.Id);
            updatedGenre.Should().NotBeNull();
            updatedGenre.Name.Should().Be(model.Name);
            updatedGenre.IsActive.Should().Be(model.IsActive);
        }
    }
}