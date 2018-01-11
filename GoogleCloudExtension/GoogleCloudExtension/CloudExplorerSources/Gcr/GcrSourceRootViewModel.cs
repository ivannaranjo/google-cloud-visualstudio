using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.ApiManagement;
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
using System.Windows.Controls;

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

        private static readonly IList<string> s_requiredApis = new List<string>
        {
            // The GCR API is required.
            KnownApis.ContainerRegistryApiName
        };

        private Lazy<DockerRepoDataSource> _dataSource;

        public DockerRepoDataSource DataSource => _dataSource.Value;

        public override TreeLeaf ErrorPlaceholder => s_errorPlaceholder;

        public override TreeLeaf LoadingPlaceholder => s_loadingPlaceholder;

        public override TreeLeaf NoItemsPlaceholder => s_noItemsPlacehoder;

        public override TreeLeaf ApiNotEnabledPlaceholder
            => new TreeLeaf
            {
                Caption = "The GCR API is not enabled",
                IsError = true,
            };

        public override string RootCaption => "Container Registry";

        public override IList<string> RequiredApis => s_requiredApis;

        public override void Initialize(ICloudSourceContext context)
        {
            base.Initialize(context);

            InvalidateProjectOrAccount();

            ContextMenu = new ContextMenu();
        }

        public override void InvalidateProjectOrAccount()
        {
            Debug.WriteLine("New credentials, invalidating data source for GCR");
            _dataSource = new Lazy<DockerRepoDataSource>(CreateDataSource);
        }


        protected override async Task LoadDataOverride()
        {
            var repos = new List<GcrRepoViewModel>();
            foreach (var name in s_repoNames)
            {
                var tags = await _dataSource.Value.GetRepoTagsAsync(name, "");
                if (tags.Children.Count > 0)
                {
                    repos.Add(new GcrRepoViewModel(this, name, tags));
                }
                if (repos.Count >= 1)
                {
                    break;
                }
            }

            Children.Clear();
            foreach (var repo in repos)
            {
                Children.Add(repo);
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
