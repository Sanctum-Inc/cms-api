namespace Domain.CourtCases;

public enum CourtCaseOutcomes
{
    None = 0,                 // No outcome yet / ongoing
    Guilty = 1,               // Criminal conviction
    NotGuilty = 2,            // Acquittal
    Liable = 3,               // Civil liability found
    NotLiable = 4,            // Civil claim dismissed
    Settled = 5,              // Resolved via settlement
    Withdrawn = 6,            // Withdrawn by a party
    Dismissed = 7,            // Dismissed by court
    StruckOff = 8,            // Removed due to procedural issues
    Postponed = 9,            // No final outcome, delayed
    JudgmentForPlaintiff = 10,
    JudgmentForDefendant = 11,
    PartiallyGranted = 12,    // Some relief granted
    Appealed = 13,            // Decision appealed
    Overturned = 14           // Overturned on appeal
}
