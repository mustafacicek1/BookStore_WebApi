using FluentAssertions;
using TestSetup;
using Webapi.Application.BookOperations.Commands.CreateBook;
using Xunit;

namespace Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("Lord Of The Rings", 0, 0, 0)]
        [InlineData("Lord Of The Rings", 1, 0, 0)]
        [InlineData("Lord Of The Rings", 0, 1, 0)]
        [InlineData("Lord Of The Rings", 0, 0, 1)]
        [InlineData("", 0, 0, 0)]
        [InlineData("", 0, 100, 1)]
        [InlineData("Lo", 200, -1, 5)]
        [InlineData("Lord", 1, 0, 1)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string title, int pageCount, int genreId, int authorId)
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new BookCreateModel { Title = title, PageCount = pageCount, PublishDate = System.DateTime.Now.AddYears(-1), GenreId = genreId, AuthorId = authorId };

            //act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGıven_Validator_ShouldBeReturnError()
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new BookCreateModel { Title = "TestTitle", PageCount = 100, PublishDate = System.DateTime.Now.Date, GenreId = 1, AuthorId = 1 };

            //act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGıven_Validator_ShouldNotBeReturnError()
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new BookCreateModel { Title = "TestTitle", PageCount = 100, PublishDate = System.DateTime.Now.Date.AddYears(-2), GenreId = 1, AuthorId = 1 };

            //act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}