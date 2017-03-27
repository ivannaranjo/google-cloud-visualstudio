using GoogleCloudExtension.Deployment;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.HostAbstraction.VS15
{
    class ToolsPathProvider : IToolsPathProvider
    {
        private readonly string _edition;

        public ToolsPathProvider(string edition)
        {
            _edition = edition;
        }

        public string GetDotnetPath()
        {
            throw new NotImplementedException();
        }

        public string GetExternalToolsPath()
        {
            throw new NotImplementedException();
        }

        public string GetMsbuildPath()
        {
            var programFilesPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            var result = Path.Combine(programFilesPath, $@"Microsoft Visual Studio\2017\{_edition}\MSBuild\15.0\Bin\MSBuild.exe");
#if DEBUG
            GcpOutputWindow.OutputLine($"Program Files: {programFilesPath}");
            GcpOutputWindow.OutputLine($"Msbuild V15 Path: {result}");
#endif
            return result;
        }

        public string GetMsdeployPath()
        {
            var programFilesPath = Environment.GetEnvironmentVariable("ProgramFiles");
            var result = Path.Combine(programFilesPath, @"IIS\Microsoft Web Deploy V3\msdeploy.exe");
#if DEBUG
            GcpOutputWindow.OutputLine($"Program Files: {programFilesPath}");
            GcpOutputWindow.OutputLine($"Msdeploy V15 path: {result}");
#endif
            return result;
        }
    }
}
