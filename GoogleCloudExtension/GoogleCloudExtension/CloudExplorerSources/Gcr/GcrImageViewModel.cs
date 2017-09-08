using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    class GcrImageViewModel : TreeLeaf
    {
        private const string IconImagePath = "CloudExplorerSources/Gae/Resources/service_icon.png";

        private static readonly Lazy<ImageSource> s_imageIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(IconImagePath));

        private readonly GcrSourceRootViewModel _owner;

        public GcrImageViewModel(GcrSourceRootViewModel owner, string name)
        {
            _owner = owner;

            Caption = name;
            Icon = s_imageIcon.Value;
        }
    }
}
