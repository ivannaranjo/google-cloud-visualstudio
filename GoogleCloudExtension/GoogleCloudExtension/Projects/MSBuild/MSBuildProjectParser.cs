using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.Projects.MSBuild
{
    internal class MSBuildProjectParser
    {
        public string TargetFramework { get; private set; }

        public MSBuildProjectParser(string path)
        {
            ParseProject(path);
        }

        private void ParseProject(string path)
        {
            var project = new Microsoft.Build.Evaluation.Project(path);

            var targetFramework = project.Properties.FirstOrDefault(x => x.Name == "TargetFramework");
            TargetFramework = targetFramework?.EvaluatedValue;
        }
    }
}
