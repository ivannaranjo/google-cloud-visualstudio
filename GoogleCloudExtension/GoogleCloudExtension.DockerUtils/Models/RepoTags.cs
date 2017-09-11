using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.DockerUtils.Models
{
    public class RepoTags
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tags")]
        public IList<string> Tags { get; set; }

        [JsonProperty("child")]
        public IList<string> Children { get; set; }

        [JsonProperty("manifest")]
        public Dictionary<string, RepoImage> Manifest { get; set; }
    }
}
