# Copilot Instructions for TdsLib

This repository contains an open implementation of the [TDS protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) (version 7.4) in managed C# code. The library is used for probing and diagnosing TDS connections to SQL Server, and can be used to manually construct and handle any step of a TDS connection or mock TDS clients.

## Project Overview

- **Purpose**: TDS protocol implementation for connection diagnostics, probing, and testing
- **Target**: .NET Standard 2.0 (broad compatibility with .NET Framework and .NET Core/.NET 5+)
- **Scope**: Login steps and generic TDS features (does not support data streams or T-SQL commands)
- **NuGet Package**: [Microsoft.Data.Tools.TdsLib](https://www.nuget.org/packages/Microsoft.Data.Tools.TdsLib)

## Documentation

- [README.md](../README.md) - Project overview, code structure, and usage examples
- [MS-TDS Protocol Specification](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) - Official TDS protocol documentation

## Tech Stack

- **Language**: C# (.NET Standard 2.0)
- **Test Framework**: xUnit with Moq for mocking
- **Build**: .NET SDK (`dotnet build`)
- **CI**: GitHub Actions (Windows and macOS), Azure Pipelines

## Repository Structure

```
src/
  Microsoft.Data.Tools.TdsLib/          # Main library
    Buffer/                             # ByteBuffer for low-level byte operations
    Exceptions/                         # Custom exceptions (e.g., ConnectionClosedException)
    IO/                                 # Connection and stream handling
      Connection/                       # IConnection interface and implementations
        Tcp/                            # TCP connection implementation
    Messages/                           # Logical message format for SQL Server communication
    Packets/                            # TDS packet structures
    Payloads/                           # Message payloads (PreLogin, Login7)
      Login7/                           # Login7 payload and authentication
        Auth/                           # Federated authentication support
      PreLogin/                         # PreLogin payload
    Tokens/                             # TDS tokens received from SQL Server
      Done/                             # Done token family
      EnvChange/                        # Environment change tokens
      Error/                            # Error tokens
      FeatureExtAck/                    # Feature acknowledgment tokens
      FedAuthInfo/                      # Federated auth info tokens
      Info/                             # Info tokens
      LoginAck/                         # Login acknowledgment tokens
      ReturnStatus/                     # Return status tokens

test/
  Microsoft.Data.Tools.TdsLib.UnitTest/         # Unit tests
  Microsoft.Data.Tools.TdsLib.IntegrationTest/  # Integration tests
```

### Architectural Layers

1. **Buffer Layer** (`Buffer/`): Low-level byte manipulation with `ByteBuffer` class
2. **IO Layer** (`IO/`): Connection abstractions via `IConnection` interface
3. **Packet Layer** (`Packets/`): TDS packet structures with headers and data
4. **Message Layer** (`Messages/`): Logical messages composed of packets
5. **Payload Layer** (`Payloads/`): Typed message payloads (PreLogin, Login7)
6. **Token Layer** (`Tokens/`): Response parsing with token parsers

## Coding Guidelines

### Naming Conventions

- **Classes/Types**: PascalCase (`TdsClient`, `ByteBuffer`, `PreLoginPayload`)
- **Interfaces**: PascalCase with `I` prefix (`IConnection`)
- **Methods**: PascalCase (`SendMessage`, `ReceiveTokenAsync`)
- **Properties**: PascalCase (`MessageHandler`, `TokenStreamHandler`)
- **Private fields**: camelCase (`tdsClient`, `incomingMessageBuffer`)
- **Parameters**: camelCase (`serverEndpoint`, `payloadGenerator`)
- **Constants**: PascalCase (`HeaderLength`, `DefaultPacketId`)
- **Enums**: PascalCase for type and members (`PacketType.SqlBatch`, `TokenType.Error`)

### File Organization

- One primary class per file, named after the class
- Related types (e.g., token + parser) in the same folder
- Namespace matches folder structure: `Microsoft.Data.Tools.TdsLib.{Folder}`

### Code Style

- **File Header**: Every file starts with copyright and license notice:
  ```csharp
  // Copyright (c) Microsoft Corporation.
  // Licensed under the MIT License.
  ```
- **Usings**: System namespaces first, then project namespaces, alphabetically sorted
- **Braces**: Allman style (opening brace on new line)
- **Access Modifiers**: Always explicit (`public`, `private`, `internal`)
- **Async Methods**: Suffix with `Async` (e.g., `ReceiveTokenAsync`)

### XML Documentation

All public APIs must have XML documentation comments:

```csharp
/// <summary>
/// Brief description of the method.
/// </summary>
/// <param name="paramName">Description of parameter.</param>
/// <returns>Description of return value.</returns>
/// <exception cref="ExceptionType">When this exception is thrown.</exception>
public async Task<Result> MethodAsync(ParamType paramName)
```

### Async/Await Patterns

- Use `ConfigureAwait(false)` on all awaited calls in library code
- Support `CancellationToken` where appropriate
- Return `Task` or `Task<T>` for async methods

### Error Handling

- Throw `ArgumentNullException` for null arguments: `connection ?? throw new ArgumentNullException(nameof(connection))`
- Use custom exceptions from `Exceptions/` folder (e.g., `ConnectionClosedException`)
- Document exceptions in XML comments with `<exception cref="...">` tags

### Abstract Classes and Inheritance

- **Token**: Abstract base class for all TDS tokens; derived classes override `Type` property
- **TokenParser**: Abstract base class for token parsers; implement `Parse` method
- **Payload**: Abstract base class for payloads; implement `BuildBufferInternal` method
- **EnvChangeToken<T>**: Generic abstract base for environment change tokens

## Testing Requirements

### Test Framework

- **xUnit** for unit and integration tests
- **Moq** for mocking interfaces (e.g., `IConnection`)
- **coverlet** for code coverage

### Test Organization

- Unit tests mirror source structure: `test/Microsoft.Data.Tools.TdsLib.UnitTest/{Folder}/`
- Test class name matches source class with `Test` suffix: `TdsClient` → `TdsClientTest`
- Test file location matches source location

### Test Naming

Use descriptive method names following the pattern `MethodName_Scenario_ExpectedResult` or simple descriptive names:

```csharp
[Fact]
public void CreateTdsClientWithNullConnection()
{
    Assert.Throws<ArgumentNullException>(() =>
    {
        IConnection connection = default;
        new TdsClient(connection);
    });
}
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with verbosity
dotnet test --verbosity normal

# Run specific test project
dotnet test test/Microsoft.Data.Tools.TdsLib.UnitTest
```

## Build and Development

### Prerequisites

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)

