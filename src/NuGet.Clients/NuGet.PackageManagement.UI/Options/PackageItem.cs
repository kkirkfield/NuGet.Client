using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace NuGet.Options
{
    internal class PackageItem
    {
        private string _id;
        private IList _sources;


        public PackageItem(string packageid, IList packagesources)
        {
            _id = packageid;
            _sources = packagesources;
        }
        public string GetID()
        {
            return _id;
        }

        public IList GetSources()
        {
            return _sources;
        }

        public override string ToString()
        {
            var tempString = "";
            tempString += "Package ID: ";
            tempString += _id;
            tempString += " Sources: ";
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
