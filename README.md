# Falu SDK for .NET

[![Release](https://img.shields.io/github/release/tinglesoftware/falu-cli.svg?style=flat-square)](https://github.com/tinglesoftware/falu-cli/releases/latest)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/tinglesoftware/falu-cli/Build%20and%20Publish?style=flat-square)

The official CLI tool for [Falu][falu] to help you build, test and managed your Falu integration right from the terminal.

## Usage

Installing the CLI provides access to the `falu` command.

```bash
falu [command]

# Run `--help` for detailed information about CLI commands
falu [command] help
```

### Commands

The Falu CLI supports a broad range of commands including:

- [`logs tail`][wiki-command-logs-tail]
- [`events resend`](/wiki/commands/events-resend)
- [`listen`](/wiki/commands/listen)

Check out the [wiki](/wiki) for more on using the CLI.

## Installation

Falu CLI is available for macOS, Windows and Linux (Ubuntu). You can download each of the binaries in the [releases](/releases) or you can use package managers in the respective platforms.

### Windows

Falu CLI is available on Windows via [Chocolatey][chocolatey] package manager:

```bash
choco install falu
```

## Issues & Comments

Feel free to contact us if you encounter any issues with the library.
Please leave all comments, bugs, requests and issues on the Issues page.

## Development

For any requests, bug or comments, please [open an issue][issues] or [submit a pull request][pulls].

[chocolatey]: https://chocolatey.org/
[issues]: https://github.com/tingle/falu-dotnet/issues/new
[pulls]: https://github.com/tingle/falu-dotnet/pulls
[falu]: https://falu.io
[wiki-command-logs-tail]: https://github.com/tingle/falu-dotnet/wiki/commands/logs-tail
[wiki-command-events-resend]: https://github.com/tingle/falu-dotnet/wiki/commands/events-resend
[wiki-command-listen]: https://github.com/tingle/falu-dotnet/wiki/commands/listen

### License

The Library is licensed under the [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form") license. Refer to the [LICENSE](./LICENSE) file for more information.
