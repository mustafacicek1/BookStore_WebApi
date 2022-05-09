using FluentAssertions;
using Webapi.Application.GenreOperations.Commands.UpdateGenre;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidatorTests
    {
        [Theory]
        [InlineData(0, "GenreTest")]
        [InlineData(-1, "GenreTest")]
        [InlineData(1, "abc")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int genreId, string genreName)
        {
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.GenreId = genreId;
            command.Model = new UpdateGenreModel { Name = genreName };

            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.GenreId = 1;
            command.Model = new UpdateGenreModel { Name = "TestGenre" };

            //act
            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}