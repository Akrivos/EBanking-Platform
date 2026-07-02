namespace EBanking.Application.Common.Models
{
    public sealed record PagedData<T>(
        IReadOnlyList<T> Items,
        int TotalCount
    );
}
