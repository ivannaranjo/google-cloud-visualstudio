using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudExtension.DataSources.Docker
{
    public class RepoImage
    {
        [JsonProperty("tag")]
        public IList<string> Tags { get; set; }
    }
}