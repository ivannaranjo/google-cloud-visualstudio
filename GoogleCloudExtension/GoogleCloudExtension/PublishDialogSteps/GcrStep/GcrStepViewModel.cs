using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.Deployment;
using GoogleCloudExtension.GCloud;
using GoogleCloudExtension.PublishDialog;
using GoogleCloudExtension.Utils;
using GoogleCloudExtension.VsVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoogleCloudExtension.PublishDialogSteps.GcrStep
{
    class GcrStepViewModel : PublishDialogStepBase
    {
        private readonly GcrStepContent _content;
        private IPublishDialog _publishDialog;
        private string _projectName;
        private string _imageTag;
        private string _imageName;

        public string ImageTag
        {
            get { return _imageTag; }
            set
            {
                SetValueAndRaise(ref _imageTag, value);
                RaisePropertyChanged(nameof(ImageFullName));
            }
        }

        public string ImageName
        {
            get { return _imageName; }
            set
            {
                SetValueAndRaise(ref _imageName, value);
                RaisePropertyChanged(nameof(ImageFullName));
            }
        }

        public string ImageFullName => $"gcr.io/{_projectName}/{ImageName}:{ImageTag}";

        private GcrStepViewModel(GcrStepContent content)
        {
            _content = content;

            CanPublish = true;
        }

        #region Overrides for PublishDialogStepBase

        public override FrameworkElement Content => _content;

        public override void OnPushedToDialog(IPublishDialog dialog)
        {
            _publishDialog = dialog;

            _projectName = CredentialsStore.Default.CurrentProjectId;
            ImageName = dialog.Project.Name.ToLower();
            ImageTag = GcpPublishStepsUtils.GetDefaultVersion();
        }

        public override async void Publish()
        {
            try
            {
                ShellUtils.SaveAllFiles();

                var project = _publishDialog.Project;

                var gcloudContext = new GCloudContext
                {
                    CredentialsPath = CredentialsStore.Default.CurrentAccountPath,
                    ProjectId = CredentialsStore.Default.CurrentProjectId,
                    AppName = GoogleCloudExtensionPackage.ApplicationName,
                    AppVersion = GoogleCloudExtensionPackage.ApplicationVersion,
                };

                var options = new GcrDeployment.DeploymentOptions
                {
                    ImageName = ImageName,
                    ImageTag = ImageTag,
                    GCloudContext = gcloudContext
                };

                GcpOutputWindow.Activate();
                GcpOutputWindow.Clear();
                GcpOutputWindow.OutputLine($"Stating build of image {ImageFullName}");

                _publishDialog.FinishFlow();

                bool result;
                using (StatusbarHelper.Freeze())
                using (StatusbarHelper.ShowDeployAnimation())
                using (var progress = StatusbarHelper.ShowProgressBar("Building Docker image"))
                using (ShellUtils.SetShellUIBusy())
                {
                    result = await GcrDeployment.PublishProjectAsync(
                        project,
                        options,
                        progress,
                        VsVersionUtils.ToolsPathProvider,
                        GcpOutputWindow.OutputLine);
                }

                if (result)
                {
                    StatusbarHelper.SetText("Docker image build succeeded");
                }
                else
                {
                    GcpOutputWindow.OutputLine($"Failed to build image {ImageFullName}.");
                    StatusbarHelper.SetText("Docker image build failed");
                }
            }
            catch (Exception ex) when (!ErrorHandlerUtils.IsCriticalException(ex))
            {
                GcpOutputWindow.OutputLine($"Failed to build image {ImageFullName}");
                StatusbarHelper.SetText(Resources.PublishFailureStatusMessage);
                _publishDialog.FinishFlow();
            }
        }

        #endregion

        internal static GcrStepViewModel CreateStep()
        {
            var content = new GcrStepContent();
            var viewModel = new GcrStepViewModel(content);
            content.DataContext = viewModel;

            return viewModel;
        }
    }
}
