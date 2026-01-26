# Copilot Instructions for TdsLib

TdsLib is an open implementation of the [TDS (Tabular Data Stream) protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) (version 7.4) in managed C#. It is designed for probing and diagnosing TDS connections to SQL Server, and can also be used to mock TDS clients. It does not support normal data streams or T-SQL commands.

## Project Overview

- **Purpose**: Low-level TDS protocol library for connection diagnostics and probing
- **Primary use cases**: Connection testing, TDS protocol exploration, TDS client mocking
- **NuGet package**: `Microsoft.Data.Tools.TdsLib`
- **License**: MIT

## Documentation

- [README.md](../README.md) - Project overview and usage examples
- [TDS Protocol Specification (MS-TDS)](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/)
- [SECURITY.md](../SECURITY.md) - Security reporting guidelines
- [SUPPORT.md](../SUPPORT.md) - Support information

## Tech Stack

- **Language**: C# (.NET Standard 2.0 for the library)
- **Target Framework**: .NET Standard 2.0 (library), .NET 6 (tests)
- **Test Framework**: xUnit with Moq for mocking
- **Build System**: .NET SDK / MSBuild
- **CI/CD**: GitHub Actions (`.github/workflows/dotnet.yml`)
- **Package Format**: NuGet

## Repository Structure

```
src/
  Microsoft.Data.Tools.TdsLib/           # Main library code
    Buffer/                              # ByteBuffer for low-level byte operations
    Exceptions/                          # Custom exceptions (e.g., ConnectionClosedException)
    IO/                                  # I/O and connection classes
      Connection/                        # Connection interfaces and options
        Tcp/                             # TCP-specific connection implementation
    Messages/                            # TDS message handling
    Packets/                             # TDS packet structures
    Payloads/                            # Message payloads (PreLogin, Login7, etc.)
      Login7/                            # Login7 payload and authentication
        Auth/                            # Federated authentication classes
      PreLogin/                          # PreLogin payload structures
    Tokens/                              # TDS response tokens from SQL Server
      Done/                              # Done token types
      EnvChange/                         # Environment change tokens
      Error/                             # Error token
      FedAuthInfo/                       # Federated auth info token
      Info/                              # Info token
      LoginAck/                          # Login acknowledgment token
      ...                                # Other token types

test/
  Microsoft.Data.Tools.TdsLib.UnitTest/        # Unit tests
  Microsoft.Data.Tools.TdsLib.IntegrationTest/ # Integration tests (requires SQL Server)
```

### Architectural Layers

| Layer | Location | Purpose |
|-------|----------|---------|
| Client | `TdsClient.cs` | Main entry point, orchestrates connection and handlers |
| Messages | `Messages/` | High-level message send/receive via `MessageHandler` |
| Tokens | `Tokens/` | Token parsing via `TokenStreamHandler` and `TokenParser` |
| Packets | `Packets/` | Low-level packet structure and serialization |
| Payloads | `Payloads/` | Message content (PreLogin, Login7, etc.) |
| IO/Connection | `IO/Connection/` | Transport layer abstraction via `IConnection` |
| Buffer | `Buffer/` | Binary data manipulation via `ByteBuffer` |

## Coding Guidelines

### Naming Conventions

- **Classes/Types**: PascalCase (e.g., `TdsClient`, `ErrorToken`, `ByteBuffer`)
- **Interfaces**: PascalCase with `I` prefix (e.g., `IConnection`)
- **Methods**: PascalCase (e.g., `SendMessage`, `ReceiveTokenAsync`)
- **Properties**: PascalCase (e.g., `MessageHandler`, `TokenType`)
- **Private fields**: camelCase (e.g., `tdsClient`, `incomingMessageBuffer`)
- **Parameters**: camelCase (e.g., `tokenStreamHandler`, `serverEndpoint`)
- **Constants**: PascalCase (e.g., `HeaderLength`, `FeatureExtensionTerminator`)
- **Enums**: PascalCase for type and values (e.g., `TokenType.Error`, `PacketType.Login7`)

### File Naming

- One public type per file, named after the type (e.g., `ErrorToken.cs`)
- Token parsers: `{TokenName}TokenParser.cs` (e.g., `ErrorTokenParser.cs`)
- Tests: `{ClassName}Test.cs` (e.g., `ByteBufferTest.cs`, `TdsClientTest.cs`)
- Test location mirrors source location (e.g., `test/.../Buffer/ByteBufferTest.cs`)

### Code Style

