using Newtonsoft.Json;

namespace OneDriveUploaderSample.Model.OneDrive.Request
{
    public class RequestLinkInfo
    {
        [JsonProperty("type")]
        public string Type
        {
            get;
            set;
        }
    }
}