using GoogleCloudExtension.Deployment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.PublishDialog
{
    public class PublishDialogSource
    {
        public string ImageTag { get; }

        public IParsedProject Project { get; }

        public string DisplayName => ImageTag ?? Project?.Name;

        public PublishDialogSource(string imageTag)
        {
            ImageTag = imageTag;
        }

        public PublishDialogSource(IParsedProject project)
        {
            Project = project;
        }
    }
}
