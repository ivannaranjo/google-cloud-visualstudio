using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.PublishDialog;
using GoogleCloudExtension.Utils;
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
        private IPublishDialog _dialog;
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
        }

        #region Overrides for PublishDialogStepBase

        public override FrameworkElement Content => _content;

        public override void OnPushedToDialog(IPublishDialog dialog)
        {
            _dialog = dialog;

            _projectName = CredentialsStore.Default.CurrentProjectId;
            ImageName = dialog.Project.Name.ToLower();
            ImageTag = GcpPublishStepsUtils.GetDefaultVersion();
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
