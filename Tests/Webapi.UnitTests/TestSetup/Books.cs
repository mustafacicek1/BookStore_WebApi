using System;
using Webapi.DbOperations;
using Webapi.Entities;

namespace TestSetup
{
    public static class Books
    {
        public static void AddBooks(this BookStoreDbContext context)
        {
            context.Books.AddRange(
                new Book()
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
        }
    }
}