- **File header**: All source files must start with the Microsoft copyright and MIT license header:
  ```csharp
  // Copyright (c) Microsoft Corporation.
  // Licensed under the MIT License.
  ```
- **Namespace per folder**: Namespaces follow folder structure (e.g., `Microsoft.Data.Tools.TdsLib.Tokens.Error`)
- **XML documentation**: All public APIs must have XML doc comments with `<summary>`, `<param>`, `<returns>`, and `<exception>` tags
- **Braces**: Allman style (opening brace on new line)
- **Using statements**: Group by System, then Microsoft, then others; sorted alphabetically

### Async Patterns

- Async methods end with `Async` suffix (e.g., `ReceiveTokenAsync`, `CopyToAsync`)
- Always use `.ConfigureAwait(false)` when awaiting tasks in library code
- Use `CancellationToken` parameter for cancellable operations
- Return `Task` or `Task<T>` for async methods

### Error Handling

- Use custom exceptions in `Exceptions/` namespace (e.g., `ConnectionClosedException`)
- Validate constructor parameters with `ArgumentNullException`:
  ```csharp
  Connection = connection ?? throw new ArgumentNullException(nameof(connection));
  ```
- Document exceptions in XML comments with `<exception>` tags
- Use `InvalidOperationException` for unsupported operations

### Implementing New Tokens

To add a new TDS token type:

1. Add the token type to `Tokens/TokenType.cs` enum
2. Create `Tokens/{TokenName}/{TokenName}Token.cs` inheriting from `Token`
3. Create `Tokens/{TokenName}/{TokenName}TokenParser.cs` inheriting from `TokenParser`
4. Register the parser in `TokenStreamHandler` constructor's `parsers` dictionary

### Implementing New Payloads

To add a new message payload:

1. Create class in `Payloads/` inheriting from `Payload`
2. Override `BuildBufferInternal()` to serialize the payload
3. Use `ByteBuffer` for binary serialization with explicit endianness (LE/BE)

## Testing Requirements

### Test Framework

- **Framework**: xUnit with `[Fact]` and `[Theory]` attributes
- **Mocking**: Moq library for creating test doubles
- **Assertions**: xUnit assertions (`Assert.Equal`, `Assert.Throws`, `Assert.All`, etc.)

### Test Organization

- Unit tests in `test/Microsoft.Data.Tools.TdsLib.UnitTest/`
- Integration tests in `test/Microsoft.Data.Tools.TdsLib.IntegrationTest/`
- Mirror source folder structure in test projects

### Test Naming

- Test methods: Describe the scenario being tested (e.g., `CreateTdsClientWithNullConnection`, `TestPositiveInt32LE`)
- Test classes: `{ClassName}Test` (e.g., `ByteBufferTest`)

### Running Tests

```bash
# Restore, build, and test
dotnet restore
dotnet build
dotnet test

# Run specific test project
dotnet test test/Microsoft.Data.Tools.TdsLib.UnitTest/

# Run with verbose output
dotnet test --verbosity normal
```

## Build and Development

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later

### Build Commands

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Build specific configuration
dotnet build --configuration Release

# Run all tests
dotnet test

# Create NuGet package (uses tdslib.nuspec)
dotnet pack
```

### CI Pipeline

The GitHub Actions workflow (`.github/workflows/dotnet.yml`) runs on:
- Push to `main` branch
- Pull requests targeting `main`

It builds and tests on both Windows and macOS.

## Best Practices

### Always Do

- Add XML documentation for all public APIs
- Use `ConfigureAwait(false)` in async library code
- Validate constructor parameters for null
- Follow existing patterns for new tokens/payloads
- Run tests before submitting changes
- Use explicit endianness methods (e.g., `ReadInt32LE`, `WriteUInt16BE`)

### Documentation Updates

Update relevant documentation when making changes:

- Update README.md when adding new features or changing usage patterns
- Update XML doc comments when changing public API signatures
- Add `<exception>` tags when methods can throw new exceptions

### Never Do

- Commit secrets, connection strings, or credentials
- Skip XML documentation for public types/members
- Use implicit endianness conversions (always specify LE/BE)
- Break backward compatibility without discussion
- Add dependencies without justification (library targets .NET Standard 2.0 with zero dependencies)

## Git Commit Messages

Use conventional commit format:

- `feat:` New features (e.g., `feat: Add support for SSPI authentication`)
- `fix:` Bug fixes (e.g., `fix: Correct packet length calculation`)
- `docs:` Documentation changes
- `test:` Test additions or modifications
- `refactor:` Code refactoring without behavior changes
- `chore:` Build, CI, or tooling changes
