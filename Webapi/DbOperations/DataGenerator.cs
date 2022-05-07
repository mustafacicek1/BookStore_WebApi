using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Webapi.Entities;

namespace Webapi.DbOperations
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                {
                    return;
                }

                context.Genres.AddRange(new Genre
                {
                    Name = "Personal Growth"
                },
                new Genre
                {
                    Name = "Science Fiction"
                },
                new Genre
                {
                    Name = "Romance"
                });

                context.Authors.AddRange(new Author
                {
                    Name = "John Ronald Reuel Tolkien",
                    DateOfBirth = DateTime.Parse("1892-01-03")
                },
                new Author
                {
                    Name = "Fyodor Dostoyevski",
                    DateOfBirth = DateTime.Parse("1821-11-11")
                },
                new Author
                {
                    Name = "Eric Ries",
                    DateOfBirth = DateTime.Parse("1978-09-22")
                });

                context.Books.AddRange(new Book()
                {
                    Title = "Lean Startup",
                    GenreId = 1,
                    AuthorId = 3,
                    PageCount = 200,
                    PublishDate = new DateTime(2010, 03, 15)
                },
                new Book()
                {
                    Title = "Lord Of The Rings",
                    GenreId = 2,
                    AuthorId = 1,
                    PageCount = 1000,
                    PublishDate = new DateTime(2001, 02, 12)
                },
                new Book()
                {
                    Title = "Hobbit",
                    GenreId = 2,
                    AuthorId = 1,
                    PageCount = 1100,
                    PublishDate = new DateTime(2005, 01, 04)
                });

                context.SaveChanges();
            }
        }
    }
}