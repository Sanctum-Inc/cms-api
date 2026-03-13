using ErrorOr;

namespace Domain.Common.CommonErrors;

public static partial class Errors
{
    public static class Document
    {
        public static Error NotFound =>
            Error.NotFound(
                code: "2001",
                description: "Document Not Found."
            );

    }
}
