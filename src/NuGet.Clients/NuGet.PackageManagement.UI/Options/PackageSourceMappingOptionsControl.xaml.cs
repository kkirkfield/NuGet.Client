// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using System.Threading.Tasks;

using Microsoft;
using Microsoft.ServiceHub.Framework;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Configuration;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Runtime;
using Resx = NuGet.PackageManagement.UI.Resources;


namespace NuGet.Options
{
    /// <summary>
    /// Interaction logic for PackageSourceMappingOptionsControl.xaml
    /// </summary>
    public partial class PackageSourceMappingOptionsControl : UserControl
    {

        public ItemsChangeObservableCollection<PackageSourceContextInfo> SourcesCollection { get; private set; } //change to package s

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

        private Dictionary<string, List<string>> _sourceMappings;

        public PackageSourceMappingOptionsControl()
        {
            ShowButtonCommand = new ShowButtonCommand(ExecuteShowButtonCommand, CanExecuteShowButtonCommand);

            HideButtonCommand = new HideButtonCommand(ExecuteHideButtonCommand, CanExecuteHideButtonCommand);

            AddButtonCommand = new AddButtonCommand(ExecuteAddButtonCommand, CanExecuteAddButtonCommand);

            RemoveButtonCommand = new RemoveButtonCommand(ExecuteRemoveButtonCommand, CanExecuteRemoveButtonCommand);

            ClearButtonCommand = new ClearButtonCommand(ExecuteClearButtonCommand, CanExecuteClearButtonCommand);



            // SourcesCollection = new ObservableCollection<object>();
            DataContext = this;
            SourcesCollection = new ItemsChangeObservableCollection<PackageSourceContextInfo>();



            InitializeComponent();

            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
            _sourceMappings = new Dictionary<string, List<string>>();
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


            _originalPackageSources = await _nugetSourcesService.GetPackageSourcesAsync(cancellationToken);

            //SourcesCollection = new ItemsChangeObservableCollection<PackageSourceContextInfo>(_originalPackageSources);
            foreach (var source in _originalPackageSources)
            {
                SourcesCollection.Add(source);
            }

            //SourcesCollection.Refresh();

            //add an event on sourcecollection --> collections changed

            //add a raised property changed

            //_nugetSourcesService?.Dispose();
            //_nugetSourcesService = null;
        }




