using GoogleCloudExtension.Deployment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.HostAbstraction.VS14
{
    class ToolsPathProvider : IToolsPathProvider
    {
        public string GetExternalToolsPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            return Path.Combine(programFilesPath, @"Microsoft Visual Studio 14.0\Web\External");
        }

        public string GetDotnetPath()
        {
            var programFilesPath = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
            return Path.Combine(programFilesPath, @"dotnet\dotnet.exe");
        }

        public string GetMsbuildPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            return Path.Combine(programFilesPath, @"MSBuild\14.0\Bin\MSBuild.exe");
        }

        public string GetMsdeployPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Path.Combine(programFilesPath, @"IIS\Microsoft Web Deploy V3\msdeploy.exe");
        }
    }
}
