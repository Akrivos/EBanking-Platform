namespace MellonBank.Application.Common.Models
{
    public class Result
    {
        public bool Succeeded { get; init; }
        public string? Error { get; init; }

        public static Result Success()
        {
            return new() { Succeeded = true };
        }
        public static Result Failure(string error) {
            return new() { Succeeded = false, Error = error };
        }
    }
}
