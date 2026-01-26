# Copilot Instructions for TdsLib

This repository contains an open implementation of the [TDS protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) (version 7.4) in managed C# code. It is primarily used for probing and diagnosing TDS connections to SQL Server.

## Project Overview

- **Purpose**: Low-level TDS protocol implementation for connection diagnostics and testing
- **Target users**: Developers needing to probe, diagnose, or mock TDS connections
- **Scope**: Login steps and generic TDS features (does not support normal data streams or T-SQL commands)

## Documentation

- [README.md](../README.md) - Overview, code structure, and usage examples
- [TDS Protocol Specification (MS-TDS)](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/)
- [NuGet Package](https://www.nuget.org/packages/Microsoft.Data.Tools.TdsLib)

## Tech Stack

- **Language**: C# (.NET Standard 2.0 for the library, .NET 6 for tests)
- **Framework**: .NET Standard 2.0 (maximum compatibility)
- **Build system**: MSBuild / `dotnet` CLI
- **Testing**: xUnit with Moq for mocking
- **Code coverage**: Coverlet
- **Package format**: NuGet

## Repository Structure

```
TdsLib/
├── src/
│   └── Microsoft.Data.Tools.TdsLib/     # Main library source
│       ├── Buffer/                       # ByteBuffer for low-level byte operations
│       ├── Exceptions/                   # Custom exceptions (ConnectionClosedException)
│       ├── IO/                           # Connection and stream handling
│       │   └── Connection/               # IConnection interface and implementations
│       │       └── Tcp/                  # TCP connection implementation
│       ├── Messages/                     # TDS message abstraction (Message, MessageHandler)
│       ├── Packets/                      # TDS packet handling (header + data)
│       ├── Payloads/                     # Message payload types
│       │   ├── Login7/                   # Login7 authentication payload
│       │   └── PreLogin/                 # PreLogin handshake payload
│       └── Tokens/                       # Response tokens from SQL Server
│           ├── Done/                     # Done token
│           ├── Error/                    # Error token
│           ├── Info/                     # Info token
│           └── ...                       # Other token types
├── test/
│   ├── Microsoft.Data.Tools.TdsLib.UnitTest/         # Unit tests
│   └── Microsoft.Data.Tools.TdsLib.IntegrationTest/  # Integration tests
├── .github/workflows/                    # GitHub Actions CI
└── .pipelines/                           # Azure Pipelines (internal release trigger)
```

### Architectural Layers

1. **Buffer Layer** (`Buffer/`): Low-level byte manipulation with `ByteBuffer` class
2. **Packet Layer** (`Packets/`): TDS packet structure (8-byte header + data)
3. **Message Layer** (`Messages/`): Logical messages composed of multiple packets
4. **Payload Layer** (`Payloads/`): Strongly-typed payload serialization (PreLogin, Login7)
5. **Token Layer** (`Tokens/`): Response parsing from SQL Server
6. **Connection Layer** (`IO/`): Transport abstraction via `IConnection` interface

## Coding Guidelines

### Naming Conventions

- **Classes/Types**: PascalCase (e.g., `TdsClient`, `ByteBuffer`, `PreLoginPayload`)
- **Interfaces**: PascalCase with `I` prefix (e.g., `IConnection`)
- **Methods**: PascalCase (e.g., `SendMessage`, `ReceiveData`)
- **Properties**: PascalCase (e.g., `PacketType`, `MessageHandler`)
- **Private fields**: camelCase with no prefix (e.g., `buffer`, `connection`)
- **Constants**: PascalCase (e.g., `HeaderLength`, `DefaultPacketSize`)
- **Files**: Match the primary type name (e.g., `TdsClient.cs`, `ByteBuffer.cs`)

### Code Style

- Use XML documentation comments (`///`) for all public APIs
- Include copyright header at the top of each file:
  ```csharp
  // Copyright (c) Microsoft Corporation.
  // Licensed under the MIT License.
  ```
- Use expression-bodied members for simple getters
- Use `nameof()` for exception argument names
- Prefer `ConfigureAwait(false)` for async library code

### Error Handling

- Throw `ArgumentNullException` for null parameters with `nameof()`:
  ```csharp
  Connection = connection ?? throw new ArgumentNullException(nameof(connection));
  ```
- Throw `ArgumentException` for invalid parameter values
- Use custom exceptions for domain-specific errors (e.g., `ConnectionClosedException`)
- Document exceptions with `<exception>` XML tags

### Async Patterns

- All I/O operations should be async (`Task`/`Task<T>`)
- Use `ConfigureAwait(false)` to avoid capturing synchronization context
- Follow naming convention: async methods end with `Async` suffix when returning Task

## Testing Requirements

### Framework

- **xUnit** for test framework
- **Moq** for mocking
- **Coverlet** for code coverage

### Test Organization

- Unit tests: `test/Microsoft.Data.Tools.TdsLib.UnitTest/`
- Integration tests: `test/Microsoft.Data.Tools.TdsLib.IntegrationTest/`
- Test files mirror source structure (e.g., `Buffer/ByteBufferTest.cs` tests `Buffer/ByteBuffer.cs`)

### Test Naming

- Test classes: `{ClassName}Test` (e.g., `PacketTest`, `TdsClientTest`)
- Test methods: Descriptive names indicating scenario (e.g., `CreatePacketWithNullBuffer`, `SetIgnoreTrue`)
- Use `[Fact]` for single test cases, `[Theory]` for parameterized tests

### Running Tests

```bash
# Run all tests
dotnet test

# Run with verbose output and results
dotnet test --verbosity normal --logger trx
```

## Best Practices

### Always Do

- Add XML documentation for all public types and members
- Implement `IDisposable` for classes managing unmanaged resources
- Use interfaces for dependency injection (e.g., `IConnection`)
- Validate constructor parameters and throw appropriate exceptions
- Override `ToString()` for debugging-friendly output
- Use `#region` sparingly and only for logical groupings (e.g., Constants)

### Documentation Updates

- Update README.md when adding new features or changing the public API
- Keep XML documentation in sync with implementation
- Document exceptions that methods can throw

### Never Do

- Commit secrets, connection strings, or credentials
- Skip null checks on public API parameters
- Use blocking calls (`.Result`, `.Wait()`) in async code paths
- Break backward compatibility of the public API without discussion

## Build and Development

### Prerequisites

- .NET 6 SDK or later
- .NET Standard 2.0 support

### Building

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Build in Release mode
dotnet build -c Release
```

### Running Tests

```bash
# Run unit tests only
dotnet test test/Microsoft.Data.Tools.TdsLib.UnitTest

# Run all tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Creating NuGet Package

The NuGet package is defined in `tdslib.nuspec`. Build in Release mode first, then pack:

```bash
dotnet build -c Release
nuget pack tdslib.nuspec -Properties configuration=Release;version=1.0.0
```

## Git Commit Messages

Use clear, descriptive commit messages:

- Use imperative mood: "Add feature" not "Added feature"
- Keep subject line under 72 characters
- Reference issues when applicable

Examples:
- `Add support for FedAuth tokens`
- `Fix packet length calculation for large payloads`
- `Update xUnit to 2.4.2`
