using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudExtension.DockerUtils.Models
{
    public class RepoImage
    {
        [JsonProperty("tag")]
        public IList<string> Tags { get; set; }

        [JsonProperty("timeCreatedMs")]
        public ulong Created { get; set; } 

        [JsonProperty("timeUploadedMs")]
        public ulong Updated { get; set; }

        [JsonProperty("imageSizeBytes")]
        public ulong Size { get; set; }
    }
}