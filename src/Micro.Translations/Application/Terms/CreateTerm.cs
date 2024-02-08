﻿using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application.Terms;

public static class CreateTerm
{
    public record Command(Guid TermId, Guid AppId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ICurrentContext context, ITermRepository terms) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var termId = new TermId(command.TermId);
            var organisationId = context.OrganisationId;
            var appId = new AppId(command.AppId);
            var name = new TermName(command.Name);
            var term = new Term(termId, organisationId, appId, name);
            await terms.CreateAsync(term);
            return Unit.Value;
        }
    }
}