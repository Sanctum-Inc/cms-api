namespace Contracts.Firm.Requests;

public sealed record GetFirmRequest
{
    public Guid? Id { get; init; }
}
