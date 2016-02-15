using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace UPH.Service
{
    [RunInstaller(true)]
    public partial class UPHServiceInstaller : Installer
    {
        public UPHServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
