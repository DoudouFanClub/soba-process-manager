using soba_process_manager.ConfigData;

namespace soba_process_manager.Api
{
    internal class Terminate
    {
        public static void Cleanup(List<LaunchInfo> terminateSettings)
        {
            foreach (LaunchInfo terminateInfo in terminateSettings)
            {
                ProcessManager.StartProcess(terminateInfo);
            }
        }
    }
}
