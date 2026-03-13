using ErrorOr;

namespace Domain.Common.CommonErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound =>
            Error.NotFound(
                code: "1001",
                description: "User Not Found."
            );

        public static Error EmailNotVerified =>
            Error.Validation(
                code: "1002",
                description: "Email Not Verified."
            );

        public static Error InvalidToken =>
            Error.Validation(
                code: "1003",
                description: "Invalid Token."
            );

    }
}
