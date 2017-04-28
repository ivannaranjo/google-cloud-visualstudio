using GoogleCloudExtension.Theming;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CreateAppEngineAppDialog
{
    class CreateAppEngineAppDialogViewModel : ViewModelBase
    {
        private static readonly string[] s_regions = new string[]
        {
            "us-central",
            "europe-west",
            "us-east1",
            "asia-northeast1"
        };

        private readonly CreateAppEngineAppDialogWindow _owner;
        private string _selectedRegion;

        public IEnumerable<string> Regions => s_regions;

        public string SelectedRegion
        {
            get { return _selectedRegion; }
            set { SetValueAndRaise(ref _selectedRegion, value); }
        }

        public bool AppCreated { get; private set; }

        public ProtectedCommand CreateAppCommand { get; }

        public CreateAppEngineAppDialogViewModel(CreateAppEngineAppDialogWindow owner)
        {
            _owner = owner;

            CreateAppCommand = new ProtectedCommand(OnCreateAppCommand);
        }

        private void OnCreateAppCommand()
        {
            throw new NotImplementedException();
        }
    }
}
