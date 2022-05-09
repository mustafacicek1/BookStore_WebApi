using FluentAssertions;
using Webapi.Application.BookOperations.Commands.UpdateBook;
using Xunit;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandValidatorTests
    {
        [Theory]
        [InlineData(0, "Lord Of The Rings", 1, 1, 1)]
        [InlineData(-5, "Lord Of The Rings", 1, 1, 1)]
        [InlineData(1, "", 1, 1, 1)]
        [InlineData(1, "Lord Of The RingsLord Of The RingsLord Of The", 1, 1, 1)]
        [InlineData(10, "Lord Of The Rings", 0, 1, 1)]
        [InlineData(10, "Lord Of The Rings", -5, 1, 1)]
        [InlineData(5, "Lord Of The Rings", 1, 0, 1)]
        [InlineData(5, "Lord Of The Rings", 1, -3, 1)]
        [InlineData(1, "Lord Of The Rings", 1, 1, 0)]
        [InlineData(1, "Lord Of The Rings", 1, 1, -10)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int bookId, string title, int pageCount, int genreId, int authorId)
        {
            //arrange
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = bookId;
            command.Model = new UpdateBookModel { Title = title, PageCount = pageCount, PublishDate = System.DateTime.Now.AddYears(-1), GenreId = genreId, AuthorId = authorId };

            //act
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
        {
            //arrange
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = 1;
            command.Model = new UpdateBookModel { Title = "TestTitle", PageCount = 100, PublishDate = System.DateTime.Now.Date, GenreId = 1, AuthorId = 1 };

            //act
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = 1;
            command.Model = new UpdateBookModel { Title = "TestTitle", PageCount = 100, PublishDate = System.DateTime.Now.Date.AddYears(-2), GenreId = 1, AuthorId = 1 };

            //act
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}