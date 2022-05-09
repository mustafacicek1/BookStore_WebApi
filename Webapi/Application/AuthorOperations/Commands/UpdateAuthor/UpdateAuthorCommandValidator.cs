using System;
using FluentValidation;

namespace Webapi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.AuthorId).GreaterThan(0);
            RuleFor(command => command.Model.Name).MinimumLength(4).When(x => x.Model.Name != string.Empty);
            RuleFor(command => command.Model.DateOfBirth.Date).LessThan(DateTime.Now.Date).When(x => x.Model.DateOfBirth != default);
        }
    }
}