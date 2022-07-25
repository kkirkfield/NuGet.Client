using System.Collections.Generic;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.Options
{
    public class MappingUIDisplay
    {
        public string ID { get; set; }
        public List<PackageSourceContextInfo> Sources { get; private set; }

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

        public MappingUIDisplay(string packageid, List<PackageSourceContextInfo> packageSources)
        {
            ID = packageid;
            Sources = packageSources;
        }
    }
}
