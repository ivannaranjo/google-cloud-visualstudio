using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.GCloud;
using GoogleCloudExtension.GCloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    public class GcrSourceRootViewModel : SourceRootViewModelBase
    {
        private static readonly TreeLeaf s_loadingPlaceholder = new TreeLeaf
        {
            Caption = "Loaing images...",
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

        private IList<GcrImage> _images;

        public override TreeLeaf ErrorPlaceholder => s_errorPlaceholder;

        public override TreeLeaf LoadingPlaceholder => s_loadingPlaceholder;

        public override TreeLeaf NoItemsPlaceholder => s_noItemsPlacehoder;

        public override string RootCaption => "Container Registry";

        protected override async Task LoadDataOverride()
        {
            try
            {
                var context = new GCloudContext
                {
                    CredentialsPath = CredentialsStore.Default.CurrentAccountPath,
                    ProjectId = CredentialsStore.Default.CurrentProjectId,
                    AppName = GoogleCloudExtensionPackage.ApplicationName,
                    AppVersion = GoogleCloudExtensionPackage.ApplicationVersion,
                };
                var repo = $"gcr.io/{context.ProjectId}";

                _images = null;
                _images = await GCloudWrapper.GetGcrDockerImagesAsync(repo, context);
                PresentViewModels();
            }
            catch (GCloudException ex)
            {
                throw new CloudExplorerSourceException(ex.Message, ex);
            }
        }

        private void PresentViewModels()
        {
            Children.Clear();
            var viewModels = CalculateViewModels(_images);
            foreach (var model in viewModels)
            {
                Children.Add(model);
            }
        }

        private IEnumerable<GcrImageViewModel> CalculateViewModels(IList<GcrImage> images)
            => images.Select(x => new GcrImageViewModel(this, x));
    }
}
