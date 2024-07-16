using System.Text.Json.Serialization;

namespace soba_process_manager.ConfigData
{
    public class LaunchInfo
    {
        [JsonPropertyName("launcher")]
        public string Launcher { get; set; }

        [JsonPropertyName("args")]
        public string Args { get; set; }

        [JsonPropertyName("wait")]
        public int Wait { get; set; }
    }

    public class InfererConfigStorage
    {
        [JsonPropertyName("LaunchSettings")]
        public List<LaunchInfo> LaunchSettings { get; set; }

        [JsonPropertyName("TerminateSettings")]
        public List<LaunchInfo> TerminateSettings { get; set; }
    }
}
