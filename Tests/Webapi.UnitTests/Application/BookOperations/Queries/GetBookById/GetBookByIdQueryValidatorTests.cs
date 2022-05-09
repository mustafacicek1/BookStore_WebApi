using FluentAssertions;
using Webapi.Application.BookOperations.Queries.GetBookById;
using Xunit;

namespace Application.BookOperations.Queries.GetBookById
{
    public class GetBookByIdQueryValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidBookIdIsGiven_Validator_ShouldBeReturnErrors(int bookId)
        {
            GetBookByIdQuery query = new GetBookByIdQuery(null, null);
            query.BookId = bookId;

            GetBookByIdQueryValidator validator = new GetBookByIdQueryValidator();
            var result = validator.Validate(query);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidBookIdIsGiven_Validator_ShouldNotBeReturnError()
        {
            GetBookByIdQuery query = new GetBookByIdQuery(null, null);
            query.BookId = 1;

            GetBookByIdQueryValidator validator = new GetBookByIdQueryValidator();
            var result = validator.Validate(query);

            result.Errors.Count.Should().Be(0);
        }
    }
}