using FluentAssertions;
using Webapi.Application.AuthorOperations.Commands.UpdateAuthor;
using Xunit;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidatorTests
    {
        [Theory]
        [InlineData(0, "AuthorTest")]
        [InlineData(-1, "AuthorTest")]
        [InlineData(1, "abc")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int authorId, string authorName)
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.AuthorId = authorId;
            command.Model = new UpdateAuthorModel { Name = authorName, DateOfBirth = new System.DateTime(1750, 10, 15) };

            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
        {
            //arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.Model = new UpdateAuthorModel { Name = "TestAuthor", DateOfBirth = System.DateTime.Now.Date };

            //act
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.AuthorId = 1;
            command.Model = new UpdateAuthorModel { Name = "TestGenre", DateOfBirth = new System.DateTime(1950, 12, 29) };

            //act
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}