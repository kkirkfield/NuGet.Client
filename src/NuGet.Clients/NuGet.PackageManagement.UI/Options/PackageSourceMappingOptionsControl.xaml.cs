// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.ServiceHub.Framework;

using Microsoft.VisualStudio.Shell;
using NuGet.Configuration;
using NuGet.PackageManagement.UI;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;
using Task = System.Threading.Tasks.Task;
using Resx = NuGet.PackageManagement.UI.Resources;
//using AddMapping = NuGet.PackageManagement.UI.AddMappingDialog;
//using NuGet.PackageManagement.UI.Options;

namespace NuGet.Options
{
    /// <summary>
    /// Interaction logic for PackageSourceMappingOptionsControl.xaml
    /// </summary>
    public partial class PackageSourceMappingOptionsControl : UserControl
    {

        public ItemsChangeObservableCollection<PackageSourceItem> SourcesCollection { get; private set; } //change to package s

        public ItemsChangeObservableCollection<PackageItem> SourceMappingsCollection { get; private set; }

        private IReadOnlyList<PackageSourceContextInfo> _originalPackageSources;

        private IReadOnlyList<PackageSourceMappingSourceItem> _originalPackageSourceMappings;
#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
        private INuGetSourcesService _nugetSourcesService;
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings

        public ICommand ShowButtonCommand { get; set; }

        public ICommand HideButtonCommand { get; set; }

        public ICommand AddButtonCommand { get; set; }

        // public ICommand IsCheckedCommand { get; set; }

        public ICommand RemoveButtonCommand { get; set; }

        public ICommand ClearButtonCommand { get; set; }

        private AddMappingDialog _addMappingDialog;


        //public ICommand IsCheckedCommand { get; set; }

        //public bool Checked { get; set; }

        private Dictionary<string, ObservableCollection<PackageSourceContextInfo>> _sourceMappings;

        public PackageSourceMappingOptionsControl()
        {
            ShowButtonCommand = new ShowButtonCommand(ExecuteShowButtonCommand, CanExecuteShowButtonCommand);

            HideButtonCommand = new HideButtonCommand(ExecuteHideButtonCommand, CanExecuteHideButtonCommand);

            AddButtonCommand = new AddButtonCommand(ExecuteAddButtonCommand, CanExecuteAddButtonCommand);

            RemoveButtonCommand = new RemoveButtonCommand(ExecuteRemoveButtonCommand, CanExecuteRemoveButtonCommand);

            ClearButtonCommand = new ClearButtonCommand(ExecuteClearButtonCommand, CanExecuteClearButtonCommand);

            // IsCheckedCommand = new IsCheckedCommand(ExecuteIsCheckedCommand, CanExecuteIsCheckedCommand);

            // SourcesCollection = new ObservableCollection<object>();
            SourcesCollection = new ItemsChangeObservableCollection<PackageSourceItem>();

            SourceMappingsCollection = new ItemsChangeObservableCollection<PackageItem>();
            DataContext = this;
            InitializeComponent();

            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
            _sourceMappings = new Dictionary<string, ObservableCollection<PackageSourceContextInfo>>();
        }

        internal async Task InitializeOnActivatedAsync(CancellationToken cancellationToken)
        {
            //_nugetSourcesService?.Dispose();
            //_nugetSourcesService = null;

            IServiceBrokerProvider serviceBrokerProvider = await ServiceLocator.GetComponentModelServiceAsync<IServiceBrokerProvider>();
            IServiceBroker serviceBroker = await serviceBrokerProvider.GetAsync();

#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
            _nugetSourcesService = await serviceBroker.GetProxyAsync<INuGetSourcesService>(
                    NuGetServices.SourceProviderService,
                    cancellationToken: cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings

            //show package sources on open
            _originalPackageSources = await _nugetSourcesService.GetPackageSourcesAsync(cancellationToken);

            //clear sources so they don't repeat after opening options window a few times
            SourcesCollection.Clear();
            foreach (var source in _originalPackageSources)
            {
                var tempSource = new PackageSourceItem(source, false);
                SourcesCollection.Add(tempSource);
            }

            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);

            //show package source mappings on open
            var componentModelMapping = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            var settings = componentModelMapping.GetService<ISettings>();
            PackageSourceMappingProvider packageSourceMappingProvider = new PackageSourceMappingProvider(settings);
            _originalPackageSourceMappings = packageSourceMappingProvider.GetPackageSourceMappingItems();
            ObservableCollection<PackageItem> SourceMappingsCollectiontemp = ReadMappingsFromConfigToUI(_originalPackageSourceMappings);
            //clear sourcemappings so that they don't repeat
            SourceMappingsCollection.Clear();
            foreach (var item in SourceMappingsCollectiontemp)
            {
                SourceMappingsCollection.Add(item);
            }
            //make sure all buttons show on open if there are already sourcemappings
            (ClearButtonCommand as ClearButtonCommand).InvokeCanExecuteChanged();
            (RemoveButtonCommand as RemoveButtonCommand).InvokeCanExecuteChanged();
        }
        private void ExecuteShowButtonCommand(object parameter)
        {
            //MyPopup.IsOpen = true;
            _addMappingDialog = new AddMappingDialog();
            //addMappingDialog.Show();
        }

