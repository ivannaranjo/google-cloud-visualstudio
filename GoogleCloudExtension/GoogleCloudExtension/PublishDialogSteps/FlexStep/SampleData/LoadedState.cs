using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.PublishDialogSteps.FlexStep.SampleData
{
    public class LoadedState
    {
        public bool ShowInputControls { get; } = true;

        public bool LoadingProject { get; } = false;

        public bool NeedsApiEnabled { get; } = false;

        public bool NeedsAppCreated { get; } = false;

        public bool GeneralError { get; } = false;

        public bool HasDockerImage { get; } = false;
    }
}