        private void ExecuteShowButtonCommand(object parameter)
        {
            MyPopup.IsOpen = true;
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
            MyPopup.IsOpen = false;
            var tempPkgID = packageID.Text;
            List<string> temp = sourcesListBox.SelectedItems as List<string>;
            // temp = (List<string>)temp;
            //source mappings keeeps getting reset when okay button is clicked because it says new ... in the constructor
            //soln --> read directly from UI and not from a field in apply changed settings
            _sourceMappings[tempPkgID] = temp;

            // PackageItem tempPkg = new PackageItem(tempPkgID, temp);
            packageList.Items.Add(tempPkgID);
            //ReadPackageItemToDictonary(packageList);
            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
            (RemoveButtonCommand as RemoveButtonCommand).InvokeCanExecuteChanged();
            (ClearButtonCommand as ClearButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteAddButtonCommand(object parameter)
        {
            var tempPkg = packageID.Text;
            return !string.IsNullOrWhiteSpace(tempPkg);
        }

        private void ExecuteRemoveButtonCommand(object parameter)
        {
            packageList.Items.Remove(packageList.SelectedItem);
            (ClearButtonCommand as ClearButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteRemoveButtonCommand(object parameter)
        {
            return MyPopup != null && packageList.Items.Count > 0;
        }

        private void ExecuteClearButtonCommand(object parameter)
        {
            packageList.Items.Clear();
            (RemoveButtonCommand as RemoveButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteClearButtonCommand(object parameter)
        {
            return MyPopup != null && packageList.Items.Count > 0;
        }


        private ObservableCollection<string> Items { get; set; }

        internal void ClearSettings()
        {
            // clear this flag so that we will set up the bindings again when the option page is activated next time
            _nugetSourcesService?.Dispose();
            _nugetSourcesService = null;
        }

        internal bool ApplyChangedSettings()
        {
            // get package sources as ordered list
            //find name in xaml for packagesourceslistbox but for mappings
            //Might have to cast differently depending on how UI is implimented
            IReadOnlyDictionary<string, IReadOnlyList<string>> Patterns = new Dictionary<string, IReadOnlyList<string>>();
            PackageSourceMapping packageSourceMappings = new PackageSourceMapping(Patterns);


            //is this how you get the settings?
            //var currentDirectory = Directory.GetCurrentDirectory();
            //ISettings settings = global::NuGet.Configuration.Settings.LoadDefaultSettings(currentDirectory);
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            var settings = componentModel.GetService<ISettings>();
            PackageSourceMappingProvider packageSourceMappingProvider = new PackageSourceMappingProvider(settings);
            _originalPackageSourceMappings = packageSourceMappingProvider.GetPackageSourceMappingItems();

            //add try/catch
            //make packagesourcemappingsprovider object to call save...
            //^^need to find settings
            //

            //makes packageSourceMappings into a list of type PackageSourceMappingsSourceItems
            /*IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = null;
            foreach (var source in packageSourceMappings.Patterns.Keys)
            {
                IEnumerable<PackagePatternItem> patterns = null;
                var packagePatterns = packageSourceMappings.Patterns[source];
                foreach (var packagePattern in packagePatterns)
                {
                    PackagePatternItem patternItem = new PackagePatternItem(packagePattern);
                    patterns.Append(patternItem);
                }
                PackageSourceMappingSourceItem mappingSourceItem = new PackageSourceMappingSourceItem(source, patterns);
                packageSourceMappingsSourceItems.Append(mappingSourceItem);
            }*/

            //Dictionary<string, List<string>> packageList = new Dictionary<string, List<string>>();
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = ReadMappingsFromUIToConfig(_sourceMappings);



            try
            {
                //impliment sourcemappingschanged
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
        private Dictionary<string, List<string>> ReadMappingsFromConfigToUI(IReadOnlyList<PackageSourceMappingSourceItem> originalMappings)
        {
            Dictionary<string, List<string>> UISourceMappings = new Dictionary<string, List<string>>();
            foreach (PackageSourceMappingSourceItem sourceItem in originalMappings)
            {
                foreach (PackagePatternItem patternItem in sourceItem.Patterns)
                {
                    //do I need to check if list exists for that pattern yet?
                    UISourceMappings[patternItem.Pattern].Append(sourceItem.Key);
                }
            }
            return UISourceMappings;
        }

        //converts from dictonary created by UI to list of packageSourceMappingSourceItems
        private IReadOnlyList<PackageSourceMappingSourceItem> ReadMappingsFromUIToConfig(Dictionary<string, List<string>> UISourceMappings)
        {
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = null;
            foreach (var source in UISourceMappings.Keys)
            {
                IEnumerable<PackagePatternItem> patterns = null;
                var packagePatterns = UISourceMappings[source];
                foreach (var packagePattern in packagePatterns)
                {
                    PackagePatternItem patternItem = new PackagePatternItem(packagePattern);
                    patterns.Append(patternItem);
                }
                PackageSourceMappingSourceItem mappingSourceItem = new PackageSourceMappingSourceItem(source, patterns);
                packageSourceMappingsSourceItems.Append(mappingSourceItem);
            }
            return packageSourceMappingsSourceItems;
        }

        /* private Dictionary<string, List<string>> ReadPackageItemToDictonary(List<PackageItem> packageItemList)
        {
            Dictionary<string, List<string>> list = new Dictionary<string, List<string>>();
            foreach (PackageItem packageItem in packageItemList)
            {
                list[packageItem.GetID()] = (List<string>)packageItem.GetSources();
            }
            _sourceMappings = list;
            return list;
        }*/

    }
}
