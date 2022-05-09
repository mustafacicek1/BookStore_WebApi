using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using Webapi.Application.GenreOperations.Queries.GetGenreDetail;
using Webapi.DbOperations;
using Webapi.Entities;
using Xunit;

namespace Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetGenreDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            int genreId = 500;
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = genreId;

            //act&assert
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap türü bulunamadı");
        }

        [Fact]
        public void WhenExistGenreIdIsGiven_Genre_ShouldBeShown()
        {
            //arrange
            var model = new Genre { Name = "WhenExistGenreIdIsGiven_Genre_ShouldBeShown" };
            _context.Genres.Add(model);
            _context.SaveChanges();

            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = model.Id;

            //act
            FluentActions.Invoking(() => query.Handle()).Invoke();

            //assert
            var genre = _context.Genres.SingleOrDefault(b => b.Id == model.Id);
            genre.Should().NotBeNull();
        }
    }
}