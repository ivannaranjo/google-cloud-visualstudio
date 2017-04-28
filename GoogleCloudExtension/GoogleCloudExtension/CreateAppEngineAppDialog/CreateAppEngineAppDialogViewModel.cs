using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.DataSources;
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
        private readonly CreateAppEngineAppDialogWindow _owner;
        private readonly GaeDataSource _dataSource;
        private string _selectedRegion;

        public AsyncPropertyValue<IEnumerable<string>> Locations { get; }

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
            _dataSource = new GaeDataSource(
                CredentialsStore.Default.CurrentProjectId,
                CredentialsStore.Default.CurrentGoogleCredential,
                GoogleCloudExtensionPackage.ApplicationName);

            Locations = new AsyncPropertyValue<IEnumerable<string>>(GetLocationsAsync());
            CreateAppCommand = new ProtectedCommand(OnCreateAppCommand);
        }

        private void OnCreateAppCommand()
        {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<string>> GetLocationsAsync()
        {
            var locations = await _dataSource.GetLocationsAsync();
            return locations.Select(x => x.LocationId).ToList();
        }
    }
}
