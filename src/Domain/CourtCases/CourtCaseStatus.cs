namespace Domain.CourtCases;

public enum CourtCaseStatus
{
    // Case has been created but not yet filed
    Draft = 0,

    // Case has been officially filed with the court
    Filed = 1,

    // Awaiting first hearing / scheduling
    Pending = 2,

    // Court dates are scheduled
    Scheduled = 3,

    // Case is currently being heard
    InProgress = 4,

    // Temporarily paused (e.g. awaiting documents, settlement talks)
    OnHold = 5,

    // Case postponed to a later date
    Postponed = 6,

    // Case resolved by settlement
    Settled = 7,

    // Court delivered a judgment
    JudgementDelivered = 8,

    // Case finalized and closed
    Closed = 9,

    // Case dismissed by the court
    Dismissed = 10,

    // Case withdrawn by a party
    Withdrawn = 11,

    // Case cancelled before hearing
    Cancelled = 12
}
