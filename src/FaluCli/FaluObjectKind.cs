namespace Falu;

public enum FaluObjectKind
{
    Payments,
    PaymentRefunds,
    Transfers,
    TransferReversals,
}

public static class FaluObjectKindExtensions
{
    private static readonly IReadOnlyDictionary<FaluObjectKind, string> suffixes = new Dictionary<FaluObjectKind, string>
    {
        [FaluObjectKind.Payments] = "Payments",
        [FaluObjectKind.PaymentRefunds] = "Payment Refunds",
        [FaluObjectKind.Transfers] = "Transfers",
        [FaluObjectKind.TransferReversals] = "Transfer Reversals",
    };

    public static string GetReadableName(this FaluObjectKind kind) => suffixes[kind];
}
