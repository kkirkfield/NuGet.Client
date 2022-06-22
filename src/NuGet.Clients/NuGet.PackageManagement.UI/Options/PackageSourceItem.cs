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
        private PackageSourceContextInfo _sourceInfo;
        private bool _isChecked;

        public PackageSourceItem(PackageSourceContextInfo sourceInfo, bool isChecked)
        {
            _sourceInfo = sourceInfo;
            _isChecked = isChecked;
        }

        public bool IsChecked()
        {
            return _isChecked;
        }

        public PackageSourceContextInfo GetSourceInfo()
        {
            return _sourceInfo;
        }

        public override string ToString()
        {
            return _sourceInfo.ToString();
        }

        public void SetCheck()
        {
            _isChecked = true;
        }
    }
}
