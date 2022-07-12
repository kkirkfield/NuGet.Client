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
using Microsoft.VisualStudio.Services.Common;
using Microsoft.Internal.VisualStudio.PlatformUI;

namespace NuGet.Options
{
    public partial class PackageSourceMappingOptionsControl : UserControl
    {
        public ItemsChangeObservableCollection<PackageItem> SourceMappingsCollection { get; private set; }

        private IReadOnlyList<PackageSourceMappingSourceItem> _originalPackageSourceMappings;

        public ICommand ShowButtonCommand { get; set; }

        public ICommand RemoveButtonCommand { get; set; }

        public ICommand ClearButtonCommand { get; set; }

        private AddMappingDialog _addMappingDialog;

        public PackageSourceMappingOptionsControl()
        {
            ShowButtonCommand = new ButtonCommand(ExecuteShowButtonCommand, CanExecuteShowButtonCommand);
            RemoveButtonCommand = new ButtonCommand(ExecuteRemoveButtonCommand, CanExecuteRemoveButtonCommand);
            ClearButtonCommand = new ButtonCommand(ExecuteClearButtonCommand, CanExecuteClearButtonCommand);
            SourceMappingsCollection = new ItemsChangeObservableCollection<PackageItem>();
            DataContext = this;
            InitializeComponent();
            (ShowButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
        }

        internal async Task InitializeOnActivatedAsync(CancellationToken cancellationToken)
        {
            IServiceBrokerProvider serviceBrokerProvider = await ServiceLocator.GetComponentModelServiceAsync<IServiceBrokerProvider>();
            IServiceBroker serviceBroker = await serviceBrokerProvider.GetAsync();

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
            (ClearButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
            (RemoveButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
        }
        private void ExecuteShowButtonCommand(object parameter)
        {
            _addMappingDialog = new AddMappingDialog(this);
            IntPtr parent = WindowHelper.GetDialogOwnerHandle();
            WindowHelper.ShowModal(_addMappingDialog, parent);
        }

        private bool CanExecuteShowButtonCommand(object parameter)
        {
            return true;
        }

        private void ExecuteRemoveButtonCommand(object parameter)
        {
            SourceMappingsCollection.Remove((PackageItem)packageList.SelectedItem);
            (ClearButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteRemoveButtonCommand(object parameter)
        {
            return SourceMappingsCollection.Count > 0;
        }

        private void ExecuteClearButtonCommand(object parameter)
        {
            SourceMappingsCollection.Clear();
            (RemoveButtonCommand as ButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteClearButtonCommand(object parameter)
        {
            return SourceMappingsCollection.Count > 0;
        }

        internal bool ApplyChangedSettings()
        {
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
                    packageSourceMappingProvider.SavePackageSourceMappings(packageSourceMappingsSourceItems);
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
                //checks to see if keys match
                if (existingSourceMappings[i] != packageSourceMappings[i])
                {
                    return true;
                }
                //checks to see if all patterns match
                if (existingSourceMappings[i].Patterns.Count != packageSourceMappings[i].Patterns.Count)
                {
                    return true;
                }
                for (int j = 0; j < existingSourceMappings[i].Patterns.Count; ++j)
                {
                    if (existingSourceMappings[i].Patterns[j] != packageSourceMappings[i].Patterns[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //converts from list of packagesourcemappingsourceItems to a dictonary that can be read by UI
        private ItemsChangeObservableCollection<PackageItem> ReadMappingsFromConfigToUI(IReadOnlyList<PackageSourceMappingSourceItem> originalMappings)
        {
            Dictionary<string, ObservableCollection<PackageSourceContextInfo>> UISourceMappings = new Dictionary<string, ObservableCollection<PackageSourceContextInfo>>();
            foreach (PackageSourceMappingSourceItem sourceItem in originalMappings)
            {
                foreach (PackagePatternItem patternItem in sourceItem.Patterns)
                {
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
                PackageSourceMappingSourceItem temp = new PackageSourceMappingSourceItem(source, mappingsDictonary[source]);
                packageSourceMappingsSourceItems.Add(temp);
            }
            return packageSourceMappingsSourceItems;
        }
    }
}
