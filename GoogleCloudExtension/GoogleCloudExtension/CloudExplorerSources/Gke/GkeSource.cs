using GoogleCloudExtension.CloudExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CloudExplorerSources.Gke
{
    public class GkeSource : CloudExplorerSourceBase<GkeSourceRootViewModel>
    {
        public GkeSource(ICloudSourceContext context) : base(context)
        { }
    }
}
