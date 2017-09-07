using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.GCloud.Models
{
    public class GcrImage
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
