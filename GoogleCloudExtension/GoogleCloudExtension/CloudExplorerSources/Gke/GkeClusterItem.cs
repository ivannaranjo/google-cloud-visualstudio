using Google.Apis.Container.v1.Data;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CloudExplorerSources.Gke
{
    public class GkeClusterItem : PropertyWindowItemBase
    {
        private readonly Cluster _cluster;

        public GkeClusterItem(Cluster cluster)
            : base(className: "GKE Cluster", componentName: cluster.Name)
        {
            _cluster = cluster;
        }

        public string Name => _cluster.Name;

        public string Status => _cluster.Status;

        public string CurrentVersion => _cluster.CurrentNodeVersion;

        public string CurrentMasterVersion => _cluster.CurrentMasterVersion;

        public string Nodes => _cluster.CurrentNodeCount.ToString();

        public string Description => _cluster.Description;

        public string IsKubernetesAlpha => _cluster.EnableKubernetesAlpha.ToString();

        public string CreationName => _cluster.CreateTime.ToString();

        public string Zone => _cluster.Zone;
    }
}
