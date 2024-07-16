using soba_process_manager.ConfigData;
using System.Diagnostics;

namespace soba_process_manager.Api
{
    public class Initialize
    {
        public static bool EndFlag = false;
        public static (bool, List<Thread>) Init(List<LaunchInfo> initSettings)
        {
            bool loadStatus = true;
            List<Thread> activeProcesses = new List<Thread>();

            foreach (LaunchInfo launchSettings in initSettings)
            {
                if (!loadStatus) break;

                string launcher = launchSettings.Launcher;
                string arg = launchSettings.Args;
                int wait = launchSettings.Wait;

                switch (launcher)
                {
                    case "python":
                    {
                        activeProcesses.Add( CreatePublicProcess(launcher, arg, wait) );
                        break;
                    }
                    case "docker":
                    {
                        activeProcesses.Add( CreatePrivateProcess(launcher, arg, wait ));
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Unknown launcher specified: ", launcher);
                        loadStatus = false;
                        break;
                    }
                }
            }

            return (loadStatus, activeProcesses);
        }

        public static Thread CreatePublicProcess(string launcher, string args, int wait)
        {
            Thread process = new Thread(() => StartPublicProcess(launcher, args, wait));
            process.Start();
            Thread.Sleep(wait * 1000);
            return process;
        }

        public static Thread CreatePrivateProcess(string launcher, string args, int wait)
        {
            Thread process = new Thread(() => StartPrivateProcess(launcher, args, wait));
            process.Start();
            Thread.Sleep(wait * 1000);
            return process;
        }

        public static void StartPublicProcess(string launcher, string args, int wait)
        {
            // Use /k if we want to show the terminal
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k {launcher} {args}",
                UseShellExecute = true,
                CreateNoWindow = false // Set to true if you don't want to show the command prompt window
            };

            Console.WriteLine(launcher);
            Console.WriteLine(args);

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");

                // Run this only once when we Terminate
                // (Shutdown Python.exe)
                if (EndFlag) process.Start();
                while (!EndFlag)
                {
                    process.Start();
                    process.WaitForExit();
                }
            }
        }

        public static void StartPrivateProcess(string launcher, string args, int wait)
        {
            // Use /k if we want to show the terminal
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {launcher} {args}",
                UseShellExecute = true,
                CreateNoWindow = false // Set to true if you don't want to show the command prompt window
            };

            Console.WriteLine(launcher);
            Console.WriteLine(args);

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");

                process.Start();
            }
        }
    }
}
