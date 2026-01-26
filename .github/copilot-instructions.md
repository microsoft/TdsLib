# Copilot Instructions for TdsLib

TdsLib is an open-source implementation of the TDS protocol (version 7.4) in managed C#. It provides tools for probing and diagnosing TDS connections to SQL Server, and can be used to manually construct and handle TDS connection steps or mock TDS clients.

## Project Overview

- **Purpose**: Implement TDS protocol for connection diagnostics and probing
- **Scope**: Login steps and generic TDS features (not full data streams or T-SQL)
- **Use Cases**: Connection probing, diagnostics, TDS client mocking

## Documentation

- [README.md](../README.md) - Project overview and usage examples
- [TDS Protocol Specification](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) - Microsoft TDS protocol reference

## Tech Stack

- **Language**: C# (.NET Standard 2.0 for library, .NET 6 for tests)
- **Testing**: xUnit with Moq for mocking
- **CI/CD**: GitHub Actions (Windows and macOS), Azure Pipelines
- **Package**: NuGet (`Microsoft.Data.Tools.TdsLib`)

## Repository Structure

```
src/
  Microsoft.Data.Tools.TdsLib/       # Main library
    Buffer/                          # ByteBuffer for type conversion
    Exceptions/                      # Custom exceptions
    IO/                              # Connection abstractions
      Connection/                    # IConnection and implementations
        Tcp/                         # TCP connection classes
    Messages/                        # TDS message handling
    Packets/                         # TDS packet structures
    Payloads/                        # Message payloads (PreLogin, Login7)
    Tokens/                          # Response tokens from SQL Server

test/
  Microsoft.Data.Tools.TdsLib.UnitTest/         # Unit tests
  Microsoft.Data.Tools.TdsLib.IntegrationTest/  # Integration tests
```

### Architectural Layers

1. **Connection Layer** (`IO/Connection/`): Abstracts transport (TCP, TLS). Implement `IConnection` for custom transports.
2. **Packet Layer** (`Packets/`): Low-level TDS packet structure and parsing.
3. **Message Layer** (`Messages/`): Logical messages composed of packets.
4. **Payload Layer** (`Payloads/`): Strongly-typed payload builders (PreLogin, Login7).
5. **Token Layer** (`Tokens/`): Parsers for server response tokens.

## Coding Guidelines

### Naming Conventions

- **Classes/Interfaces**: PascalCase (e.g., `TdsClient`, `IConnection`)
- **Methods**: PascalCase (e.g., `SendMessage`, `PerformTlsHandshake`)
- **Properties**: PascalCase (e.g., `MessageHandler`, `TokenStreamHandler`)
- **Parameters/locals**: camelCase (e.g., `serverEndpoint`, `packetSize`)
- **Private fields**: camelCase (no underscore prefix)
- **Constants**: PascalCase

### Code Style

- Place copyright header at the top of every source file:
  ```csharp
  // Copyright (c) Microsoft Corporation.
  // Licensed under the MIT License.
  ```
- Use XML documentation comments on all public members
- Organize usings: System namespaces first, then project namespaces
- Use `nameof()` for exception argument names
- Use expression-bodied members for simple properties: `public override TokenType Type => TokenType.Error;`

### Interfaces and Abstractions

- Define interfaces for all connection types (see `IConnection`)
- Use abstract base classes for token types (see `Token`)
- Payload classes must inherit from `Payload` and implement `BuildBuffer()`

### Async Patterns

- Use `async`/`await` for all I/O operations
- Append `Async` suffix to async methods
- Use `ConfigureAwait(false)` in library code:
  ```csharp
  await Connection.StartTLS().ConfigureAwait(false);
  ```

### Error Handling

- Create custom exceptions in `Exceptions/` folder inheriting from appropriate base
- Validate arguments with `ArgumentNullException`:
  ```csharp
  Connection = connection ?? throw new ArgumentNullException(nameof(connection));
  ```
- Document exceptions in XML comments with `<exception>` tags

### Buffer Operations

- Use `ByteBuffer` for all binary data manipulation
- Use explicit endianness methods: `WriteInt32LE`, `WriteInt32BE`, `ReadUInt16BE`
- Reset buffer position before reading after writing

## Testing Requirements

### Test Framework

- **Unit Tests**: xUnit with `[Fact]` for single cases, `[Theory]` for parameterized
- **Mocking**: Moq for interface mocking
- **Coverage**: coverlet.collector

### Test Organization

- Mirror source folder structure in test projects
- Name test classes: `{ClassName}Test.cs`
- Name test methods: `{MethodName}_{Scenario}` or descriptive name

### Test Patterns

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
dotnet test                           # Run all tests
dotnet test --filter "FullyQualifiedName~UnitTest"  # Unit tests only
```

## Build and Development

### Prerequisites

- .NET 6 SDK or later
- .NET Standard 2.0 compatible runtime

### Build Commands

```bash
dotnet restore    # Restore dependencies
dotnet build      # Build solution
dotnet test       # Run tests
dotnet pack       # Create NuGet package
```

### CI Pipeline

The GitHub Actions workflow runs on both Windows and macOS, executing:
1. `dotnet restore`
2. `dotnet build --no-restore`
3. `dotnet test --no-build`

## Best Practices

### Always Do

- Add XML documentation to all public APIs
- Implement `IDisposable` for classes that hold unmanaged resources
- Use `sealed` for classes not designed for inheritance
- Write unit tests for new functionality
- Validate all public method parameters
- Use `using` statements or `IDisposable` pattern for connections

### Never Do

- Commit secrets or connection strings
- Block on async operations (no `.Result` or `.Wait()`)
- Ignore return values from async methods
- Skip `ConfigureAwait(false)` in library code
- Break backward compatibility of public APIs

## Git Commit Messages

Use conventional commit format:
- `feat: Add new token parser for ENVCHANGE`
- `fix: Handle connection timeout in TcpConnection`
- `test: Add unit tests for ByteBuffer slicing`
- `docs: Update README with new usage examples`
