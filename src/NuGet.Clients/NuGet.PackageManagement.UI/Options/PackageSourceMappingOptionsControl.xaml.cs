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
            DataContext = this;
            SourcesCollection = new ItemsChangeObservableCollection<PackageSourceItem>();

            SourceMappingsCollection = new ItemsChangeObservableCollection<PackageItem>();

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
            foreach (var item in SourceMappingsCollectiontemp)
            {
                SourceMappingsCollection.Add(item);
            }

            //var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);

            //var settings = componentModel.GetService<ISettings>();
            //PackageSourceMappingProvider packageSourceMappingProvider = new PackageSourceMappingProvider(settings);
            // _originalPackageSourceMappings = packageSourceMappingProvider.GetPackageSourceMappingItems();

            //SourcesCollection.AddRange(_originalPackageSources);
            //SourcesCollection = new ItemsChangeObservableCollection<PackageSourceContextItem>(_originalPackageSources);
            //SourcesCollection.Refresh();

            //add an event on sourcecollection --> collections changed

            //add a raised property changed

            //_nugetSourcesService?.Dispose();
            //_nugetSourcesService = null;
        }

        /*private void ExecuteIsCheckedCommand(object parameter)
        {
            
        }

        private bool CanExecuteIsCheckedCommand(object parameter)
        {
            return true;
        }*/




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
            ObservableCollection<PackageSourceContextInfo> tempSources = new ObservableCollection<PackageSourceContextInfo>();
            foreach (PackageSourceItem source in sourcesListBox.Items)
            {
                if (source.IsChecked)
                {
                    tempSources.Add(source.SourceInfo);
                    //MessageBox.Show(source.GetSourceInfo().ToString());
                }
            }


            // List<string> temp = sourcesListBox.SelectedItem as List<string>;
            // temp = (List<string>)temp;
            //source mappings keeeps getting reset when okay button is clicked because it says new ... in the constructor
            //soln --> read directly from UI and not from a field in apply changed settings
            _sourceMappings[tempPkgID] = tempSources;
            PackageItem tempPkg = new PackageItem(tempPkgID, tempSources);
            //packageList.Items.Add(tempPkg);
            SourceMappingsCollection.Add(tempPkg);
            //sourceList.Items.Add(temp);
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
            // _originalPackageSourceMappings = packageSourceMappingProvider.GetPackageSourceMappingItems();

            //add try/catch
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = ReadMappingsFromUIToConfig(SourceMappingsCollection);



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
            //why does it exit when it returns false??
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
                    if (!UISourceMappings.ContainsKey(sourceItem.Key))
                    {
                        UISourceMappings[sourceItem.Key] = new ObservableCollection<PackageSourceContextInfo>();
                    }
                    UISourceMappings[sourceItem.Key].Add(new PackageSourceContextInfo(patternItem.Pattern));
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
            ObservableCollection<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = new ObservableCollection<PackageSourceMappingSourceItem>();
            foreach (var packageItem in UISourceMappings)
            {
                ObservableCollection<PackagePatternItem> sources = new ObservableCollection<PackagePatternItem>();
                foreach (var source in packageItem.GetSources())
                {
                    PackagePatternItem temp = new PackagePatternItem(source.Name);
                    sources.Add(temp);
                }
                PackageSourceMappingSourceItem mappingSourceItem = new PackageSourceMappingSourceItem(packageItem.GetID(), sources);
                packageSourceMappingsSourceItems.Add(mappingSourceItem);
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
