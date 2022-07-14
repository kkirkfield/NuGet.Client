using System.Collections.ObjectModel;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.Options
{
    public class PackageItem
    {
        public string ID { get; set; }
        public ObservableCollection<PackageSourceContextInfo> Sources { get; private set; }

        //View binds to this string
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

        public PackageItem(string packageid, ObservableCollection<PackageSourceContextInfo> packageSources)
        {
            ID = packageid;
            Sources = packageSources;
        }
    }
}
