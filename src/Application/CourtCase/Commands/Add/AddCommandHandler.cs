using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<Guid>>
{
    private readonly ICourtCaseRepository _courtCaseRepository;
    private readonly ISessionResolver _sessionResolver;

    public AddCommandHandler(
        ICourtCaseRepository courtCaseRepository,
        ISessionResolver sessionResolver)
    {
        _courtCaseRepository = courtCaseRepository;
        _sessionResolver = sessionResolver;
    }

    public async Task<ErrorOr<Guid>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var courtCase = new Domain.CourtCases.CourtCase
        {
            Id = Guid.NewGuid(),
            CaseNumber = request.CaseNumber,
            Location = request.Location,
            Plaintiff = request.Plaintiff,
            Defendant = request.Defendant,
            Status = request.Status,
            Type = request.Type,
            Outcome = request.Outcome,
            DateCreated = DateTime.UtcNow,
            UserId = Guid.Parse(userId)
        };

        await _courtCaseRepository.AddAsync(courtCase, cancellationToken);
        await _courtCaseRepository.SaveChangesAsync(cancellationToken);

        return courtCase.Id;
    }
}