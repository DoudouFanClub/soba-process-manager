/*
 * Startup:
    npm run dev -o
    
    ubuntu + mongod
    redis-cli
    go run .

    python tcp_core.py <args>
    
    docker run -d --gpus=all -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama
*/

/*
 * Shutdown:
    q + enter

    ctrl + c
    shutdown + quit
    ctrl + c

    taskkill <pid>

    docker stop <container_name>
*/

using soba_process_manager.Api;
using soba_process_manager.ConfigData;
using System.Diagnostics;
using System.Text.Json;

string filePath = "AppSetting.json";

if (!File.Exists(filePath))
{
    throw new FileNotFoundException("The configuration file was not found.", filePath);
}

// Load The Config File
string jsonString = File.ReadAllText(filePath);
InfererConfigStorage config = JsonSerializer.Deserialize<InfererConfigStorage>(jsonString);

if (config == null) return;

// Initialize Each Known Process
bool loadStatus = true;
List<Thread> activeProcesses = new List<Thread>();
(loadStatus, activeProcesses) = Initialize.Init(config.LaunchSettings);

// So Printout Appears Below
Thread.Sleep(3000);
Console.WriteLine("\n===================================================\n");

// Wait For User To Terminate
string userTerminateStatus = "";
while (userTerminateStatus != "quit" && loadStatus)
{
    Console.Write("Type 'quit' to terminate - ");
    userTerminateStatus = Console.ReadLine();
    ProcessManager.EndFlag = true;
}
// Cleanup
Terminate.Cleanup(config.TerminateSettings);
Environment.Exit(0);