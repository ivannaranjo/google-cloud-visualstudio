using Google.Apis.Container.v1.Data;
using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GoogleCloudExtension.CloudExplorerSources.Gke
{
    public class GkeClusterViewModel : TreeLeaf, ICloudExplorerItemSource
    {
        private const string IconClusterPath = "CloudExplorerSources/Gae/Resources/instance_icon_running.png";

        private static readonly Lazy<ImageSource> s_instanceClusterIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(IconClusterPath));

        private readonly GkeSourceRootViewModel _owner;
        private readonly Cluster _cluster;

        public GkeClusterViewModel(GkeSourceRootViewModel owner, Cluster cluster)
        {
            _owner = owner;
            _cluster = cluster;

            Caption = _cluster.Name;
            Icon = s_instanceClusterIcon.Value;
        }

        #region ICloudExplorerItemSource

        object ICloudExplorerItemSource.Item => new GkeClusterItem(_cluster);

        event EventHandler ICloudExplorerItemSource.ItemChanged
        {
            add { }
            remove { }
        }

        #endregion
    }
}
