using System;
using FluentAssertions;
using Webapi.Application.AuthorOperations.Commands.CreateAuthor;
using Xunit;

namespace Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandValidatorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("abc")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string authorName)
        {
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel { Name = authorName, DateOfBirth = new System.DateTime(1950, 5, 1) };

            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
        {
            //arrange
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel { Name = "TestAuthor", DateOfBirth = DateTime.Now.Date };

            //act
            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel { Name = "TestAuthor" };

            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}