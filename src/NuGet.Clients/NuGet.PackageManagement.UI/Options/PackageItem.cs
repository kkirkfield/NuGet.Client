using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.Options
{
    public class PackageItem
    {
        //public string ID { get; set; }
        //public ObservableCollection<PackageSourceContextInfo> _sources { get; private set; }
        private string _id;
        private ObservableCollection<PackageSourceContextInfo> _sources;

        public PackageItem(string packageid, ObservableCollection<PackageSourceContextInfo> package_sources)
        {
            _id = packageid;
            _sources = package_sources;
        }
        public string GetID()
        {
            return _id;
        }

        public ObservableCollection<PackageSourceContextInfo> GetSources()
        {
            return _sources;
        }

        public override string ToString()
        {
            var tempString = "";
            tempString += "Package ID: ";
            tempString += _id;
            tempString += "_sources: ";
            for (int i = 0; i < _sources.Count; i++)
            {
                tempString += _sources[i].ToString();
                if (i < _sources.Count - 1)
                {
                    tempString += ", ";
                }
            }
            return tempString;
        }
    }

}
