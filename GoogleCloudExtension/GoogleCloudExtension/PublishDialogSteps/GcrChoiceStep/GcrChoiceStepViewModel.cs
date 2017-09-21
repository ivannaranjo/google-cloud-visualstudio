using GoogleCloudExtension.PublishDialog;
using GoogleCloudExtension.PublishDialogSteps.Common;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace GoogleCloudExtension.PublishDialogSteps.GcrChoiceStep
{
    public class GcrChoiceStepViewModel : PublishDialogStepBase
    {
        private const string AppEngineIconPath = "PublishDialogSteps/ChoiceStep/Resources/AppEngine_128px_Retina.png";
        private const string GceIconPath = "PublishDialogSteps/ChoiceStep/Resources/ComputeEngine_128px_Retina.png";
        private const string GkeIconPath = "PublishDialogSteps/ChoiceStep/Resources/ContainerEngine_128px_Retina.png";

        private static readonly Lazy<ImageSource> s_appEngineIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(AppEngineIconPath));
        private static readonly Lazy<ImageSource> s_gceIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(GceIconPath));
        private static readonly Lazy<ImageSource> s_gkeIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(GkeIconPath));

        private readonly GcrChoiceStepContent _content;
        private IPublishDialog _dialog;
        private IEnumerable<Choice> _choices;

        /// <summary>
        /// The choices available for the project being published.
        /// </summary>
        public IEnumerable<Choice> Choices
        {
            get { return _choices; }
            set { SetValueAndRaise(ref _choices, value); }
        }

        private GcrChoiceStepViewModel(GcrChoiceStepContent content)
        {
            _content = content;
        }

        #region PublishDialogStepBase overrides

        public override FrameworkElement Content => _content;

        public override void OnPushedToDialog(IPublishDialog dialog)
        {
            _dialog = dialog;

            Choices = new List<Choice>
            {
                new Choice
                {
                    Name = Resources.PublishDialogChoiceStepAppEngineFlexName,
                    Command = new ProtectedCommand(OnAppEngineChoiceCommand),
                    Icon = s_appEngineIcon.Value,
                    ToolTip = Resources.PublishDialogChoiceStepAppEngineToolTip
                },
                new Choice
                {
                    Name = Resources.PublishDialogChoiceStepGkeName,
                    Command = new ProtectedCommand(OnGkeChoiceCommand),
                    Icon = s_gkeIcon.Value,
                    ToolTip = Resources.PublishDialogChoiceStepGkeToolTip
                },

            };
        }

        #endregion

        private void OnGkeChoiceCommand()
        {
            throw new NotImplementedException();
        }

        private void OnAppEngineChoiceCommand()
        {
            throw new NotImplementedException();
        }

        public static IPublishDialogStep CreateStep()
        {
            var content = new GcrChoiceStepContent();
            var viewModel = new GcrChoiceStepViewModel(content);
            content.DataContext = viewModel;

            return viewModel;
        }
    }
}
