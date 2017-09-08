using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.DataSources.Docker;
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
        private bool _isLoading = false;
        private bool _isLoaded = false;
        private RepoTags _tags;

        public GcrPathStepViewModel(GcrSourceRootViewModel owner, string name)
        {
            _owner = owner;

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

                Debug.WriteLine($"Loading GCR children of node {Caption}");
                _tags = await _owner.DataSource?.GetRepoTagsAsync(Caption);
                Children.Clear();
                if (_tags != null)
                {
                    foreach (var child in _tags.Children)
                    {
                        Children.Add(new GcrPathStepViewModel(_owner, child));
                    }
                    foreach (var entry in _tags.Manifest)
                    {
                        Children.Add(new GcrImageViewModel(
                            _owner,
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
