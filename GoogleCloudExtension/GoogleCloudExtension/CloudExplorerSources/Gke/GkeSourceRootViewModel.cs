using Google.Apis.Container.v1.Data;
using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.ApiManagement;
using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.DataSources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CloudExplorerSources.Gke
{
    public class GkeSourceRootViewModel : SourceRootViewModelBase
    {
        private static readonly TreeLeaf s_loadingPlaceholder = new TreeLeaf
        {
            Caption = "Loaing clusters...",
            IsLoading = true
        };
        private static readonly TreeLeaf s_errorPlaceholder = new TreeLeaf
        {
            Caption = "Failed to load clusters.",
            IsError = true
        };
        private static readonly TreeLeaf s_noItemsPlacehoder = new TreeLeaf
        {
            Caption = "No clusters found.",
            IsWarning = true
        };

        private static readonly IList<string> s_requiredApis = new List<string>
        {
            // The GKE API is required.
            KnownApis.ContainerEngineApiName
        };

        private IList<Cluster> _clusters;
        private Lazy<GkeDataSource> _dataSource;

        public GkeDataSource DataSource => _dataSource.Value;

        public override TreeLeaf ErrorPlaceholder => s_errorPlaceholder;

        public override TreeLeaf LoadingPlaceholder => s_loadingPlaceholder;

        public override TreeLeaf NoItemsPlaceholder => s_noItemsPlacehoder;

        public override TreeLeaf ApiNotEnabledPlaceholder
            => new TreeLeaf
            {
                Caption = "The GKE API is not enabled",
                IsError = true
            };

        public override string RootCaption => "Container Engine";

        public override IList<string> RequiredApis => s_requiredApis;

        public override void Initialize(ICloudSourceContext context)
        {
            base.Initialize(context);

            InvalidateProjectOrAccount();
        }

        public override void InvalidateProjectOrAccount()
        {
            Debug.WriteLine("New credentials, invalidating data source for GKE");
            _dataSource = new Lazy<GkeDataSource>(CreateDataSource);
        }

        protected override async Task LoadDataOverride()
        {
            try
            {
                _clusters = null;
                _clusters = await _dataSource.Value.GetClusterListAsync();
                PresentViewModels();
            }
            catch (DataSourceException ex)
            {
                throw new CloudExplorerSourceException(ex.Message, ex);
            }
        }

        private void PresentViewModels()
        {
            Children.Clear();
            var viewModels = CalculateViewModels(_clusters);
            foreach (var model in viewModels)
            {
                Children.Add(model);
            }
        }

        private IEnumerable<TreeNode> CalculateViewModels(IList<Cluster> clusters)
            => clusters?.Select(x => new GkeClusterViewModel(this, x)) ?? Enumerable.Empty<TreeNode>();

        private GkeDataSource CreateDataSource()
        {
            if (CredentialsStore.Default.CurrentProjectId != null)
            {
                return new GkeDataSource(
                    CredentialsStore.Default.CurrentProjectId,
                    CredentialsStore.Default.CurrentGoogleCredential,
                    GoogleCloudExtensionPackage.VersionedApplicationName);
            }
            else
            {
                return null;
            }
        }
    }
}
