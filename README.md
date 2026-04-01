# Modbus TCP Automation Client

This application is a continuous Modbus TCP client designed to automate sequential "write" commands to a target Modbus device.

## Configuration

The automation is controlled entirely by an `appsettings.json` file located in the same directory as the executable. If the file is missing when you launch the application, a default one will be automatically created for you.

### Example `appsettings.json`

```json
{
  "DeviceIp": "127.0.0.1",
  "DevicePort": 502,
  "SettlingDelayMs": 1000,
  "SlaveId": 1,
  "Commands": [
    {
      "CoilAddress": 0,
      "Value": true
    },
    {
      "CoilAddress": 1,
      "Value": false
    },
    {
      "CoilAddress": 5,
      "Value": true
    },
    {
      "CoilAddress": 2,
      "Value": true
    }
  ]
}
```

- **DeviceIp**: Target device IP address.
- **DevicePort**: Target device Modbus TCP port (default is 502).
- **SettlingDelayMs**: The generic delay duration (in milliseconds) the application waits between sequential coil write events to allow the connected device space to settle.
- **SlaveId**: Standard Modbus Slave/Unit Identifier.
- **Commands**: An array of commands to execute in precise sequence. Each command defines which zero-indexed `CoilAddress` to write to and the concrete boolean `Value` (`true` or `false`) to send.

## Project Structure

The project is organized into the following structure:

- **ImplementationFolder/**: Contains the core application logic, subcomponents, and the main project file (`ModbusTcpClientAutomation.csproj`).
  - **Core/**: Domain models and business logic.
  - **Application/**: Service layers and application flow.
  - **Infrastructure/**: Modbus client implementations and external integrations.
- **testFolder/**: Contains all test projects and test files.
- **main.sln**: The main solution file for the entire project.

## Usage

You can launch the application directly from PowerShell. If you are running from the source code during development, you can use the following command from the root directory:

```powershell
dotnet run --project ImplementationFolder/ModbusTcpClientAutomation.csproj
```

Once launched, the application will:
1. Load your configuration dynamically from `ImplementationFolder/appsettings.json`.
2. Connect cleanly to the target endpoint.
3. Automatically execute the prescribed commands, predictably applying the `SettlingDelayMs` delay after each command.
4. Smoothly disconnect upon completion.

## CLI Output and Logs

During execution, the application outputs its live progress directly to the standard command line interface (stdout). You will see exactly what it is doing, including execution events, command transaction timestamps, and exact device response successes or failures right in your PowerShell window.

Additionally, every single action printed to the screen is permanently recorded and appended into a text file named **`modbus_actions.log`**. This master file resides right next to the app enabling simple post-action verification tracing without needing to keep the terminal open.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
See [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) for details on the open source software used in this project.
