# TdsLib

This repository contains an open implementation of the [TDS protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tds/) (version 7.4) in managed C# code. It is not a full implementation, current support is focused on the login steps and some generic TDS features. It does not support _normal_ data streams or T-SQL commands.

This library is mainly used for probing and for diagnosing TDS connections but can be used to manually construct and handle any step of a TDS connection. It can also be used to mock TDS clients.

## Requirements

- [dotnet](https://dotnet.microsoft.com/en-us/)
- [net standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0)

## Code structure

```text
src
  Microsoft.TdsLib - TdsLib code
    Buffer - Buffer class to handle type conversion
    IO - Connection related classes
    Messages - Logical message format used to generate Packets to be sent to SQL Server
    Packets - Packet related classes
    Payloads - Message Payloads
    Tokens - Tokens that can be received from SQL server

test
  Microsoft.TdsLib.UnitTest - Unit tests
  Microsoft.TdsLib.IntegrationTest - Integration tests
```

## Example

### Establish a TDS connection and login to a SQL server

```csharp
using Microsoft.TdsLib;
using Microsoft.TdsLib.IO.Connection.Tcp;
using Microsoft.TdsLib.Messages;
using Microsoft.TdsLib.Packets;
using Microsoft.TdsLib.Payloads.Login7;
using Microsoft.TdsLib.Payloads.PreLogin;
using Microsoft.TdsLib.Tokens.Error;

string hostname = "sqlserver.contoso.net";
int port = 1433;

using TdsClient client = new TdsClient(new ServerEndpoint(hostname, port));

// Send PreLogin message
await client.MessageHandler.SendMessage(new Message(PacketType.PreLogin)
{
    Payload = new PreLoginPayload(encrypt: true)
});

// Receive PreLogin message
var preLoginResponseMessage = await client.MessageHandler.ReceiveMessage(b => new PreLoginPayload(b));

// Perform TLS handshake
await client.PerformTlsHandshake();

// Prepare Login7 message
Login7Payload login7Payload = new Login7Payload()
{
    Hostname = hostname,
    ServerName = "MyServerName",
    AppName = "MyAppName",
    Language = "us_english",
    Database = "MyDatabaseName",
    //Username,
    //Password
};

login7Payload.TypeFlags.AccessIntent = OptionAccessIntent.ReadWrite;

Message login7Message = new Message(PacketType.Login7) { Payload = login7Payload };

// Send Login7 message
await client.MessageHandler.SendMessage(login7Message);

// Receive Login response tokens
await client.TokenStreamHandler.ReceiveTokensAsync(tokenEvent =>
{
    if (tokenEvent.Token is ErrorToken errorToken)
    {
        // Error was received from the server
    }
});

// If no error token was received, and SQL server did not close the connection, then the connection to the server is now established and the user is logged in.
```

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit <https://cla.opensource.microsoft.com>.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
