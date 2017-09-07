using GoogleCloudExtension.CloudExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    public class GcrSource : CloudExplorerSourceBase<GcrSourceRootViewModel>
    {
        public GcrSource(ICloudSourceContext context) : base(context)
        { }
    }
}
