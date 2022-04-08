using Falu.Client;
using Falu.Client.Money;
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
        var type = context.ParseResult.ValueForArgument<MpesaStatementType>("type");
        var filePath = context.ParseResult.ValueForOption<string>("--file");

        Uploader uploader = type switch
        {
            MpesaStatementType.Payments => client.MoneyMpesa.UploadPaymentsAsync,
            MpesaStatementType.Transfers => client.MoneyMpesa.UploadTransfersAsync,
            _ => throw new NotSupportedException($"'{nameof(MpesaStatementType)}.{type}' is not supported for this command."),
        };

        // ensure the directory exists
        if (!File.Exists(filePath))
        {
            logger.LogError("The file {FilePath} does not exist.", filePath);
            return -1;
        }

        var fileName = Path.GetFileName(filePath);
        using var fileContent = File.OpenRead(filePath);
        var response = await uploader(fileName, fileContent, cancellationToken: cancellationToken);
        response.EnsureSuccess();

        var extracted = response.Resource!;
        var receiptNumbers = extracted.Select(r => r.Receipt).ToList();
        logger.LogInformation("Uploaded statement successfully. Imported/Updated {ImportedCount} records.", extracted.Count);
        logger.LogDebug("Imported/Updated Receipt Numbers:\r\n-{ReceiptNumbers}", string.Join("\r\n-", receiptNumbers));

        return 0;
    }

    private delegate Task<ResourceResponse<List<ExtractedMpesaStatementRecord>>> Uploader(string fileName,
                                                                                          Stream fileContent,
                                                                                          RequestOptions? options = null,
                                                                                          CancellationToken cancellationToken = default);
}
