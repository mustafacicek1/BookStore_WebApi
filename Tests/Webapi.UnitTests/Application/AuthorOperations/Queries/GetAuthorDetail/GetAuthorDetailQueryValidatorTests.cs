using FluentAssertions;
using Webapi.Application.AuthorOperations.Queries.GetAuthorDetail;
using Xunit;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidAuthorIdIsGiven_Validator_ShouldBeReturnErrors(int authorId)
        {
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(null, null);
            query.AuthorId = authorId;

            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            var result = validator.Validate(query);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidAuthorIdIsGiven_Validator_ShouldNotBeReturnError()
        {
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(null, null);
            query.AuthorId = 1;

            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            var result = validator.Validate(query);

            result.Errors.Count.Should().Be(0);
        }
    }
}