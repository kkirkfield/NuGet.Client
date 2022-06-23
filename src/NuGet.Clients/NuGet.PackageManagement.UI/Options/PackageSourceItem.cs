// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.Options
{
    public class PackageSourceItem
    {
        public PackageSourceContextInfo SourceInfo { get; private set; }
        public bool IsChecked { get; set; }


        public PackageSourceItem(PackageSourceContextInfo sourceInfo, bool isChecked)
        {
            SourceInfo = sourceInfo;
            IsChecked = isChecked;
        }
    }
}
