namespace Domain.CourtCaseDates;

public enum CourtCaseDateTypes
{
    Unknown = 0,

    // Hearings
    Hearing = 1,
    Trial = 2,
    MotionHearing = 3,
    PreTrialConference = 4,

    // Administrative
    FilingDeadline = 5,
    SubmissionDeadline = 6,
    ResponseDeadline = 7,

    // Case lifecycle
    Arraignment = 8,
    Sentencing = 9,
    Judgment = 10,
    SettlementConference = 11,

    // Misc
    Mention = 12,
    StatusConference = 13,
    Mediation = 14
}
