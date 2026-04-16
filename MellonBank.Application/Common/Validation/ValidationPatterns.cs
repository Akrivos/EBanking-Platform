namespace MellonBank.Application.Common.Validation
{
    public static class ValidationPatterns
    {
        public const string Afm = @"^\d{9}$";

        public const string PhoneNumber = @"^\+?[1-9]\d{7,14}$";

        public const string StrongPassword =
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$";
    }
}
