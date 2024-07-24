using soba_process_manager.ConfigData;
using System.Diagnostics;
using System.Text.Json;

namespace soba_process_manager.Api
{
    internal class ProcessManager
    {
        private List<Process> Processes { get; set; }

        private Thread? ManagerThread { get; set; } 

        private CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

        private InfererConfigStorage? Config { get; set; }


        internal ProcessManager()
        {
            string filePath = "appSetting.json";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The configuration file was not found.", filePath);
            }

            // Load The Config File
            string jsonString = File.ReadAllText(filePath);
            Config = JsonSerializer.Deserialize<InfererConfigStorage>(jsonString);
            Processes = [];
        }

        public void Init()
        {
            if (Config == null)
            {
                Console.WriteLine("Config file doesn't exist");
                return;
            }

            foreach (var info in Config.LaunchSettings) 
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = info.AutoCloseConsole ? $"/k {info.Launcher} {info.Args}" : $"/c {info.Launcher} {info.Args}",
                    UseShellExecute = true,
                    CreateNoWindow = info.ShowConsole,
                };

                var process = new Process {  StartInfo = startInfo };

                process.OutputDataReceived += (_, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (_, e) => Console.WriteLine($"Error: {e.Data}");

                Processes.Add(process);
            }
        }

        public void Start()
        {
            int index = 0;
            foreach (var process in Processes)
            {
                process.Start();
                Console.WriteLine($"Process started: {Config.LaunchSettings[index].Launcher} {Config.LaunchSettings[index].Args}");
                ++index;
            }


            var task = Task.Run(() =>
            {
                Manage();
            }, Cts.Token);

        }

        public void End()
        {
            Cts.Cancel();

            int i = 0;

            foreach (var process in Processes)
            {
                if (Config.LaunchSettings[i].Launcher == "python")
                {
                    process.WaitForExit();
                    process.Close();
                }
                ++i;
            }

            foreach (var terminateSetting in Config.TerminateSettings)
            {
                var endInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = terminateSetting.AutoCloseConsole ? $"/k {terminateSetting.Launcher} {terminateSetting.Args}" : $"/c {terminateSetting.Launcher} {terminateSetting.Args}",
                    UseShellExecute = true,
                    CreateNoWindow = terminateSetting.ShowConsole
                };

                using (var process = new Process { StartInfo = endInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }

        }
        
        private void Manage()
        {
            while (!Cts.IsCancellationRequested)
            {
                int i = 0;
                foreach (var process in Processes)
                {
                    if (process.HasExited && Config.LaunchSettings[i].Persistent)
                    {
                        process.Start();
                    }
                    ++i;
                }
                Thread.Sleep(1000);
            }
        }

    }
}
