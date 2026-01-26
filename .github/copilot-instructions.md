# Copilot Instructions for TdsLib

This repository contains an open implementation of the [TDS protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) (version 7.4) in managed C# code. It is focused on the login steps and TDS connection diagnostics.

## Project Overview

- **Purpose**: A .NET library for probing and diagnosing TDS (Tabular Data Stream) connections to SQL Server
- **Use cases**: Connection diagnostics, TDS probing, mocking TDS clients
- **Limitations**: Does not support normal data streams or T-SQL commands

## Documentation

- [README.md](../README.md) - Main documentation with usage examples
- [TDS Protocol Specification](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) - Microsoft TDS protocol documentation

## Tech Stack

- **Language**: C# (.NET Standard 2.0)
- **Test Framework**: xUnit with Moq for mocking
- **Target Framework**: netstandard2.0 (library), net6.0 (tests)
- **Build Tool**: dotnet CLI / MSBuild

## Repository Structure

```
src/
  Microsoft.Data.Tools.TdsLib/          # Main library code
    Buffer/                             # ByteBuffer for low-level byte operations
    Exceptions/                         # Custom exceptions (ConnectionClosedException)
    IO/                                 # Connection and stream handling
      Connection/                       # IConnection interface and implementations
        Tcp/                            # TCP-specific connection classes
    Messages/                           # Message format and handling
    Packets/                            # TDS packet classes
    Payloads/                           # Message payloads (PreLogin, Login7, etc.)
      Login7/                           # Login7 payload and authentication
        Auth/                           # Fed Auth implementations
      PreLogin/                         # PreLogin payload options
    Tokens/                             # TDS response tokens
      Done/                             # Done token types
      EnvChange/                        # Environment change tokens
      Error/                            # Error tokens
      FedAuthInfo/                      # Federation auth info tokens
      FeatureExtAck/                    # Feature extension ack tokens
      Info/                             # Info tokens
      LoginAck/                         # Login acknowledgment tokens
      ReturnStatus/                     # Return status tokens

test/
  Microsoft.Data.Tools.TdsLib.UnitTest/         # Unit tests
  Microsoft.Data.Tools.TdsLib.IntegrationTest/  # Integration tests
```

### Architectural Layers

| Layer | Location | Purpose |
|-------|----------|---------|
| Client | `TdsClient.cs` | Main entry point, orchestrates message/token handling |
| Messages | `Messages/` | High-level message abstraction, splits into packets |
| Packets | `Packets/` | TDS packet format with headers and data |
| Payloads | `Payloads/` | Message content (PreLogin, Login7, etc.) |
| Tokens | `Tokens/` | Response parsing from SQL Server |
| Connection | `IO/Connection/` | Low-level connection management |
| Buffer | `Buffer/` | Byte manipulation utilities |

## Coding Guidelines

### Naming Conventions

- **Classes/Types**: PascalCase (e.g., `TdsClient`, `PreLoginPayload`)
- **Methods**: PascalCase (e.g., `SendMessage`, `ReceiveTokensAsync`)
- **Properties**: PascalCase (e.g., `MessageHandler`, `TokenStreamHandler`)
- **Private fields**: camelCase with no prefix (e.g., `buffer`, `connection`)
- **Parameters**: camelCase (e.g., `tokenStreamHandler`, `packetSize`)
- **Constants**: PascalCase (e.g., `DefaultPacketId`, `HeaderLength`)
- **Enums**: PascalCase for both type and members (e.g., `PacketType.SqlBatch`)

### File Organization

- One public class per file, named after the class
- Internal classes may be in the same file if closely related
- Token parsers are `internal` and follow pattern: `{TokenName}TokenParser.cs`
- Tests mirror source structure in test project

### Code Style

- Use XML documentation comments for all public APIs with `<summary>`, `<param>`, and `<returns>` tags
- Use file-scoped license headers:
  ```csharp
  // Copyright (c) Microsoft Corporation.
  // Licensed under the MIT License.
  ```
- Use `ConfigureAwait(false)` on all awaited calls in library code
- Prefer expression-bodied members for simple properties:
  ```csharp
  public override TokenType Type => TokenType.Done;
  ```

### Async Patterns

Always use `ConfigureAwait(false)` to avoid deadlocks:

```csharp
await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);
```

### Error Handling

- Throw `ArgumentNullException` for null arguments with `nameof()`:
  ```csharp
  Connection = connection ?? throw new ArgumentNullException(nameof(connection));
  ```
- Throw `ArgumentException` for invalid argument values
- Use `InvalidOperationException` for unsupported operations
- Custom exceptions inherit from appropriate base (e.g., `ConnectionClosedException : IOException`)

### Implementing New Tokens

1. Create token class inheriting from `Token` in `Tokens/{TokenName}/`
2. Override `Type` property returning appropriate `TokenType`
3. Create internal `{TokenName}TokenParser : TokenParser`
4. Register parser in `TokenStreamHandler`

### Implementing New Payloads

1. Create payload class inheriting from `Payload` in `Payloads/{PayloadName}/`
2. Override `BuildBufferInternal()` to construct the `Buffer` property
3. Use `ByteBuffer` for all binary data manipulation

## Testing Requirements

### Framework and Structure

- **Framework**: xUnit
- **Mocking**: Moq
- **Test location**: `test/Microsoft.Data.Tools.TdsLib.UnitTest/`
- **Naming**: `{ClassName}Test.cs` (e.g., `PacketTest.cs`, `TdsClientTest.cs`)

### Test Patterns

- Use `[Fact]` for single test cases
- Use descriptive method names: `CreatePacketWithNullBuffer`, `TestStatusResetConnectionIgnore`
- Test both success and failure cases
- Use `Assert.Throws<T>()` for exception testing:
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
dotnet test                                    # Run all tests
dotnet test --filter "FullyQualifiedName~UnitTest"  # Run unit tests only
```

## Build and Development

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) or later
- Visual Studio 2022 or VS Code with C# extension

### Build Commands

```bash
dotnet restore   # Restore NuGet packages
dotnet build     # Build solution
dotnet test      # Run tests
dotnet pack      # Create NuGet package (uses tdslib.nuspec)
```

### Solution Structure

Open `TdsLib.sln` in Visual Studio or use `dotnet` CLI.

## Best Practices

### Always Do

- Add XML documentation for all public members
- Use `ConfigureAwait(false)` on async calls
- Implement `IDisposable` for classes managing resources
- Write unit tests for new functionality
- Follow existing patterns in the codebase
- Use `ByteBuffer` for all binary data manipulation

### Never Do

- Block on async code (no `.Result` or `.Wait()`)
- Forget `ConfigureAwait(false)` in library code
- Store credentials or connection strings in code
- Skip null checks for public API parameters
- Break backward compatibility without discussion

## Git Commit Messages

Use clear, descriptive commit messages:

```
Add support for new token type XYZ

- Implement XyzToken and XyzTokenParser
- Register parser in TokenStreamHandler
- Add unit tests for token parsing
```