### Build Commands

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Run tests
dotnet test
```

### CI/CD

- **GitHub Actions**: Runs on `push` and `pull_request` to `main` branch
  - Builds and tests on Windows and macOS
  - Uses .NET 6.0
- **Azure Pipelines**: Triggers internal release pipeline on `main` branch

## Best Practices

### Always Do

- Add XML documentation for all public APIs
- Use `ConfigureAwait(false)` in async library code
- Include the copyright header in new files
- Write unit tests for new functionality
- Use interfaces for dependencies (e.g., `IConnection`)
- Follow existing patterns when adding new tokens or payloads

### Documentation Updates

When making changes:
- Update README.md if adding new features or changing usage patterns
- Update XML documentation when changing method signatures or behavior
- Keep inline comments concise and only where logic is non-obvious

### Never Do

- Commit secrets, credentials, or connection strings
- Skip tests for changes to core functionality
- Break backward compatibility without discussion
- Use `ConfigureAwait(true)` in library code (default behavior)
- Remove or modify the copyright header

## Adding New Components

### Adding a New Token

1. Create token class in `Tokens/{TokenName}/` inheriting from `Token`
2. Override `Type` property to return appropriate `TokenType`
3. Create parser class inheriting from `TokenParser`
4. Register parser in `TokenStreamHandler` constructor
5. Add unit tests in corresponding test folder

### Adding a New Payload

1. Create payload class in `Payloads/{PayloadName}/` inheriting from `Payload`
2. Implement `BuildBufferInternal()` method
3. Add constructor accepting `ByteBuffer` for parsing received payloads
4. Add unit tests

## Git Commit Messages

Use clear, descriptive commit messages:
- Start with a verb in imperative mood: "Add", "Fix", "Update", "Remove"
- Keep the first line under 72 characters
- Reference issues when applicable

Examples:
- `Add FedAuthInfo token parser`
- `Fix buffer overflow in PreLoginPayload parsing`
- `Update README with new connection example`
