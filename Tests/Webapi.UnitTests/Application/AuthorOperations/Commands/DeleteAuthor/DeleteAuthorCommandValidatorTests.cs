using FluentAssertions;
using Webapi.Application.AuthorOperations.Commands.DeleteAuthor;
using Xunit;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidAuthorIdIsGiven_Validator_ShouldBeReturnErrors(int authorId)
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand(null);
            command.AuthorId = authorId;

            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidGenreIdIsGiven_Validator_ShouldNotBeReturnError()
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand(null);
            command.AuthorId = 1;

            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}