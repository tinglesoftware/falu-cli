namespace Falu.Commands.Money;

public class UploadMpesaPaymentsStatementCommand : UploadMpesaStatementCommand
{
    public UploadMpesaPaymentsStatementCommand() : base("Upload an MPESA statement to Falu for payments.") { }
}

public class UploadMpesaPaymentRefundsStatementCommand : UploadMpesaStatementCommand
{
    public UploadMpesaPaymentRefundsStatementCommand() : base("Upload an MPESA statement to Falu for payment refunds.") { }
}

public class UploadMpesaTransfersStatementCommand : UploadMpesaStatementCommand
{
    public UploadMpesaTransfersStatementCommand() : base("Upload an MPESA statement to Falu for transfers.") { }
}

public class UploadMpesaTransferReversalsStatementCommand : UploadMpesaStatementCommand
{
    public UploadMpesaTransferReversalsStatementCommand() : base("Upload an MPESA statement to Falu for transfer reversals.") { }
}

public abstract class UploadMpesaStatementCommand : Command
{
    public UploadMpesaStatementCommand(string description) : base("mpesa-upload", description)
    {
        this.AddOption<string>(new[] { "-f", "--file", },
                               description: $"File path for the statement file (upto {Constants.MaxMpesaStatementFileSizeString}).",
                               configure: o => o.IsRequired = true);
    }
}
