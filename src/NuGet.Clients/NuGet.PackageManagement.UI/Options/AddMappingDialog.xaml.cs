// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.PlatformUI;

namespace NuGet.Options
{
    /// <summary>
    /// Interaction logic for AddMappingDialog.xaml
    /// </summary>
    public partial class AddMappingDialog : UserControl
    {
        public AddMappingDialog()
        {
            InitializeComponent();
        }

        // Allows the user to drag the window around
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            //this.DragMove();
        }

    }
}
