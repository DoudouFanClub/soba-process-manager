using soba_process_manager.ConfigData;
using System.Diagnostics;

namespace soba_process_manager.Api
{
    internal class ProcessManager
    {
        public static bool EndFlag = false;
        public static Thread CreateThreadedProcess(LaunchInfo info)
        {
            Thread process = new Thread(() => StartProcess(info));
            process.Start();
            Thread.Sleep(info.Wait * 1000);
            return process;
        }

        public static void StartProcess(LaunchInfo info)
        {
            // Use /k if we want to show the terminal
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = info.AutoCloseConsole ? $"/k {info.Launcher} {info.Args}" : $"/c {info.Launcher} {info.Args}",
                UseShellExecute = true,
                CreateNoWindow = info.ShowConsole
            };

            if (info.Persistent) Console.WriteLine("Launched Persistent Process: {0} {1}", info.Launcher, info.Args);
            else Console.WriteLine("Launched Process: {0} {1}", info.Launcher, info.Args);

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");

                // Run this only once when we Terminate
                // (Shutdown Python.exe)
                if (EndFlag) process.Start();
                else
                {
                    // Normal Running Process - Start Once
                    if (!info.Persistent)
                    {
                        process.Start();
                    }
                    // Normal Running Process - Persistent Process
                    while (!EndFlag && info.Persistent)
                    {
                        process.Start();
                        process.WaitForExit();
                    }
                }
            }
        }
    }
}
