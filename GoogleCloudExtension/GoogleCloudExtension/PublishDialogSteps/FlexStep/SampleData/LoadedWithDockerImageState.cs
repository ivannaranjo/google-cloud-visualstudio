using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.PublishDialogSteps.FlexStep.SampleData
{
    public class LoadedWithDockerImageState : LoadedState
    {
        public string DockerImage { get; } = "gcr.io/project/image:tag";

        new public bool HasDockerImage { get; } = true;
    }
}
