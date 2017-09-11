using GoogleCloudExtension.CloudExplorer;
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
    public class GcrRepoViewModel : TreeHierarchy
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

        public string RepoName { get; }

        private readonly GcrSourceRootViewModel _owner;
        private readonly RepoTags _tags;

        public GcrRepoViewModel(GcrSourceRootViewModel owner, string repoName, RepoTags tags)
        {
            _owner = owner;
            _tags = tags;

            RepoName = repoName;

            Caption = repoName;
            Icon = s_pathStepIcon.Value;

            var viewModels = CalculateViewModels(_tags);
            foreach (var model in viewModels)
            {
                Children.Add(model);
            }
            if (Children.Count == 0)
            {
                Children.Add(s_noItemsPlaceholder);
            }
        }

        private IEnumerable<GcrPathStepViewModel> CalculateViewModels(RepoTags tags)
            => tags?.Children.Select(x => new GcrPathStepViewModel(_owner, this, x)) ?? Enumerable.Empty<GcrPathStepViewModel>();
    }
}
