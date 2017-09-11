using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.DockerUtils.Models;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    public class GcrPathStepViewModel : TreeHierarchy
    {
        private const string IconPathStepPath = "CloudExplorerSources/Gae/Resources/service_icon.png";

        private static readonly Lazy<ImageSource> s_pathStepIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(IconPathStepPath));

        private static readonly TreeLeaf s_loadingPlaceholder = new TreeLeaf
        {
            Caption = "Loading children...",
            IsLoading = true
        };
        private static readonly TreeLeaf s_errorPlaceholder = new TreeLeaf
        {
            Caption = "Failed to load ",
            IsError = true
        };
        private static readonly TreeLeaf s_noItemsPlaceholder = new TreeLeaf
        {
            Caption = "No children found.",
            IsWarning = true
        };

        private readonly GcrSourceRootViewModel _owner;
        private readonly GcrRepoViewModel _repo;
        private readonly string _path;
        private bool _isLoading = false;
        private bool _isLoaded = false;
        private RepoTags _tags;

        public GcrPathStepViewModel(GcrSourceRootViewModel owner, GcrRepoViewModel repo, string name, string path)
        {
            _owner = owner;
            _repo = repo;
            _path = path;

            Caption = name;
            Icon = s_pathStepIcon.Value;
            Children.Add(s_loadingPlaceholder);
        }

        protected override async void OnIsExpandedChanged(bool newValue)
        {
            if (!newValue || _isLoading || _isLoaded)
            {
                return;
            }

            try
            {
                _isLoading = true;

                Debug.WriteLine($"Loading GCR children of node {_path}");
                _tags = await _owner.DataSource?.GetRepoTagsAsync(_repo.RepoName, _path);
                Children.Clear();
                if (_tags != null)
                {
                    foreach (var child in _tags.Children)
                    {
                        var newPath = $"{_path}/{child}";
                        Children.Add(new GcrPathStepViewModel(_owner, _repo, child, newPath));
                    }
                    foreach (var entry in _tags.Manifest.OrderByDescending(x => x.Value.Uploaded))
                    {
                        Children.Add(new GcrImageViewModel(
                            _owner,
                            _repo,
                            name: Caption,
                            hash: entry.Key,
                            image: entry.Value));
                    }
                }
                if (Children.Count == 0)
                {
                    Children.Add(s_noItemsPlaceholder);
                }
                _isLoaded = true;
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
