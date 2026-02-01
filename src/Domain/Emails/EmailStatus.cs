namespace Domain.Emails;

public enum EmailStatus
{
    Pending = 0,
    Sending = 1,
    Sent = 2,
    Failed = 3,
    Retrying = 4,
    Cancelled = 5
}
