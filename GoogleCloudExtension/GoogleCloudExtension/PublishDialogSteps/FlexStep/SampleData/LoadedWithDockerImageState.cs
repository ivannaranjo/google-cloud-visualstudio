using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.PublishDialogSteps.FlexStep.SampleData
{
    public class LoadedWithDockerImageState : LoadedState
    {
        public string AppYamlPath
        {
            get { return @"c:\somewhere\over\the\raimbow\app.yaml"; }
            set { }
        }

        new public bool HasDockerImage { get; } = true;
    }
}
