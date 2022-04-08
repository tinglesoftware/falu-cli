using Falu.Client.Events;
using Falu.Client.Money;
using Falu.Client.Payments;
using Microsoft.Extensions.Options;

namespace Falu.Client;

internal class FaluCliClient : FaluClient
{
    public FaluCliClient(HttpClient backChannel, IOptionsSnapshot<FaluClientOptions> optionsAccessor)
        : base(backChannel, optionsAccessor)
    {
        Events = new ExtendedEventsServiceClient(BackChannel, Options);
        Payments = new ExtendedPaymentsServiceClient(BackChannel, Options);
        Transfers = new ExtendedTransfersServiceClient(BackChannel, Options);
        PaymentRefunds = new ExtendedPaymentRefundsServiceClient(BackChannel, Options);
        TransferReversals = new ExtendedTransferReversalsServiceClient(BackChannel, Options);
    }

    public new ExtendedEventsServiceClient Events { get; protected set; }
    public new ExtendedPaymentsServiceClient Payments { get; protected set; }
    public new ExtendedTransfersServiceClient Transfers { get; protected set; }
    public new ExtendedPaymentRefundsServiceClient PaymentRefunds { get; protected set; }
    public new ExtendedTransferReversalsServiceClient TransferReversals { get; protected set; }
}
