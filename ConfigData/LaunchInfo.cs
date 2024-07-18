using System.Text.Json.Serialization;

namespace soba_process_manager.ConfigData
{
    // Data Struct
    public class LaunchInfo
    {
        [JsonPropertyName("launcher")]
        public string Launcher { get; set; }

        [JsonPropertyName("args")]
        public string Args { get; set; }

        [JsonPropertyName("persistent")]
        public bool Persistent { get; set; }

        [JsonPropertyName("show_console")]
        public bool ShowConsole { get; set; }

        [JsonPropertyName("auto_close_console")]
        public bool AutoCloseConsole { get; set; }

        [JsonPropertyName("wait")]
        public int Wait { get; set; }
    }

    // Headers
    public class InfererConfigStorage
    {
        [JsonPropertyName("LaunchSettings")]
        public List<LaunchInfo> LaunchSettings { get; set; }

        [JsonPropertyName("TerminateSettings")]
        public List<LaunchInfo> TerminateSettings { get; set; }
    }
}
