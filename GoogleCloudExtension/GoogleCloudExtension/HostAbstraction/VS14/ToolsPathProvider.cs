using GoogleCloudExtension.Deployment;
using GoogleCloudExtension.Utils;
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
            var result = Path.Combine(programFilesPath, @"Microsoft Visual Studio 14.0\Web\External");
#if DEBUG
            GcpOutputWindow.OutputLine($"External tools path: {result}");
#endif
            return result;
        }

        public string GetDotnetPath()
        {
            var programFilesPath = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
            var result = Path.Combine(programFilesPath, @"dotnet\dotnet.exe");
#if DEBUG
            GcpOutputWindow.OutputLine($"Dotnet path: {result}");
#endif
            return result;
        }

        public string GetMsbuildPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var result = Path.Combine(programFilesPath, @"MSBuild\14.0\Bin\MSBuild.exe");
#if DEBUG
            GcpOutputWindow.OutputLine($"Msbuild path: {result}");
#endif
            return result;
        }

        public string GetMsdeployPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var result = Path.Combine(programFilesPath, @"IIS\Microsoft Web Deploy V3\msdeploy.exe");
#if DEBUG
            GcpOutputWindow.OutputLine($"Msdeploy path: {result}");
#endif
            return result;
        }
    }
}
