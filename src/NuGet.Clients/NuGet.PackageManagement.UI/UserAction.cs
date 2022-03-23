// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Versioning;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Represents an action performed in PM UI
    /// </summary>
    public class UserAction
    {
        private UserAction(NuGetOperationType action, string packageId, NuGetVersion packageVersion, bool isSolutionLevel, ItemFilter activeTab, UIOperationSource uiSource)
        {
            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            Action = action;
            PackageId = packageId;
            Version = packageVersion;
            UIOperationsource = uiSource;
            ActiveTab = activeTab;
            IsSolutionLevel = isSolutionLevel;
        }

        public bool IsSolutionLevel { get; private set; }
        public ItemFilter ActiveTab { get; private set; }
        public UIOperationSource UIOperationsource { get; private set; }
        public NuGetOperationType Action { get; private set; }
        public string PackageId { get; }
        public NuGetVersion Version { get; }

        public static UserAction CreateInstallAction(string packageId, NuGetVersion packageVersion, bool isSolutionLevel, ItemFilter activeTab, UIOperationSource uiSource)
        {
            if (packageVersion == null)
            {
                throw new ArgumentNullException(nameof(packageVersion));
            }

            return new UserAction(NuGetOperationType.Install, packageId, packageVersion, isSolutionLevel, activeTab, uiSource);
        }

        public static UserAction CreateUnInstallAction(string packageId, bool isSolutionLevel, ItemFilter activeTab, UIOperationSource uiSource)
        {
            return new UserAction(NuGetOperationType.Uninstall, packageId, packageVersion: null, isSolutionLevel, activeTab, uiSource);
        }

        public static UserAction CreateUpdateAction(bool isSolutionLevel, ItemFilter activeTab, UIOperationSource uiSource)
        {
            return new UserAction(NuGetOperationType.Update, null, null, isSolutionLevel, activeTab, uiSource);
        }
    }
}
