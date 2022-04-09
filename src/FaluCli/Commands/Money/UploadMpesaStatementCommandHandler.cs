using Falu.Client;
using Falu.Core;
using System.IO;

namespace Falu.Commands.Money;

internal class UploadMpesaStatementCommandHandler : ICommandHandler
{
    public FaluCliClient client;
    private readonly ILogger logger;

    public UploadMpesaStatementCommandHandler(FaluCliClient client, ILogger<UploadMpesaStatementCommandHandler> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();
        var filePath = context.ParseResult.ValueForOption<string>("--file")!;

        var command = context.ParseResult.CommandResult.Command;
        Uploader uploader = command switch
        {
            UploadMpesaPaymentsStatementCommand => client.Payments.UploadMpesaAsync,
            UploadMpesaTransfersStatementCommand => client.Transfers.UploadMpesaAsync,
            UploadMpesaPaymentRefundsStatementCommand => client.PaymentRefunds.UploadMpesaAsync,
            UploadMpesaTransferReversalsStatementCommand => client.TransferReversals.UploadMpesaAsync,
            _ => throw new InvalidOperationException($"Command of type '{command.GetType().FullName}' is not supported here."),
        };

        // ensure the file exists
        var info = new FileInfo(filePath);
        if (!info.Exists)
        {
            logger.LogError("The file {FilePath} does not exist.", filePath);
            return -1;
        }

        // ensure the file size does not exceed the limit
        var size = ByteSizeLib.ByteSize.FromBytes(info.Length);
        if (size > Constants.MaxMpesaStatementFileSize)
        {
            logger.LogError("The file provided exceeds the size limit of {SizeLimit}. Trying exporting a smaller date range.", Constants.MaxMpesaStatementFileSizeString);
            return -1;
        }

        var fileName = Path.GetFileName(filePath);
        using var fileContent = File.OpenRead(filePath);
        var response = await uploader(fileName, fileContent, cancellationToken: cancellationToken);
        response.EnsureSuccess();

        var extracted = response.Resource!;
        var receiptNumbers = extracted.Select(r => r.Receipt).ToList();
        logger.LogInformation("Uploaded statement successfully. Imported/Updated {ImportedCount} records.", extracted.Count);
        if (extracted.Count > 0)
        {
            logger.LogDebug("Imported/Updated Receipt Numbers:\r\n-{ReceiptNumbers}", string.Join("\r\n-", receiptNumbers));
        }

        return 0;
    }

    private delegate Task<ResourceResponse<List<ExtractedMpesaStatementRecord>>> Uploader(string fileName,
                                                                                          Stream fileContent,
                                                                                          RequestOptions? options = null,
                                                                                          CancellationToken cancellationToken = default);
}
