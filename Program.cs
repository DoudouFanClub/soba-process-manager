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

// Initialize Each Known Process
//bool loadStatus = true;
//List<Thread> activeProcesses = new List<Thread>();
var processManager = new ProcessManager();

processManager.Init();
processManager.Start();

// So Printout Appears Below
Thread.Sleep(3000);
Console.WriteLine("\n===================================================\n");

// Wait For User To Terminate
string userTerminateStatus = "";

while (userTerminateStatus != "quit")
{
    Console.Write("Type 'quit' to terminate - ");
    userTerminateStatus = Console.ReadLine();
}

// Cleanup
processManager.End();
Environment.Exit(0);