using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.PublishDialogSteps.FlexStep.SampleData
{
    public class NeedsApiEnabledState
    {
        public bool ShowInputControls { get; } = false;

        public bool LoadingProject { get; } = false;

        public bool NeedsApiEnabled { get; } = true;

        public bool NeedsAppCreated { get; } = false;

        public bool GeneralError { get; } = false;
    }
}
