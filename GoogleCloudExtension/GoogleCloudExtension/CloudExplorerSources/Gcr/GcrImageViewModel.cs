using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.DockerUtils;
using GoogleCloudExtension.DockerUtils.Models;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    class GcrImageViewModel : TreeLeaf, ICloudExplorerItemSource
    {
        private const string IconImagePath = "CloudExplorerSources/Gae/Resources/instance_icon_running.png";

        private static readonly Lazy<ImageSource> s_imageIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(IconImagePath));

        private readonly GcrSourceRootViewModel _owner;
        private readonly GcrRepoViewModel _repo;
        private readonly RepoImage _image;
        private readonly string _hash;
        private readonly string _fullPath;

        public GcrImageViewModel(GcrSourceRootViewModel owner, GcrRepoViewModel repo, string name, string hash, RepoImage image)
        {
            _owner = owner;
            _repo = repo;
            _image = image;
            _hash = hash;

            _fullPath = owner.DataSource.GetFullPath(repo: _repo.RepoName, name: name, hash: hash);

            Caption = String.Join(", ", image.Tags);
            Icon = s_imageIcon.Value;
        }

        #region ICloudExplorerItemSource

        object ICloudExplorerItemSource.Item => new GcrImageItem(hash: _hash, fullPath: _fullPath, image: _image);

        event EventHandler ICloudExplorerItemSource.ItemChanged
        {
            add { }
            remove { }
        }

        #endregion
    }
}
