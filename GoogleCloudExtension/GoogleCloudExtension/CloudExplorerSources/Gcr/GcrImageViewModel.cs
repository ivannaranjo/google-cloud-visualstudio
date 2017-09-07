using System;
using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.GCloud.Models;
using System.Windows.Media;
using GoogleCloudExtension.Utils;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    public class GcrImageViewModel : TreeLeaf, ICloudExplorerItemSource
    {
        private const string IconClusterPath = "CloudExplorerSources/Gae/Resources/instance_icon_running.png";

        private static readonly Lazy<ImageSource> s_imageIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(IconClusterPath));


        private readonly GcrSourceRootViewModel _owner;
        private readonly GcrImage _image;

        public GcrImageViewModel(GcrSourceRootViewModel owner, GcrImage image)
        {
            _owner = owner;
            _image = image;

            Caption = _image.Name;
            Icon = s_imageIcon.Value;
        }

        #region ICloudExplorerItemSource
        object ICloudExplorerItemSource.Item => new GcrImageItem(_image);

        event EventHandler ICloudExplorerItemSource.ItemChanged
        {
            add { }
            remove { }
        }
        #endregion
    }
}