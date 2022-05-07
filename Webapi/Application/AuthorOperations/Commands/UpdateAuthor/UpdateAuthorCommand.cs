using System;
using System.Linq;
using Webapi.DbOperations;

namespace Webapi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand
    {
        public int AuthorId { get; set; }
        public UpdateAuthorModel Model { get; set; }
        private readonly IBookStoreDbContext _context;

        public UpdateAuthorCommand(IBookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(a => a.Id == AuthorId);
            if (author is null)
                throw new InvalidOperationException("Yazar bulunamadı");

            if (_context.Authors.Any(a => a.Name.ToLower() == Model.Name.ToLower() && a.Id != AuthorId))
                throw new InvalidOperationException("Aynı isimde bir yazar zaten mevcut");

            author.Name = string.IsNullOrEmpty(Model.Name.Trim()) ? author.Name : Model.Name;
            author.DateOfBirth = Model.DateOfBirth == default ? author.DateOfBirth : Model.DateOfBirth;
            _context.SaveChanges();
        }
    }

    public class UpdateAuthorModel
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}