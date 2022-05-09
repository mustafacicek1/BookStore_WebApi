using FluentAssertions;
using Webapi.Application.GenreOperations.Commands.DeleteGenre;
using Xunit;

namespace Application.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidGenreIdIsGiven_Validator_ShouldBeReturnErrors(int genreId)
        {
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = genreId;

            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidGenreIdIsGiven_Validator_ShouldNotBeReturnError()
        {
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = 1;

            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}