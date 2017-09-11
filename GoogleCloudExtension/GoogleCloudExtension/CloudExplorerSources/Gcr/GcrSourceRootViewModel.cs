using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.DataSources;
using GoogleCloudExtension.DockerUtils;
using GoogleCloudExtension.DockerUtils.Models;
using GoogleCloudExtension.GCloud;
using GoogleCloudExtension.GCloud.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    public class GcrSourceRootViewModel : SourceRootViewModelBase
    {
        private static readonly string[] s_repoNames = new string[]
        {
            "gcr.io",
            "us.gcr.io",
            "eu.gcr.io",
            "asia.gcr.io",
        };

        private static readonly TreeLeaf s_loadingPlaceholder = new TreeLeaf
        {
            Caption = "Loading images...",
            IsLoading = true
        };
        private static readonly TreeLeaf s_errorPlaceholder = new TreeLeaf
        {
            Caption = "Failed to load images.",
            IsError = true
        };
        private static readonly TreeLeaf s_noItemsPlacehoder = new TreeLeaf
        {
            Caption = "No images found.",
            IsWarning = true
        };

        private Lazy<DockerRepoDataSource> _dataSource;

        public DockerRepoDataSource DataSource => _dataSource.Value;

        public override TreeLeaf ErrorPlaceholder => s_errorPlaceholder;

        public override TreeLeaf LoadingPlaceholder => s_loadingPlaceholder;

        public override TreeLeaf NoItemsPlaceholder => s_noItemsPlacehoder;

        public override string RootCaption => "Container Registry";

        public override void Initialize(ICloudSourceContext context)
        {
            base.Initialize(context);

            InvalidateProjectOrAccount();
        }

        public override void InvalidateProjectOrAccount()
        {
            Debug.WriteLine("New credentials, invalidating data source for GCR");
            _dataSource = new Lazy<DockerRepoDataSource>(CreateDataSource);
        }


        protected override async Task LoadDataOverride()
        {
            Children.Clear();
            Children.Add(s_loadingPlaceholder);

            var repoTasks = s_repoNames.Select(async (x) =>
            {
                return new Tuple<string, RepoTags>(x, await _dataSource.Value.GetRepoTagsAsync(x, ""));
            });
            var repoTags = await Task.WhenAll(repoTasks);

            Children.Clear();
            foreach (var entry in repoTags)
            {
                var tags = entry.Item2;
                if (tags.Children.Count > 0)
                {
                    Children.Add(new GcrRepoViewModel(this, entry.Item1, tags));
                }
            }
            if (Children.Count == 0)
            {
                Children.Add(s_noItemsPlacehoder);
            }
        }

        private DockerRepoDataSource CreateDataSource()
        {
            if (CredentialsStore.Default.CurrentProjectId != null)
            {
                return new DockerRepoDataSource(
                    CredentialsStore.Default.CurrentProjectId,
                    CredentialsStore.Default.CurrentGoogleCredential);
            }
            else
            {
                return null;
            }
        }
    }
}
