using soba_process_manager.ConfigData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soba_process_manager.Api
{
    internal class Terminate
    {
        public static void Cleanup(List<LaunchInfo> terminateSettings)
        {
            foreach (LaunchInfo terminateInfo in terminateSettings)
            {

                string launcher = terminateInfo.Launcher;
                string arg = terminateInfo.Args;
                int wait = terminateInfo.Wait;

                switch (launcher)
                {
                    case "taskkill":
                    {
                        Initialize.StartPublicProcess(launcher, arg, wait);
                        break;
                    }
                    case "docker":
                    {
                        Initialize.StartPrivateProcess(launcher, arg, wait);
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Unknown terminate launcher specified: ", launcher);
                        break;
                    }
                }
            }
        }
    }
}
