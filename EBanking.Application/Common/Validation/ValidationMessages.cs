namespace EBanking.Application.Common.Validation
{
    public static class ValidationMessages
    {
        public const string Afm =
            "AFM must be exactly 9 digits.";

        public const string PhoneNumber =
            "Phone number must be a valid international format (8 to 15 digits, optional + prefix).";

        public const string StrongPassword =
            "Password must contain uppercase, lowercase, number and special character.";
    }
}
