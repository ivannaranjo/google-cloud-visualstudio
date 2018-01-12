using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.PublishDialogSteps.FlexStep.SampleData
{
    public class NeedsAppCreatedState
    {
        public bool ShowInputControls { get; } = false;

        public bool LoadingProject { get; } = false;

        public bool NeedsApiEnabled { get; } = false;

        public bool NeedsAppCreated { get; } = true;

        public bool GeneralError { get; } = false;
    }
}
