using System.Collections.ObjectModel;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.Options
{
    public class PackageItem
    {
        public string ID { get; set; }
        public ObservableCollection<PackageSourceContextInfo> Sources { get; private set; }

        public string SourcesString
        {
            get
            {
                string sourcesString = "";
                for (int i = 0; i < Sources.Count; i++)
                {
                    sourcesString += Sources[i].Name;
                    if (i < Sources.Count - 1)
                    {
                        sourcesString += ", ";
                    }
                }
                return sourcesString;
            }
        }
        /// <summary>
        /// /private string ID;
        /// </summary>
        //private ObservableCollection<PackageSourceContextInfo> Sources;

        public PackageItem(string packageid, ObservableCollection<PackageSourceContextInfo> packageSources)
        {
            ID = packageid;
            Sources = packageSources;
        }
        public string GetID()
        {
            return ID;
        }

        public ObservableCollection<PackageSourceContextInfo> GetSources()
        {
            return Sources;
        }

        public override string ToString()
        {
            var tempString = "";
            tempString += "Package ID: ";
            tempString += ID;
            tempString += " Sources: ";
            for (int i = 0; i < Sources.Count; i++)
            {
                tempString += Sources[i].ToString();
                if (i < Sources.Count - 1)
                {
                    tempString += ", ";
                }
            }
            return tempString;
        }
    }

}