        private bool CanExecuteShowButtonCommand(object parameter)
        {
            if (MyPopup != null && MyPopup.IsOpen == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteHideButtonCommand(object parameter)
        {
            MyPopup.IsOpen = false;
            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteHideButtonCommand(object parameter)
        {
            return true;
        }


        private void ExecuteAddButtonCommand(object parameter)
        {
            //MessageBox.Show(packageID.Text.Length.ToString());
            //MyPopup.IsOpen = false;
            var tempPkgID = _addMappingDialog.packageID.Text;
            ObservableCollection<PackageSourceContextInfo> tempSources = new ObservableCollection<PackageSourceContextInfo>();
            foreach (PackageSourceItem source in _addMappingDialog.sourcesListBox.Items)
            {
                if (source.IsChecked)
                {
                    tempSources.Add(source.SourceInfo);
                }
            }
            PackageItem tempPkg = new PackageItem(tempPkgID, tempSources);
            SourceMappingsCollection.Add(tempPkg);
            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
            (RemoveButtonCommand as RemoveButtonCommand).InvokeCanExecuteChanged();
            (ClearButtonCommand as ClearButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteAddButtonCommand(object parameter)
        {
            /*var tempPkg = packageID.Text;
            ObservableCollection<PackageSourceContextInfo> tempSources = new ObservableCollection<PackageSourceContextInfo>();
            foreach (PackageSourceItem source in sourcesListBox.Items)
            {
                if (source.IsChecked)
                {
                    tempSources.Add(source.SourceInfo);
                }
            }
            //return true;
            return !string.IsNullOrWhiteSpace(tempPkg) && tempSources.Count > 0;
            //MessageBox.Show(packageID.Text.Length.ToString());*/
            return true;
        }

        private void ExecuteRemoveButtonCommand(object parameter)
        {
            SourceMappingsCollection.Remove((PackageItem)packageList.SelectedItem);
            (ClearButtonCommand as ClearButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteRemoveButtonCommand(object parameter)
        {
            return MyPopup != null && SourceMappingsCollection.Count > 0;
        }

        private void ExecuteClearButtonCommand(object parameter)
        {
            SourceMappingsCollection.Clear();
            (RemoveButtonCommand as RemoveButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteClearButtonCommand(object parameter)
        {
            return MyPopup != null && packageList.Items.Count > 0;
        }


        private ObservableCollection<string> Items { get; set; }

        internal void ClearSettings()
        {
            _nugetSourcesService?.Dispose();
            _nugetSourcesService = null;
        }

        internal bool ApplyChangedSettings()
        {
            // get package sources as ordered list
            IReadOnlyDictionary<string, IReadOnlyList<string>> Patterns = new Dictionary<string, IReadOnlyList<string>>();
            PackageSourceMapping packageSourceMappings = new PackageSourceMapping(Patterns);
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            var settings = componentModel.GetService<ISettings>();
            PackageSourceMappingProvider packageSourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = ReadMappingsFromUIToConfig(SourceMappingsCollection);
            try
            {
                if (SourceMappingsChanged(_originalPackageSourceMappings, packageSourceMappingsSourceItems))
                {
                    packageSourceMappingProvider.SavePackageSourceMappings(packageSourceMappingsSourceItems); //in packagesourceprovider
                }
            }
            // Thrown during creating or saving NuGet.Config.
            catch (NuGetConfigurationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resx.ErrorDialogBoxTitle);
                return false;
            }
            // Thrown if no nuget.config found.
            catch (InvalidOperationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resx.ErrorDialogBoxTitle);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MessageHelper.ShowErrorMessage(Resx.ShowError_ConfigUnauthorizedAccess, Resx.ErrorDialogBoxTitle);
                return false;
            }
            // Unknown exception.
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Resx.ShowError_ApplySettingFailed, Resx.ErrorDialogBoxTitle);
                ActivityLog.LogError(NuGetUI.LogEntrySource, ex.ToString());
                return false;
            }

            return true;
        }

        // Returns true if there are no changes between existingSourceMappings and packageSourceMappings.
        private static bool SourceMappingsChanged(IReadOnlyList<PackageSourceMappingSourceItem> existingSourceMappings, IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappings)
        {
            if (existingSourceMappings.Count != packageSourceMappings.Count)
            {
                return true;
            }

            for (int i = 0; i < existingSourceMappings.Count; ++i)
            {
                if (!existingSourceMappings[i].Equals(packageSourceMappings[i]))
                {
                    return true;
                }
            }

            return false;
        }

        //returns list of mappings from config
        private IReadOnlyList<PackageSourceMappingSourceItem> GetOriginalMappings()
        {
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            var settings = componentModel.GetService<ISettings>();
            PackageSourceMappingProvider packageSourceMappingProvider = new PackageSourceMappingProvider(settings);
            return packageSourceMappingProvider.GetPackageSourceMappingItems();
        }

        //converts from list of packagesourcemappingsourceItems to a dictonary that can be read by UI
        private ItemsChangeObservableCollection<PackageItem> ReadMappingsFromConfigToUI(IReadOnlyList<PackageSourceMappingSourceItem> originalMappings)
        {
            Dictionary<string, ObservableCollection<PackageSourceContextInfo>> UISourceMappings = new Dictionary<string, ObservableCollection<PackageSourceContextInfo>>();
            foreach (PackageSourceMappingSourceItem sourceItem in originalMappings)
            {
                foreach (PackagePatternItem patternItem in sourceItem.Patterns)
                {
                    //do I need to check if list exists for that pattern yet?
                    if (!UISourceMappings.ContainsKey(patternItem.Pattern))
                    {
                        UISourceMappings[patternItem.Pattern] = new ObservableCollection<PackageSourceContextInfo>();
                    }
                    UISourceMappings[patternItem.Pattern].Add(new PackageSourceContextInfo(sourceItem.Key));
                }
            }
            ItemsChangeObservableCollection<PackageItem> mappingsCollection = new ItemsChangeObservableCollection<PackageItem>();
            foreach (string packageID in UISourceMappings.Keys)
            {
                PackageItem temp = new PackageItem(packageID, UISourceMappings[packageID]);
                mappingsCollection.Add(temp);
            }

            return mappingsCollection;
        }

        //converts from dictonary created by UI to list of packageSourceMappingSourceItems
        private ObservableCollection<PackageSourceMappingSourceItem> ReadMappingsFromUIToConfig(ItemsChangeObservableCollection<PackageItem> UISourceMappings)
        {
            Dictionary<string, ObservableCollection<PackagePatternItem>> mappingsDictonary = new Dictionary<string, ObservableCollection<PackagePatternItem>>();
            foreach (var packageItem in UISourceMappings)
            {
                foreach (var source in packageItem.GetSources())
                {
                    //Contains method did not work since diff instances of packageitem even though name is the same
                    //made own contains method
                    bool newSource = true;
                    foreach (var mapping in mappingsDictonary.Keys)
                    {
                        if (mapping == source.Name)
                        {
                            newSource = false;
                        }
                    }
                    if (newSource == true)
                    {
                        mappingsDictonary[source.Name] = new ObservableCollection<PackagePatternItem>();
                    }
                    PackagePatternItem tempID = new PackagePatternItem(packageItem.GetID());
                    bool newID = true;
                    foreach (var id in mappingsDictonary[source.Name])
                    {
                        if (id.Pattern == tempID.Pattern)
                        {
                            newID = false;
                        }
                    }
                    if (newID == true)
                    {
                        mappingsDictonary[source.Name].Add(tempID);
                    }
                }
            }

            //turn dictonary to observable collection of packageSourceMappingSourceItem
            ObservableCollection<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = new ObservableCollection<PackageSourceMappingSourceItem>();
            foreach (var source in mappingsDictonary.Keys)
            {
                packageSourceMappingsSourceItems.Add(new PackageSourceMappingSourceItem(source, mappingsDictonary[source]));
            }
            return packageSourceMappingsSourceItems;
        }
    }
}
