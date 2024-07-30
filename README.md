# soba-process-manager

A robust C# process manager that launches and manages the lifetime of attached applications. This manager ensures its child processes availability by automatically restarting applications that crash or terminate unexpectedly. When the process manager shuts down, it performs thorough cleanup and termination of all attached processes. Additionally, this manager supports dynamic configuration changes, allowing you to restart existing attached applications when configuration files are updated. This enables seamless adaptation to changing requirements without manual intervention.

## Key features

- Launch and manage attached applications
- Automatic restart of crashed or terminated applications
- Thorough cleanup and termination on process manager shutdown
- Dynamic configuration change support with automatic application restart

## How to use
### Create a configuration file for LaunchSettings and TerminateSettings

On Process Manager launch, `LaunchSettings` are automatically called and run. To call the termination commands within `TerminateSettings`, simply type `quit` in the Console.
```json
// Settings.json
{
  "LaunchSettings": [
    {
      "launcher": "docker",
      "args": "start ollama_1",
      "persistent": false,
      "show_console": false,
      "auto_close_console": false,
      "wait": 0
    },
    {
      "launcher": "python",
      "args": "main.py 127.0.0.1 7060 11434",
      "persistent": false,
      "show_console": false,
      "auto_close_console": false,
      "wait": 0
    }
  ],
  "TerminateSettings": [
    {
      "launcher": "docker",
      "args": "stop ollama_1",
      "persistent": false,
      "show_console": false,
      "auto_close_console": false,
      "wait": 0
    },
    {
      "launcher": "taskkill",
      "args": "/IM python.exe /F",
      "persistent": false,
      "show_console": true,
      "auto_close_console": false,
      "wait": 0
    }
}
```

### Available Commands

| Command | Description |
|----------|----------|
| restart | Terminates all existing processes based on `TerminateSettings` from the `Settings.json` file. Re-reads the `Settings.json` file and invokes `LaunchSettings` once again. |
| quit | Terminates all existing processes based on `TerminateSettings` and exits the process manager. |