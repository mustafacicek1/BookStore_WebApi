using FluentAssertions;
using Webapi.Application.GenreOperations.Commands.CreateGenre;
using Xunit;

namespace Application.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandValidatorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("abc")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string genreName)
        {
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel { Name = genreName };

            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel { Name = "TestGenre" };

            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}