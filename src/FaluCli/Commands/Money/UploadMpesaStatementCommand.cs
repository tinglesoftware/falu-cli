﻿namespace Falu.Commands.Money;

public class UploadMpesaStatementCommand : Command
{
    public UploadMpesaStatementCommand() : base("upload", "Upload an MPESA statement to Falu.")
    {
        this.AddArgument<MpesaStatementType>(name: "type",
                                             description: "The type of statement to upload.");

        this.AddOption<string>(new[] { "-f", "--file", },
                               description: "File path for the statement file (upto 128KiB).",
                               configure: o => o.IsRequired = true);
    }
}
