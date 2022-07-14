using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Build.Utilities;
using Microsoft.ServiceHub.Framework;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.Configuration;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;
using Resx = NuGet.PackageManagement.UI.Resources;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.Internal.VisualStudio.PlatformUI;

using Microsoft.VisualStudio.Shell;


namespace NuGet.Options
{
    public partial class AddMappingDialog : Window
    {
        public ICommand HideButtonCommand { get; set; }

        public ICommand AddButtonCommand { get; set; }

        public ItemsChangeObservableCollection<PackageSourceContextInfoChecked> SourcesCollection { get; private set; } //change to package s

        private IReadOnlyList<PackageSourceContextInfo> _originalPackageSources;

#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
        private INuGetSourcesService _nugetSourcesService;
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings

        private PackageSourceMappingOptionsControl _parent;

        public AddMappingDialog(PackageSourceMappingOptionsControl parent)
        {
            _parent = parent;
            HideButtonCommand = new ButtonCommand(ExecuteHideButtonCommand, CanExecuteHideButtonCommand);
            AddButtonCommand = new ButtonCommand(ExecuteAddButtonCommand, CanExecuteAddButtonCommand);
            SourcesCollection = new ItemsChangeObservableCollection<PackageSourceContextInfoChecked>();
            DataContext = this;
            InitializeComponent();
            CancellationToken cancellationToken = new CancellationToken(false);
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () => await InitializeOnActivatedAsync(cancellationToken));
        }

        internal async Task InitializeOnActivatedAsync(CancellationToken cancellationToken)
        {
            IServiceBrokerProvider serviceBrokerProvider = await ServiceLocator.GetComponentModelServiceAsync<IServiceBrokerProvider>();
            IServiceBroker serviceBroker = await serviceBrokerProvider.GetAsync();
#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
            _nugetSourcesService = await serviceBroker.GetProxyAsync<INuGetSourcesService>(
                    NuGetServices.SourceProviderService,
                    cancellationToken: cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings

            //show package sources on open
            _originalPackageSources = await _nugetSourcesService.GetPackageSourcesAsync(cancellationToken);
            SourcesCollection.Clear();
            foreach (var source in _originalPackageSources)
            {
                var tempSource = new PackageSourceContextInfoChecked(source, false);
                SourcesCollection.Add(tempSource);
            }
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
        }

        private void ExecuteHideButtonCommand(object parameter)
        {
            Close();
            (_parent.ShowButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteHideButtonCommand(object parameter)
        {
            return true;
        }

        private void ExecuteAddButtonCommand(object parameter)
        {
            Close();
            //does not add mapping if package ID is null
            if (!string.IsNullOrEmpty(packageID.Text))
            {
                var tempPkgID = packageID.Text;
                ObservableCollection<PackageSourceContextInfo> tempSources = new ObservableCollection<PackageSourceContextInfo>();
                foreach (PackageSourceContextInfoChecked source in sourcesListBox.Items)
                {
                    if (source.IsChecked)
                    {
                        tempSources.Add(source.SourceInfo);
                    }
                }
                PackageItem tempPkg = new PackageItem(tempPkgID, tempSources);
                _parent.SourceMappingsCollection.Add(tempPkg);
            }
            (_parent.ShowButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
            (_parent.RemoveButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
            (_parent.ClearButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteAddButtonCommand(object parameter)
        {
            return true;
        }

        // Allows the user to drag the window around
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
