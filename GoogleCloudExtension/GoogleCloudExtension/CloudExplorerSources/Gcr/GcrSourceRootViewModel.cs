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


        protected override Task LoadDataOverride()
        {
            Children.Clear();
            foreach (var repo in s_repoNames.Select(x => new GcrRepoViewModel(this, x)))
            {
                Children.Add(repo);
            }

            // TODO: Better way of doing this.
            var ts = new TaskCompletionSource<bool>();
            ts.SetResult(false);
            return ts.Task;
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
