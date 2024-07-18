using soba_process_manager.ConfigData;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace soba_process_manager.Api
{
    internal class Initialize
    {
        public static (bool, List<Thread>) Init(List<LaunchInfo> initSettings)
        {
            bool loadStatus = true;
            List<Thread> activeProcesses = new List<Thread>();

            foreach (LaunchInfo launchSettings in initSettings)
            {
                if (!loadStatus) break;

                activeProcesses.Add( ProcessManager.CreateThreadedProcess(launchSettings) );
            }

            return (loadStatus, activeProcesses);
        }
    }
}
