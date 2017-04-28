using GoogleCloudExtension.Theming;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CreateAppEngineAppDialog
{
    class CreateAppEngineAppDialogWindow : CommonDialogWindowBase
    {
        public CreateAppEngineAppDialogViewModel ViewModel { get; }

        private CreateAppEngineAppDialogWindow():
            base("Create App Engine App")
        {
            ViewModel = new CreateAppEngineAppDialogViewModel(this);
            Content = new CreateAppEngineAppDialogContent { DataContext = ViewModel };
        }

        public static bool PromptUser()
        {
            var dialog = new CreateAppEngineAppDialogWindow();
            dialog.ShowModal();
            return dialog.ViewModel.AppCreated;
        }
    }
}
