using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using Webapi.Application.AuthorOperations.Queries.GetAuthorDetail;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int authorId = 500;
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = authorId;

            //act&assert
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar bulunamadı");
        }

        [Fact]
        public void WhenExistAuthorIdIsGiven_Author_ShouldBeShown()
        {
            //arrange
            var model = new Author
            {
                Name = "WhenExistAuthorIdIsGiven_Author_ShouldBeShown",
                DateOfBirth = new DateTime(1850, 05, 06)
            };
            _context.Authors.Add(model);
            _context.SaveChanges();

            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = model.Id;

            //act
            FluentActions.Invoking(() => query.Handle()).Invoke();

            //assert
            var author = _context.Authors.SingleOrDefault(b => b.Id == model.Id);
            author.Should().NotBeNull();
        }
    }
}