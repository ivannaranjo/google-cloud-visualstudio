using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.PublishDialogSteps.FlexStep.SampleData
{
    public class LoadingState
    {
        public bool ShowInputControls { get; } = false;

        public bool LoadingProject { get; } = true;

        public bool NeedsApiEnabled { get; } = false;

        public bool NeedsAppCreated { get; } = false;

        public bool GeneralError { get; } = false;
    }
}
