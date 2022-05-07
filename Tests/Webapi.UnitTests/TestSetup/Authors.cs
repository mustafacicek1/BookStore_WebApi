using System;
using Webapi.DbOperations;
using Webapi.Entities;

namespace TestSetup
{
    public static class Authors
    {
        public static void AddAuthors(this BookStoreDbContext context)
        {
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
        }
    }
}